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
    public InputField hostIpAddressField;

    // Use this for initialization
    void Start () {
		if (dropdown) {
			dropdown.ClearOptions();
			string [] ports = SerialPort.GetPortNames ();
			List<string> portList = new List<string>();
			portList.AddRange (ports);
			dropdown.AddOptions(portList);
            for(int i=0;i<portList.Count;i++) {
                Debug.Log(portList[i]);
                Debug.Log(PlayerPrefs.GetString("serialCom"));
                if(portList[i] == PlayerPrefs.GetString("serialCom")) {
                    dropdown.value = i;
                    Debug.Log("kokokoko");
                    Debug.Log("kokokoko");
                }
            }

            dropdown.onValueChanged.AddListener(delegate
            {
                selectvalue(dropdown);
            });
        }

		ipAddressLabel.text = "IPAddress: " + UnityEngine.Network.player.ipAddress;
		portInputField.text = PlayerPrefs.GetInt ("realtimeUdpPort", 22227).ToString();
        hostIpAddressField.text = PlayerPrefs.GetString("hostIpAddress", "192.168.1.240");
	}

    private void selectvalue(Dropdown gdropdown)
    {
        PlayerPrefs.SetString("serialCom", gdropdown.options[gdropdown.value].text);
        PlayerPrefs.Save();
        Debug.Log(dropdown.options[gdropdown.value].text);
    }

    public void OnClickHostButton() {
		SceneManager.LoadScene ("ResetMotionSimulator");
	}

	public void OnClickClientButton() {
		SceneManager.LoadScene ("Client");
	}
    
	public void ValueChangedPortField() {
		PlayerPrefs.SetInt ("replayUdpPort", int.Parse(portInputField.text));
		PlayerPrefs.Save ();
	}

    public void ValueChangedHostIpAddressField()
    {
        PlayerPrefs.SetString("hostIpAddress", hostIpAddressField.text);
        PlayerPrefs.Save();
    }
}
