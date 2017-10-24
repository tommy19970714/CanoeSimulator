using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncObject : MonoBehaviour {

    public GameObject syncObject;
	
    // Use this for initialization
	void Start () {
        syncObject = GameObject.Find("/SteamVR/[CameraRig]/Controller (right)");
        Debug.Log(syncObject);
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.rotation = syncObject.transform.rotation;
        Vector3 pos = syncObject.transform.position;
        this.transform.position = pos;
    }
}