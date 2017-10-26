using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientNetworkManager : NetworkManager
{
    public GameController gameController;

    // Use this for initialization
    void Start()
    {
        this.networkAddress = PlayerPrefs.GetString("hostIpAddress", "192.168.1.240");
        StartCoroutine("Loop");
    }

    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (!IsClientConnected() && !isNetworkActive)
            {
                Debug.Log("startClient");
                StartClient();
            }
        }
    }

    public override  void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        gameController.ClientStart();
    }
}