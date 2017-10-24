using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostNetworkManager : NetworkManager {

	// Use this for initialization
	void Awake () {
        this.StartHost();
        Debug.Log(this.networkAddress);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
