using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Network;
using UnityEngine;
using UnityEngine.Events;

public class NetworkManager : MonoBehaviour {
	private static NetworkManager m_Instance;
	private static int localIDCounter = 0;
	private Socket m_Socket;
	private bool m_Active = false;
	private List<NetworkObject> m_NetworkObjects = new List<NetworkObject>();

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
	public List<NetworkObject> NetworkObjects {
		get {
			return m_NetworkObjects;
		}
	}

	[Header("Network Prefabs")]
	public GameObject playerPrefab;

	[HideInInspector]
	public UnityEvent onConnected;

	public static int AssignLocalID() {
		return localIDCounter++;
	}

	void Awake() {
		if(onConnected == null) {
			onConnected = new UnityEvent();
		}

		m_Instance = this;
	}

	IEnumerator Start() {
		// Give Some delays to other GameObject can set Event handlers
		yield return new WaitForSecondsRealtime(1f);

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
		int clientID = byteReader.ReadInt();
		int localID = byteReader.ReadInt();

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

	public void BroadcastMessage(MessageType messageType, byte[] data) {
		// Byte Order
		// byte MessageType
		// int clientID
		// byte[] data
		byte[] sendingData = new byte[sizeof(byte) + sizeof(int) + data.Length];
		ByteWriter byteWriter = new ByteWriter(sendingData);
		byteWriter.WriteByte((byte) messageType);
		byteWriter.WriteInt(this.clientID);
		byteWriter.WriteBytes(data);

		try {
			m_Socket.Send(sendingData, sendingData.Length, 0);
		}
		catch(SocketException e) {
			Debug.Log("Failed to send: " + e.ToString());
		}
	}

	public GameObject Instantiate(InstantiateType instantiateType, Vector3 spawnPos, Quaternion spawnRot) {
		// Byte Order
		// int localID
		// Vector3 instantiateType
		// Vecotr3 spawnPos
		// Quaternion spawnRot
		byte[] sendingData = new byte[sizeof(int) + sizeof(byte) + sizeof(float) * 7];
		ByteWriter byteWriter = new ByteWriter(sendingData);
		byteWriter.WriteInt(localIDCounter);
		byteWriter.WriteByte((byte) instantiateType);
		byteWriter.WriteVector3(spawnPos);
		byteWriter.WriteQuaternion(spawnRot);

		Debug.Log("Instantiating Object...");
		Debug.Log("Assigned LocalID: " + (localIDCounter));

		BroadcastMessage(MessageType.Instantiate, sendingData);

		// Actual instantiate from Unity
		GameObject instance = GameObject.Instantiate(GetPrefab(instantiateType), spawnPos, spawnRot);

		NetworkObject networkObject = instance.GetComponent<NetworkObject>();

		if(networkObject == null) {
			throw new System.NullReferenceException("Object must have NetworkObject Component.");
		}

		networkObject.isLocal = true;
		networkObject.localID = localIDCounter;

		m_NetworkObjects.Add(networkObject);

		// Increase LocalID Counter
		localIDCounter++;

		return instance;
	}

	GameObject InstantiateFromNetwork(InstantiateType instantiateType, int clientID, int localID, Vector3 spawnPos, Quaternion spawnRot) {
		Debug.Log("Got Instantiate Message");
		Debug.Log("ClientID: " + clientID);
		Debug.Log("LocalID: " + localID);
		Debug.Log("Type: " + instantiateType);

		// TODO: Instantiate
		// TODO: Set isLocal false
		// TODO: Set clientID and localID
		// TODO: Set Transform

		return null;
	}

	void OnApplicationQuit() {
		if(m_Socket != null) {
			m_Socket.Close();
			m_Socket = null;
		}
	}
}
