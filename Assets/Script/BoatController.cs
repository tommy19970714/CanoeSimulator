using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour {

	public bool inputMouseKey0 = false;
	public bool inputKeySHIFTL = false;
	public bool inputKeySPACE = false;
	public bool inputKeyW = false;
	public bool inputKeyS = false;
	public bool inputKeyA = false;
	public bool inputKeyD = false;
	public bool inputKeyF = false;
	public bool inputKeyQ = false;
	public bool inputKeyE = false;
	public bool inputKeyESC = false;
	public bool inputMouseKey1 = false;
	public float inputMouseX = 0.0f;
	public float inputMouseY = 0.0f;
	public float inputMouseWheel = 0.0f;

	private float useSpeed = 0.0f;
	private float useSideSpeed = 0.0f;
	private float moveSpeed = 0.05f;
	private float moveForward = 0.0f;
	private float moveSideways = 0.0f;

	public float speed = 3.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

//		//---------------------------------
//		//  GET KEYBOARD AND MOUSE INPUTS
//		//---------------------------------
//
//		//"WASD" MOVEMENT KEYS
//		inputKeyW = Input.GetKey("w");
//		inputKeyS = Input.GetKey("s");
//		inputKeyA = Input.GetKey("a");
//		inputKeyD = Input.GetKey("d");
//
//		//"QE" KEYS
//		inputKeyQ = Input.GetKey("q");
//		inputKeyE = Input.GetKey("e");
//
//		//LEFT MOUSE BUTTON
//		inputMouseKey0 = Input.GetKey("mouse 0");
//
//		//RIGHT MOUSE BUTTON
//		inputMouseKey1 = Input.GetKey("mouse 1");
//
//		//GET MOUSE MOVEMENT and SCROLLWHEEL
//		inputMouseX = Input.GetAxisRaw("Mouse X");
//		inputMouseY = Input.GetAxisRaw("Mouse Y");
//		inputMouseWheel = Input.GetAxisRaw("Mouse ScrollWheel");
//
//		//EXTRA KEYS
//		inputKeySHIFTL = Input.GetKey("left shift");
//		inputKeySPACE = Input.GetKey("space");
//		inputKeyF = Input.GetKey("f");
//		inputKeyESC = Input.GetKey("escape");
//
//
//		//---------------------------------
//		//  Speed control
//		//---------------------------------
//		moveForward = 0.0f;
//		moveSideways = 0.0f;
//		if (inputKeyW) {
//			moveForward = 1.0f;
//			Debug.Log ("D");
//		}
//		if (inputKeyS) {
//			moveForward = -1.0f;
//		}
//		if (inputKeyA) {
//			moveSideways = -1.0f;
//		}
//		if (inputKeyD) {
//			moveSideways = 1.0f;
//		}
//
//		float spdLerp = 5.0f;
//		if (moveForward > 0.0f) {
//			spdLerp = 2.5f;
//		}
//		if (moveForward != 0.0f && moveSideways != 0.0f) {
//			moveSpeed *= 0.75f;
//		}
//		useSpeed = Mathf.Lerp(useSpeed, (moveSpeed * moveForward), Time.deltaTime*spdLerp);
//		useSideSpeed = Mathf.Lerp(useSideSpeed, (moveSpeed * moveSideways), Time.deltaTime*spdLerp);
//		Debug.Log (useSpeed.ToString()+","+useSideSpeed.ToString());
//		Vector3 pos = this.transform.position;
//		pos.x += useSpeed;
//		pos.z += useSideSpeed;
//		this.transform.position = pos;

		if (Input.GetKey ("up")) {
			transform.position -= transform.right * speed * Time.deltaTime;
		}
		if (Input.GetKey ("down")) {
			transform.position += transform.right * speed * Time.deltaTime;
		}
		if (Input.GetKey("right")) {
			transform.position += transform.forward * speed * Time.deltaTime;
		}
		if (Input.GetKey ("left")) {
			transform.position -= transform.forward * speed * Time.deltaTime;
		}
	}
}
