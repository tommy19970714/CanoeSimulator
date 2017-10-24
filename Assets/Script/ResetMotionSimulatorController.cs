using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetMotionSimulatorController : MonoBehaviour
{
    public Serial serial;
    //public Text messageText;
    //public TextMesh viveMessageText;

    void Start()
    {
        StartCoroutine("InitiationWait");
    }

    IEnumerator InitiationWait()
    {
        //messageText.text = "enterが押されるまでinitiationします...";
        //viveMessageText.text = "enterが押されるまでinitiationします...";
        serial.dumpinitialize();
        yield return new WaitForSeconds(1.0f);
        yield return new WaitWhile(() => Input.GetKeyDown(KeyCode.Return) == false);
        yield return new WaitForSeconds(0.1f);
        serial.dumpstop();
        serial.close();

        SceneManager.LoadScene("CanueRiver");
    }
}