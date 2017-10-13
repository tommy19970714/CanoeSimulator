using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulaterManager : MonoBehaviour {

    public GameObject canoe;
    public GameObject paddle;
    public GameObject controller;

    public StayJudge stayJudge;

    private GameObject rightObject;
    private GameObject leftObject;
    private Paddle paddle_right;
    private Paddle paddle_left;

    private float flowLevel = 0; // 水の流れの定数
    private Vector3 paddleAngle = Vector3.zero; //[rad]

    private Vector3 velocity = Vector3.zero;
    private float rVelocity = 0;

    class Paddle {
        public Vector3 position;
        public Vector3 beforePosition;
        public int sinkLevel; // 0-126の値をとる(0のときは沈んでいない)
         
        public Paddle(Vector3 p)
        {
            position = p;
            beforePosition = Vector3.zero;
            sinkLevel = 0;
        }

        public void update(Vector3 p)
        {
            beforePosition = position;
            position = p;
        } 

        public Vector3 getVariation() // 変化量を返す
        {
            return position - beforePosition;
        }
    }

    void Start () {
        // init
        rightObject = paddle.transform.Find("Right").gameObject;
        leftObject = paddle.transform.Find("Left").gameObject;
        paddle_right = new Paddle(rightObject.transform.position);
        paddle_left = new Paddle(leftObject.transform.position);
    }
    
    void Update () {
        ParametorUpdate();
        Control();

        Debug.Log(velocity);

        // カヌーの回転と移動
        canoe.transform.Translate(velocity.x * Time.deltaTime, 0, velocity.z * Time.deltaTime);
        canoe.transform.Rotate(0, rVelocity * Time.deltaTime, 0);
    }

    void ParametorUpdate()
    {
        // paddle angle (カヌーからのローカル軸に変換)
        paddleAngle = canoe.transform.rotation.eulerAngles - controller.transform.rotation.eulerAngles;

        //right paddle update
        paddle_right.update(rightObject.transform.position);
        paddle_right.sinkLevel = stayJudge.rightSinkCounter;

        //left paddle update
        paddle_left.update(leftObject.transform.position);
        paddle_left.sinkLevel = stayJudge.leftSinkCounter;
    }

    void Control()
    {
        // パドルの変化量
        float pdx_r = paddle_right.getVariation().x;
        float pdz_r = paddle_right.getVariation().z;

        float pdx_l = paddle_left.getVariation().x;
        float pdz_l = paddle_left.getVariation().z;

        // 左右のパドルの中心とカヌーの重心の距離
        float distance_r = Vector3.Distance(canoe.transform.position, paddle_right.position);
        float distance_l = Vector3.Distance(canoe.transform.position, paddle_left.position);

        float constXZ = 0.05f; // xv zvを求めるための定数
        float constR = 0f; // rvを求めるための定数

        Debug.Log(paddle_right.sinkLevel);

        // x,z方向の速度を求める
        velocity.x += (constXZ * paddle_right.sinkLevel * pdx_r) * Mathf.Cos(paddleAngle.z) + (constXZ * paddle_left.sinkLevel * pdx_l) * Mathf.Sin(paddleAngle.z); // paddleAngle.zをかける 
        velocity.z += (constXZ * paddle_right.sinkLevel * pdz_r) * Mathf.Sin(paddleAngle.z) + (constXZ * paddle_left.sinkLevel * pdz_l) * Mathf.Cos(paddleAngle.z); // paddleAngle.zをかける

        // y軸の角速度を求める
        rVelocity += constR / distance_l * Mathf.Sqrt(pdx_l * pdx_l + pdz_l * pdz_l);
        rVelocity -= constR / distance_r * Mathf.Sqrt(pdx_r * pdx_r + pdz_r * pdz_r);
    }
}
