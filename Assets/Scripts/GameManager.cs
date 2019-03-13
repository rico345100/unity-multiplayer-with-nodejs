using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class GameManager : MonoBehaviour {
	[Header("Lobby Refs")]
	public GameObject lobbyCam;
	
	[Header("Game Refs")]
	public Transform spawnPoint;

	void Start() {
		NetworkManager.Instance.onConnected.AddListener(() => {
			NetworkManager.Instance.Log("Connected.");
			NetworkManager.Instance.Log("Creating Player...");

			lobbyCam.SetActive(false);

			// Spawn next to the previous player
			Vector3 newPos = new Vector3(
				spawnPoint.position.x + (NetworkManager.Instance.clientID * 10),
				spawnPoint.position.y,
				spawnPoint.position.z
			);
			NetworkManager.Instance.Instantiate(InstantiateType.Player, newPos, spawnPoint.rotation);
		});
	}
}
