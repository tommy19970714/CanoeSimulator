using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public int polecounter;
	public GameObject countLabel;
	public GameObject timerLabel;
	private List<int> idList = new List<int>();
	public float timer = 0;
	public int LimitTime = 30;

	// Use this for initialization
	void Start () {
        countLabel.GetComponent<TextMesh>().text = "Score: 0";
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		int remainTime = LimitTime - (int)timer;
		if(remainTime < 0) {
			PlayerPrefs.SetInt ("Score", polecounter);
			SceneManager.LoadScene ("EndScene");
		}
        timerLabel.GetComponent<TextMesh>().text = "残り時間:" + remainTime.ToString () + "秒";
	}

	public void addCounter(int id) {
        Debug.Log("addcounter");
		if (idList.FindAll (x => x == id).Count == 0) {
			polecounter += 1;
			idList.Add (id);
            Debug.Log("add counter count = 0");
		}
        countLabel.GetComponent<TextMesh>().text = "Score: " + polecounter.ToString ();
	}
}
