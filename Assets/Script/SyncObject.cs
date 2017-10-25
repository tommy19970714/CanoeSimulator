using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncObject : MonoBehaviour {

    public GameObject syncObject;
    

    // Use this for initialization
	void Start () {
        this.transform.position = new Vector3(240, 80, 60);
        syncObject = GameObject.Find("/SteamVR/[CameraRig]/Controller (right)");
        StartCoroutine("Loop");
    }
	
	// Update is called once per frame
	void Update () {
        if(syncObject != null)
        {
            this.transform.rotation = syncObject.transform.rotation;
            Vector3 pos = syncObject.transform.position;
            this.transform.position = pos;
        }
    }

    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (syncObject == null)
            {
                syncObject = GameObject.Find("/SteamVR/[CameraRig]/Controller (right)");
            }
        }
    }
}