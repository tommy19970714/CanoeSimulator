using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostNetworkManager : NetworkManager {

    // Use this for initialization
    void Start()
    {
        Debug.Log(this.networkAddress);
        StartCoroutine("Loop");
    }

    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            if (!isNetworkActive)
            {
                Debug.Log("startHost");
                this.StartHost();
            }
        }
    }
}
