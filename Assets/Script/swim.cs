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
		transform.Rotate (new Vector3 (1f, 0, 2.5f));
		//Vector3 canuePosition = canue.transform.position;
		//canuePosition.y += 0.3f;
		//transform.position = canuePosition;
	}
}
