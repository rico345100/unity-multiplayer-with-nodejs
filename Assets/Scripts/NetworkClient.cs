using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NetworkUtil;

public class NetworkClient : MonoBehaviour {
	private Socket m_Socket;
	public string ipAddress = "127.0.0.1";
	public const int port = 1337;
	public float m_SyncRate = 1f;

	void Start() {
		InitializeSocket();
		Connect();
		
		StartCoroutine(CoSyncTransform());
	}

	void InitializeSocket() {
		m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 10000);
		m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 10000);
	}

	void Connect() {
		try {
			IPAddress ipAddr = System.Net.IPAddress.Parse(ipAddress);
			IPEndPoint ipEndPoint = new System.Net.IPEndPoint(ipAddr, port);
			m_Socket.Connect(ipEndPoint);
		}
		catch(SocketException e) {
			Debug.Log("Failed to connect: " + e.ToString());
		}
	}

	IEnumerator CoSyncTransform() {
		while(true) {
			if(m_Socket == null) yield break;

			byte[] data = new byte[1 + sizeof(float) * 7];
			data[0] = (byte) MessageType.SyncTransform;

			ByteWriter byteWriter = new ByteWriter(data, 1);
			byteWriter.WriteVector3(transform.position);
			byteWriter.WriteQuaternion(transform.rotation);

			try {
				m_Socket.Send(data, data.Length, 0);
			}
			catch(SocketException e) {
				Debug.Log("Failed to send: " + e.ToString());
			}

			yield return new WaitForSecondsRealtime(m_SyncRate);
		}
	}

	void OnApplicationQuit() {
		if(m_Socket != null) {
			m_Socket.Close();
			m_Socket = null;
		}
	}

	// TODO: Implement receive section
	// void ReceiveTestData() {
	// 	try {
	// 		byte[] recvBytes = new byte[2000];
	// 		m_Socket.Receive(recvBytes);

	// 		string decodedData = Encoding.Default.GetString(recvBytes);
	// 		Debug.Log(string.Format("Received: {0}", decodedData));
	// 	}
	// 	catch(SocketException e) {
	// 		Debug.Log("Failed to receive: " + e.ToString());
	// 	}
	// }
}
