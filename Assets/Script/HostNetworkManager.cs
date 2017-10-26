using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class HostNetworkManager : NetworkManager {

    public GameController gameController;

    // Use this for initialization
    void Awake()
    {
        Debug.Log(this.networkAddress);
        StartCoroutine("Loop");
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (!isNetworkActive)
            {
                Debug.Log("startHost");
                this.StartHost();
                
            }
        }
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        gameController.HostStart();
    }

    void OnSceneUnloaded(Scene scene)
    {
        Debug.Log(scene.name + " scene unloaded");
        this.StopHost();
        Destroy(this.gameObject);
    }
}
