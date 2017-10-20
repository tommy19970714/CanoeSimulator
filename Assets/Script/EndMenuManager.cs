using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenuManager: MonoBehaviour {

	public GameObject scoreText;

	// Use this for initialization
	void Start () {
		scoreText.GetComponent<TextMesh>().text = PlayerPrefs.GetInt ("Score", 0).ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space)) { // もう一度
			SceneManager.LoadScene ("CanueRiver");
		}
		if (Input.GetKey (KeyCode.Space)) { // 初期画面に戻る 
			SceneManager.LoadScene ("StartScene");
		}
	}
}