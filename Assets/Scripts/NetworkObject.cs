using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net.Sockets;
using Network;

public class NetworkObject : MonoBehaviour {
	// Expose these values just for debugging purpose
	public int localID = 0;
	public float m_SyncRate = 1f;
	public bool isLocal = false;

	void Start() {
		Debug.Log("Assigned ID: " + this.localID);
		// StartCoroutine(CoSyncTransform());
	}

	IEnumerator CoSyncTransform() {
		while(true) {
			byte[] data = new byte[sizeof(float) * 7];
			ByteWriter byteWriter = new ByteWriter(data);
			byteWriter.WriteVector3(transform.position);
			byteWriter.WriteQuaternion(transform.rotation);

			// NetworkManager.Instance.SendMessage(localID, MessageType.SyncTransform, data);
			yield return new WaitForSecondsRealtime(m_SyncRate);
		}
	}
}
