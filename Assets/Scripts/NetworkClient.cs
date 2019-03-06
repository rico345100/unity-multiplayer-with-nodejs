using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net.Sockets;
using NetworkUtil;

public class NetworkClient : MonoBehaviour {
	public int objectID = 0;
	public float m_SyncRate = 1f;
	public bool isLocal = false;

	void Start() {
		Debug.Log("Assigned ObjectID: " + this.objectID);
		// StartCoroutine(CoSyncTransform());
	}

	IEnumerator CoSyncTransform() {
		while(true) {
			byte[] data = new byte[sizeof(float) * 7];
			ByteWriter byteWriter = new ByteWriter(data);
			byteWriter.WriteVector3(transform.position);
			byteWriter.WriteQuaternion(transform.rotation);

			// NetworkManager.Instance.SendMessage(objectID, MessageType.SyncTransform, data);
			yield return new WaitForSecondsRealtime(m_SyncRate);
		}
	}
}
