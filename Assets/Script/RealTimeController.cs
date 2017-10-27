using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Threading;

public class RealTimeController : ControlModel {

	// 外部クラス
	public UDPServer udpserver;
	public RealTimeDataManager realtimedata;
	public GameObject canoe;
    public SimulaterManager simulaterManager;


    //定数
    public float realtimeOffset = 0;

	//temp変数
	private float height;
	private float beforeheight = 0;

	//switch 変数
	public bool pause = false;
	public bool started = false;

	//thread
	public Thread control;

	void Start() {
		StartCoroutine("waitThread");
	}

    void Update()
    {
        
    }

    IEnumerator waitThread()
	{
		yield return new WaitWhile(() => (started == false));

		yield return new WaitForSeconds (realtimeOffset);

		Debug.Log ("thread start");
        startMotion();

    }

	public void startMotion()
	{
		udpserver.threadrestart();
		serial.dumpstop();
		serial.dumpoparation();
		pause = false;
		started = true;
		control = new Thread(new ThreadStart(threadcontrol));
		control.Start();
		Debug.Log("start");
	}

	public void stopMotion()
	{
		pause = true;
		started = false;
		udpserver.threadstop();
		stopthread();
		serial.dumpstop();
		Debug.Log("stop");
	}

    public void initMotion()
    {
        stopMotion();
        serial.dumpinitialize();
    }

    public void endMotion()
    {
        stopMotion();
        serial.close();
    }

    //UDPで受信したデータをもとに時間同期をして仮想オブジェクトを動かす
    //UDPの受信側のほうが早いためキューの中身がなくなることはないはず
    void OnApplicationQuit()
	{
		started = false;
		stopthread();
	}

	void stopthread()
	{
		if (control != null)
		{
			if (control.IsAlive)
			{
				control.Abort();
			}
		}
	}

	public void threadcontrol()
	{
		{
			while (started == true && pause == false)
			{
				if (realtimedata.time.Count > 3)
				{
					lock (udpserver.lockobject)
					{
						//制御動作をここに書くDequeueは必要
						//マイコンへの制御値をここ書き込む
						Vector3 rotationrad = realtimedata.rad.Dequeue();

						if (realtimedata.time.Count > 1)
						{
							realtimedata.integral_acc(realtimedata.time.Dequeue(), realtimedata.time.Peek());
						}
						else
						{
							realtimedata.time.Dequeue();
						}

						if (realtimedata.y.Count > 1)
						{
							beforeheight = realtimedata.y.Dequeue();
							height = realtimedata.y.Peek();
							control_height(height, beforeheight);
                            allControl(rotationrad * 2.0f,true);
                            simulaterManager.rotationrad = rotationrad;
						}
					}
				}
				else
				{
					//Debug.Log("empty");
				}
			}
			Debug.Log("thread exit");
		}
	}
}