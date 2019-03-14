using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Net.Sockets;
using Network;

public class NetworkEvent: UnityEvent<byte[]> {}

public class NetworkObject : MonoBehaviour {
	// Expose these values just for debugging purpose
	public int clientID;
	public int localID;
	public float m_SyncRate = 0.1f;
	public bool isLocal = false;
	
	public NetworkEvent onReceivedBytes;
	public UnityEvent onSendingBytes;

	void Awake() {
		if(onReceivedBytes == null) {
			onReceivedBytes = new NetworkEvent();
		}
		if(onSendingBytes == null) {
			onSendingBytes = new UnityEvent();
		}
	}

	void Start() {
		if(!isLocal) return;
		StartCoroutine(CoStartSync());
	}

	IEnumerator CoStartSync() {
		m_SyncRate = Mathf.Max(0.1f, m_SyncRate);	// Set minimum sync rate to 0.1 second

		while(true) {
			onSendingBytes.Invoke();
			yield return new WaitForSecondsRealtime(m_SyncRate);
		}
	}

	public void SendBytes(MessageType messageType, byte[] data) {
		if(data == null) return;
		NetworkManager.Instance.SendMessage(messageType, data);
	}

	public void ReceiveBytes(byte[] data) {
		onReceivedBytes.Invoke(data);
	}
}
