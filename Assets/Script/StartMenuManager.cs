using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour {

	public Dropdown dropdown;
	public Button hostButton;
	public Button clientButton;
	public Text ipAddressLabel;
	public InputField portInputField;

	// Use this for initialization
	void Start () {
		if (dropdown) {
			dropdown.ClearOptions();
			string [] ports = SerialPort.GetPortNames ();
			List<string> portList = new List<string>();
			portList.AddRange (ports);
			dropdown.AddOptions(portList);
			dropdown.value = 0;
		}

		ipAddressLabel.text = "IPAddress: " + UnityEngine.Network.player.ipAddress;
		portInputField.text = PlayerPrefs.GetInt ("realtimeUdpPort", 22227).ToString();
	}
	
	public void OnClickHostButton() {
		SceneManager.LoadScene ("ResetMotionSimulator");
	}

	public void OnClickClientButton() {
		SceneManager.LoadScene ("Client");
	}

	public void ValueChnagedDropDown(Dropdown dropdown) {
		Debug.Log("dropdown.value = " + dropdown.value);    //値を取得（先頭から連番(0～n-1)）
		Debug.Log("text(options) = " + dropdown.options[dropdown.value].text); //リストからテキストを取得
		Debug.Log("text(captionText) = " + dropdown.captionText.text); //Labelからテキストを取得
		PlayerPrefs.SetString("serialCom", dropdown.options[dropdown.value].text);
		PlayerPrefs.Save ();
	}

	public void ValueChangedInputField() {
		PlayerPrefs.SetInt ("replayUdpPort", int.Parse(portInputField.text));
		PlayerPrefs.Save ();
	}
}
