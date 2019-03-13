using UnityEngine;

public class Player : MonoBehaviour {
	void Start() {
		NetworkObject networkObject = GetComponent<NetworkObject>();
		
		if(!networkObject.isLocal) {
			Destroy(GetComponent<Dot_Truck_Controller>());
			Destroy(transform.Find("Main Camera").gameObject);
		}
	}
}
