using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleJudge : MonoBehaviour {

	public GameController gameController;

	// Use this for initialization
	void Awake () {
        GameObject gameControllerObject = GameObject.Find("/GameController");
        gameController = gameControllerObject.GetComponent<GameController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
        Debug.Log("Ontrigger"); 
        Debug.Log(other.transform.tag);
		if (other.gameObject.CompareTag("Canoe"))
		{
            Debug.Log("on canoe");
			gameController.addCounter (this.GetInstanceID());
			Debug.Log (this.GetInstanceID());
		}
	}
}
