using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public int polecounter;
	public Text countLabel;
	public Text timerLabel;
	private List<int> idList = new List<int>();
	public float timer = 0;
	public int LimitTime = 30;

	// Use this for initialization
	void Start () {
		countLabel.text = "0";
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		int remainTime = LimitTime - (int)timer;
		if(remainTime < 0) {
			Application.LoadLevel("EndSene");
		}
		timerLabel.text = remainTime.ToString ();
	}

	public void addCounter(int id) {
		if (idList.FindAll (x => x == id).Count == 0) {
			polecounter += 1;
			idList.Add (id);
		}
		countLabel.text = polecounter.ToString ();
	}
}
