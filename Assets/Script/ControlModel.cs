using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlModel : MonoBehaviour {

	public Serial serial;

	//temp変数
	private float before_height = 0;
	private Vector3 tempmove = new Vector3(0,0,0);

	//定数
	private float middle = 37.5f;
	private float max = 92.5f;
	private float min = 0.0f;
    public float limitDegree = 15;
    private float a = (float)0.8;//ハイパスの定数

    //モーションテーブルの制御値を返すmethod
    public Vector3 allControl(Vector3 rotation, bool send)
	{
		Vector3 move = controlAngle3 (limitRotation(rotation));

		/*
        move.x -= 800 * before_height;
		move.y -= 800 * before_height;
		move.z -= 800 * before_height;

		move.x += middle;
		move.y += middle;
		move.z += middle;
        */
		move = comparison (move);

		//1mmしか変化してないときは動かさない（細かな上下動抑止）
		if (Mathf.Abs(tempmove.x - move.x) < 1)
		{
			move.x = tempmove.x;
		}
		if (Mathf.Abs(tempmove.y - move.y) < 1)
		{
			move.y = tempmove.y;
		}
		if (Mathf.Abs(tempmove.z - move.z) < 1)
		{
			move.z = tempmove.z;
		}
		tempmove.x = move.x;
		tempmove.y = move.y;
		tempmove.z = move.z;

		Vector3 senddata = new Vector3 (move.x * 400 / 5, move.y * 400 / 5, move.z * 400 / 5);
		Debug.Log(senddata);
		if(send) serial.dumphex (senddata);

		return move;
	}

	public void control_height(float height,float before)
	{ 
		before_height += a*height + (1.0f-a)*before;
		if (before_height < 0) {
			before_height += -before_height * before_height / 1; 
		} else {
			before_height -= before_height * before_height / 1;
		}

		if (before_height > 20) before_height = 20;
		if (before_height < -20) before_height = -20;
	}

	private Vector3 comparison(Vector3 move)
	{
		if (move.x > max)
		{
			move.y -= move.x - max;
			move.z -= move.x - max;
			move.x = max;
		}
		if (move.y > max)
		{
			move.x -= move.y - max;
			move.z -= move.y - max;
			move.y = max;
		}
		if (move.z > max)
		{
			move.x -= move.z - max;
			move.y -= move.z - max;
			move.z = max;
		}


		if (move.x < min)
		{
			move.y += min - move.x;
			move.z += min - move.x;
			move.x = min;
		}
		if (move.y < min)
		{
			move.x += min - move.y;
			move.z += min - move.y;
			move.y = 0;
		}
		if (move.z < min)
		{
			move.x += min - move.z;
			move.y += min - move.z;
			move.z = min;
		}



		if (move.x > max) {
			move.x = max;
		} else if (move.x < min) {
			move.x = min;
		}

		if (move.y > max) {
			move.y = max;
		} else if (move.y < min) {
			move.y = min;
		}

		if (move.z > max) {
			move.z = max;
		} else if (move.z < min) {
			move.z = min;
		}

		if (move.x > max) move.x = max;
		else if (move.x < min) move.x = min;
		if (move.y > max) move.y = max;
		else if (move.y < min) move.y = min;
		if (move.z > max) move.z = max;
		else if (move.z < min) move.z = min;

		return move;
	}

	//センサデータを用いてモーションテーブルを傾けるための制御値を返す
	private Vector3 controlAngle3(Vector3 degree){

		const float arm_length = 355;
		const float base_radius = 150;
		const float end_radisu = 245;


		//!!!! S H I R A N A I !!!!
		float theta = - degree.z / (float)Mathf.PI * 180f;
		float psi = degree.x / (float)Mathf.PI * 180f;
		//!!!!!!!!!!!!!!!!!!!!!!!!!

		//Debug.Log("theta = "+ theta + "[deg]");
		//Debug.Log("psi = " + psi + "[deg]");

		Vector3 a = new Vector3(0.0f, 1.0f, 0.0f);
		Vector3 b1 = new Vector3(0f, 0f, -base_radius);
		Vector3 b2 = new Vector3(Mathf.Sqrt(3) / 2 * base_radius, 0, 1.0f / 2.0f * base_radius);
		Vector3 b3 = new Vector3(-Mathf.Sqrt(3) / 2 * base_radius, 0, 1.0f / 2.0f * base_radius);

		Vector3 s1 = new Vector3(0, 0, -end_radisu);
		Vector3 s2 = new Vector3(Mathf.Sqrt(3) / 2 * end_radisu, 0, 1.0f / 2.0f * end_radisu);
		Vector3 s3 = new Vector3(-Mathf.Sqrt(3) / 2 * end_radisu, 0, 1.0f / 2.0f * end_radisu);

		Vector3 p = new Vector3(0, 342.06f + 50f, 0);

		Quaternion rotation = Quaternion.Euler(psi, 0, theta);

		Vector3 c1 = p + rotation * s1 - b1;
		Vector3 c2 = p + rotation * s2 - b2;
		Vector3 c3 = p + rotation * s3 - b3;

		//Debug.Log("c1 = " + c1);
		//Debug.Log("c2 = " + c2);
		//Debug.Log("c3 = " + c3);

		float u1 = Vector3.Dot(c1, a) - Mathf.Sqrt(Mathf.Pow(Vector3.Dot(c1, a), 2) - Vector3.Dot(c1, c1) + Mathf.Pow(arm_length, 2));
		float u2 = Vector3.Dot(c2, a) - Mathf.Sqrt(Mathf.Pow(Vector3.Dot(c2, a), 2) - Vector3.Dot(c2, c2) + Mathf.Pow(arm_length, 2));
		float u3 = Vector3.Dot(c3, a) - Mathf.Sqrt(Mathf.Pow(Vector3.Dot(c3, a), 2) - Vector3.Dot(c3, c3) + Mathf.Pow(arm_length, 2));

		//Debug.Log("theta = " + theta + ", psi = " + psi + ", return values = " + u1 + "," + u2 + "," + u3);
		return new Vector3(u1, u2, u3);
	}

	//モーションテーブルの機械的な限界値を制限するmethod
	private Vector3 limitRotation(Vector3 rotation)
	{
		float limitRad = Mathf.PI * limitDegree / 180.0f;
		float returnx = rotation.x;
		float returnz = rotation.z;
		if (rotation.x > limitRad) {
			returnx = limitRad;
		} else if (rotation.x < -limitRad) {
			returnx = -limitRad;
		}
		if (rotation.z > limitRad) {
			returnz = limitRad;
		} else if (rotation.z < -limitRad) {
			returnz = -limitRad;
		}
		return new Vector3 (returnx, rotation.y, returnz);

	}
}
