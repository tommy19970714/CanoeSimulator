using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayJudge : MonoBehaviour
{
    public int rightSinkCounter = 0;
    public int leftSinkCounter = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("right" + rightSinkCounter.ToString());
        Debug.Log("left" + leftSinkCounter.ToString());
    }

    private void OnTriggerStay(Collider collider)
    {
        //Debug.Log("stay");

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Paddle_right"))
        {
            rightSinkCounter++;
        }
        else if (other.gameObject.CompareTag("Paddle_left"))
        {
            leftSinkCounter++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Paddle_right"))
        {
            rightSinkCounter--;
        }
        else if (other.gameObject.CompareTag("Paddle_left"))
        {
            leftSinkCounter--;
        }
    }
}
