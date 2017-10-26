using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameController : MonoBehaviour
{

    public int polecounter;
    public GameObject countLabel;
    public GameObject timerLabel;
    private List<int> idList = new List<int>();
    private int LimitTime = 60;
    public TextAnimation3D textAnimation;
    public TextMesh finishText;
    public Serial serial;

    public float timer = 0;

    // Use this for initialization
    void Start()
    {
        // 衝突を無視するように設定
        int layer1 = LayerMask.NameToLayer("Canoe");
        int layer2 = LayerMask.NameToLayer("Paddle");
        Physics.IgnoreLayerCollision(layer1, layer2);

        finishText.gameObject.SetActive(false);
        textAnimation.StartAnimation("Start");
        countLabel.GetComponent<TextMesh>().text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        int remainTime = LimitTime - (int)timer;
        if (remainTime == 30)
        {
            textAnimation.StartAnimation("30秒");
        }
        else if (remainTime == 10)
        {
            textAnimation.StartAnimation("10秒");
        }
        else if (remainTime == 3)
        {
            textAnimation.StartAnimation("3秒");
        }
        else if (remainTime == 0)
        {
            textAnimation.StartAnimation("Finish!");
            timerLabel.SetActive(false);
            countLabel.SetActive(false);
            finishText.gameObject.SetActive(true);
            finishText.text = "Your Score : " + polecounter.ToString();
            if (serial != null)
            {
                serial.dumpstop();
                serial.dumpinitialize();
            }
            Pauser.Pause();
            SteamVR_Fade.Start(Color.clear, 0f);
            SteamVR_Fade.Start(new Color(0,0,0,0.8f), 2.5f);
        }
        timerLabel.GetComponent<TextMesh>().text = "残り時間:" + remainTime.ToString() + "秒";

        // 初期画面に戻る
        if (Input.GetKey(KeyCode.Escape) || (Input.GetKey(KeyCode.Space) && remainTime < 0))
        {
            serial.dumpstop();
            serial.close();
            SceneManager.LoadScene("StartScene");
        }
    }

    public void addCounter(int id)
    {
        Debug.Log("addcounter");
        if (idList.FindAll(x => x == id).Count == 0)
        {
            polecounter += 1;
            idList.Add(id);
        }
        countLabel.GetComponent<TextMesh>().text = "Score: " + polecounter.ToString();

        textAnimation.StartAnimation("Nice!");
    }
}
