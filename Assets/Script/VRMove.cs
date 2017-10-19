using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMove : MonoBehaviour {

    public GameObject cameraDriver;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = cameraDriver.transform.position;
        pos.y += 1.0f;
        this.transform.position = pos;
    }
}
