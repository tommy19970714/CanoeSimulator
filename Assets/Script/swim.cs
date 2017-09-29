using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swim : MonoBehaviour {

	public GameObject canue;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (1f, 2f, 0f));
		Vector3 canuePosition = canue.transform.position;
		canuePosition.y += 0.9f;
		transform.position = canuePosition;
	}
}
