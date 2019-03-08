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
			Debug.Log("Connected to Network!");
			Debug.Log("Creating Player...");

			lobbyCam.SetActive(false);
			NetworkManager.Instance.Instantiate(InstantiateType.Player, spawnPoint.position, spawnPoint.rotation);
		});
	}
}
