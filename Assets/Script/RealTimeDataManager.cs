using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class RealTimeDataManager : MonoBehaviour {

	//UDP ServerからDequeueされたデータをリストで確保
	public Queue<float> time;
	public Queue<float> ac;
	public Queue<Vector3> rad;
	public Vector3 beforerad = new Vector3();
	private Queue<float> v;
	public Queue<float> y;
	private float time_offset = 0;
	private bool firsttime = true;

	void Start () {
		resetdata();
	}

	public void resetdata()
	{
		time = new Queue<float>();
		ac = new Queue<float>();
		rad = new Queue<Vector3>();
		v = new Queue<float>();
		y = new Queue<float>();
		firsttime = true;
		Debug.Log("reseted");
	}

	public void getdata_realtime(string msg)
	{
		float a = (float)0.95;
		try
		{
			string[] cols = msg.Split(',');
			if (firsttime == true)
			{
				time.Enqueue(0);
				time_offset = float.Parse(cols[0]);
				firsttime = false;
			}
			else
			{
				time.Enqueue(float.Parse(cols[0]) - time_offset);
			}
			float radx = -float.Parse(cols[1])/3;
			float radz = -float.Parse(cols[2])/3;
			ac.Enqueue(float.Parse(cols[3]));

			radx = a * beforerad.x + (1 - a) * radx;
			radz = a * beforerad.z + (1 - a) * radz;

			beforerad = new Vector3(radx, 0, radz);
			rad.Enqueue(beforerad);
		}
		catch
		{
			Debug.LogError("UDPで壊れたデータを受信しました");
		}
	}
	public void integral_acc(float beforetime,float nowtime )
	{
		float velocity = (ac.Dequeue() + ac.Peek()) * ((nowtime - beforetime) * (float)0.001) / 2;
		v.Enqueue(velocity);
		if(v.Count > 1)
		{
			float distance = (v.Dequeue() + v.Peek()) * ((nowtime - beforetime) * (float)0.001) / 2;
			y.Enqueue(distance);
		}
	}
}