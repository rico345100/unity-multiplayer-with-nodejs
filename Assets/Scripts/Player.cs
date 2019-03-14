using UnityEngine;
using Network;

public class Player : MonoBehaviour {
	private NetworkObject m_NetworkObject;
	private Vector3 m_SyncPos;
	private Quaternion m_SyncRot;
	public float syncSmooth = 10.0f;

	void Start() {
		m_NetworkObject = GetComponent<NetworkObject>();
		m_SyncPos = transform.position;
		m_SyncRot = transform.rotation;

		m_NetworkObject.onReceivedBytes.AddListener(UpdateTransform);
		m_NetworkObject.onSendingBytes.AddListener(SendTransform);
		
		if(!m_NetworkObject.isLocal) {
			WheelCollider[] wheelColliders = GetComponentsInChildren<WheelCollider>();

			foreach(WheelCollider wheelCollider in wheelColliders) {
				Destroy(wheelCollider);
			}

			Destroy(GetComponent<Dot_Truck_Controller>());
			Destroy(GetComponent<Rigidbody>());
			Destroy(transform.Find("Main Camera").gameObject);
		}
	}

	void FixedUpdate() {
		if(m_NetworkObject.isLocal) return;

		transform.position = Vector3.Lerp(transform.position, m_SyncPos, Time.fixedDeltaTime * syncSmooth);
		transform.rotation = Quaternion.Lerp(transform.rotation, m_SyncRot, Time.fixedDeltaTime * syncSmooth);
	}

	void SendTransform() {
		byte[] data = new byte[sizeof(int) + sizeof(float) * 7];
		ByteWriter byteWriter = new ByteWriter(data);
		byteWriter.WriteInt(m_NetworkObject.localID);
		byteWriter.WriteVector3(transform.position);
		byteWriter.WriteQuaternion(transform.rotation);

		m_NetworkObject.SendBytes(MessageType.SyncTransform, data);
	}

	void UpdateTransform(byte[] data) {
		ByteReader byteReader = new ByteReader(data);
		byteReader.ReadByte(); // MessageType
		byteReader.ReadInt();	// clientID
		byteReader.ReadInt();	// localID
		m_SyncPos = byteReader.ReadVector3();
		m_SyncRot = byteReader.ReadQuaternion();
	}
}
