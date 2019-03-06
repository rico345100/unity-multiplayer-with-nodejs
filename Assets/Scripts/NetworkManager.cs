using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using NetworkUtil;
using UnityEngine;
using UnityEngine.Events;

public class NetworkManager : MonoBehaviour {
	private static NetworkManager m_Instance;
	private static int objectId = 0;
	private Socket m_Socket;
	private bool m_Active = false;
	private List<NetworkClient> m_NetworkClients = new List<NetworkClient>();

	[Header("Network Settings")]
	public string ipAddress = "127.0.0.1";
	public const int port = 1337;
	public int clientID;
	public bool Active {
		get {
			return m_Active;
		}
		set {
			m_Active = value;
		}
	}
	public static NetworkManager Instance {
		get { 
			return m_Instance;
		}
	}
	public Socket Socket {
		get {
			return m_Socket;
		}
	}
	public List<NetworkClient> NetworkClients {
		get {
			return m_NetworkClients;
		}
	}

	[Header("Network Prefabs")]
	public GameObject playerPrefab;

	[HideInInspector]
	public UnityEvent onConnected;

	public static int AssignObjectID() {
		return objectId++;
	}

	void Awake() {
		if(onConnected == null) {
			onConnected = new UnityEvent();
		}

		m_Instance = this;
	}

	void Start() {
		InitializeSocket();
		Connect();
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

			ReceiveID();
		}
		catch(SocketException e) {
			Debug.Log("Failed to connect: " + e.ToString());
		}
	}

	void ReceiveID() {
		try {
			byte[] recvBytes = new byte[2000];
			m_Socket.Receive(recvBytes);

			ByteReader byteReader = new ByteReader(recvBytes);
			this.clientID = byteReader.ReadInt();
			Debug.Log("Received ClientID: " + this.clientID);

			onConnected.Invoke();

			// ReceiveTrasmissions();
		}
		catch(SocketException e) {
			Debug.Log("Failed to receive: " + e.ToString());
		}
	}

	void ReceiveTrasmissions() {
		try {
			byte[] recvBytes = new byte[2000];
			m_Socket.Receive(recvBytes);

			DispatchMessage(recvBytes);

			// TODO: Keep listening. Find way to prevent thread lock
			// Continue
			// ReceiveTrasmissions();
		}
		catch(SocketException e) {
			Debug.Log("Failed to receive: " + e.ToString());
		}
	}

	void DispatchMessage(byte[] data) {
		ByteReader byteReader = new ByteReader(data);
		MessageType messageType = (MessageType) byteReader.ReadByte();
		int senderID = byteReader.ReadInt();
		int objectID = byteReader.ReadInt();

		Debug.Log("Receive Message");
		Debug.Log("MessageType: " + messageType);

		// Dispatch
		switch(messageType) {
			case MessageType.Instantiate:
				// TODO: Implement Instantiation
				break;
			case MessageType.SyncTransform:
				// TODO: Implement Sync Transform
				break;
			default:
				throw new System.InvalidOperationException("Unknown MessageType " + messageType);
		}
	}

	GameObject GetPrefab(InstantiateType type) {
		switch(type) {
			case InstantiateType.Player:
				return playerPrefab;
			default:
				throw new System.InvalidOperationException("Unknown InstantiateType " + type);
		}
	}

	public void BroadcastMessage(int objectId, MessageType messageType, byte[] data) {
		byte[] sendingData = new byte[1 + sizeof(int) * 2 + data.Length];
		ByteWriter byteWriter = new ByteWriter(sendingData);
		byteWriter.WriteByte((byte) messageType);
		byteWriter.WriteInt(this.clientID);
		byteWriter.WriteInt(objectId);
		byteWriter.WriteBytes(data);

		try {
			m_Socket.Send(sendingData, sendingData.Length, 0);
		}
		catch(SocketException e) {
			Debug.Log("Failed to send: " + e.ToString());
		}
	}

	void OnApplicationQuit() {
		if(m_Socket != null) {
			m_Socket.Close();
			m_Socket = null;
		}
	}
}
