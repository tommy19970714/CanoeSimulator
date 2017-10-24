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
        this.transform.rotation = cameraDriver.transform.rotation;

        //Quaternion tmp = cameraDriver.transform.rotation;
        //tmp.y = cameraDriver.transform.rotation.y;
        //tmp.x = cameraDriver.transform.rotation.x;
        //this.transform.rotation = tmp;

        Vector3 pos = cameraDriver.transform.position;
        this.transform.position = pos;
    }
}
