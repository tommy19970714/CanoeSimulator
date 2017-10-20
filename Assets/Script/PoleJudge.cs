using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleJudge : MonoBehaviour {

	public GameController gameController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Canoe"))
		{
			gameController.addCounter (this.GetInstanceID());
			Debug.Log (this.GetInstanceID());
		}
	}
}
