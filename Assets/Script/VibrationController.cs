using System.Collections;
using System.Collections.Generic;
using UnityEngine.VR;
using UnityEngine;
using UnityEngine.Networking;

public class VibrationController : MonoBehaviour
{
    private double headUnderValue = 0.2;
    private string ledServer = "http://192.168.1.103/led";
    private string vibrationServer = "http://192.168.2.50/vibration";
    public SteamVR_TrackedObject leftController;
    public SteamVR_TrackedObject rightController;
    private bool responseSW = false;


    // Use this for initialization
    void Start()
    {
        UnityEngine.XR.XRSettings.showDeviceView = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 着席モードでRキーで位置トラッキングをリセットする
        if (Input.GetKeyDown(KeyCode.R))
        {
            SteamVR.instance.hmd.ResetSeatedZeroPose();
            Debug.Log("puress R");
        }
        if (rightController != null && leftController != null)
        {
            if (rightController.isActiveAndEnabled)
            {
                StartCoroutine(controllerTouchpadTriger(rightController));

            }
            else if (leftController.isActiveAndEnabled)
            {
                StartCoroutine(controllerTouchpadTriger(leftController));
            }
        }
    }

    IEnumerator controllerTouchpadTriger(SteamVR_TrackedObject controller)
    {
        var device = SteamVR_Controller.Input((int)controller.index);
        string requestURL = "";
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Debug.Log("タッチパッドをクリックした");
            Vector2 position = device.GetAxis();
            double strong = (position.y + 1) / 2;
            requestURL = vibrationServer + "/0/on?" + "strength=" + strong.ToString("0.000") + "&interval=500"+ "&duty=0.5";
        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Debug.Log("タッチパッドをクリックして離した");
            requestURL = vibrationServer + "/0/off";
        }
        if (requestURL.Length != 0)
        {
            UnityWebRequest request = UnityWebRequest.Get(requestURL);
            yield return new WaitWhile(() => (responseSW == true));
            responseSW = true;
            Debug.Log(requestURL);
            //yield return request.SendWebRequest();
            responseSW = false;
            
        }
    }
}
