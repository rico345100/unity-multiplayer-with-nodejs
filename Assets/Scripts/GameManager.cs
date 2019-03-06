using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkUtil;

public class GameManager : MonoBehaviour {
	[Header("Lobby Refs")]
	public GameObject lobbyCam;
	
	[Header("Game Refs")]
	public Transform spawnPoint;

	void Start() {
		NetworkManager.Instance.onConnected.AddListener(() => {
			lobbyCam.SetActive(false);
			// TODO: Spawn Player through Network
			
			// Test Send
			byte[] data = new byte[sizeof(float) * 7];
			ByteWriter byteWriter = new ByteWriter(data);
			byteWriter.WriteVector3(transform.position);
			byteWriter.WriteQuaternion(transform.rotation);

			NetworkManager.Instance.BroadcastMessage(0, MessageType.SyncTransform, data);
		});
	}
}
