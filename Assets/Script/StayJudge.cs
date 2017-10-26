using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StayJudge : MonoBehaviour
{
    public int rightSinkCounter = 0;

    public int leftSinkCounter = 0;

    public GameObject wave;
    public GameObject paddle;
    public Vector3 beforePaddlePos;

    public float beforeWaveTime;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("WaitInit");
    }

    IEnumerator WaitInit()
    {
        while (true)
        {
            if (paddle == null)
            {
                GameObject[] findObjects = GameObject.FindGameObjectsWithTag("networkPaddle");
                foreach (GameObject obj in findObjects)
                {
                    NetworkIdentity identity = obj.GetComponent<NetworkIdentity>();
                    if (identity.isLocalPlayer == true)
                    {
                        this.paddle = obj;
                    }
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(paddle != null) beforePaddlePos = paddle.transform.position;
    }

    private void OnTriggerStay(Collider collider)
    {
        //Debug.Log("stay");

    }

    void GenerateWave(Transform transform)
    {
        if (Time.time - beforeWaveTime > 0.7f && paddle != null)
        {
            beforeWaveTime = Time.time;
            float velocity = ((paddle.transform.position - beforePaddlePos) / Time.deltaTime).magnitude;
            GameObject newWave = Instantiate(wave, transform.position, Quaternion.identity);
            newWave.GetComponent<WaveGenerator>().Play(velocity);
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Paddle_right"))
        {
            NetworkIdentity identity = other.transform.parent.GetComponentInParent<NetworkIdentity>();
            if (identity.isLocalPlayer == true && identity.isActiveAndEnabled)
            {
                rightSinkCounter++;
                if (rightSinkCounter == 126) GenerateWave(other.transform);
            }
        }
        else if (other.gameObject.CompareTag("Paddle_left"))
        {
            NetworkIdentity identity = other.transform.parent.GetComponentInParent<NetworkIdentity>();
            if (identity.isLocalPlayer == true && identity.isActiveAndEnabled)
            {
                leftSinkCounter++;
                if (leftSinkCounter == 126) GenerateWave(other.transform);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Paddle_right"))
        {
            NetworkIdentity identity = other.transform.parent.GetComponentInParent<NetworkIdentity>();
            if (identity.isLocalPlayer == true && identity.isActiveAndEnabled)
            {
                rightSinkCounter--;
                if (rightSinkCounter == 0) GenerateWave(other.transform);
            }
        }
        else if (other.gameObject.CompareTag("Paddle_left"))
        {
            NetworkIdentity identity = other.transform.parent.GetComponentInParent<NetworkIdentity>();
            if (identity.isLocalPlayer == true && identity.isActiveAndEnabled)
            {
                leftSinkCounter--;
                if (leftSinkCounter == 0) GenerateWave(other.transform);
            }
        }
    }
}
