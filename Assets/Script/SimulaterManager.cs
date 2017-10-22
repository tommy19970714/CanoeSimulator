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
    private Vector3 position = Vector3.zero;
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
        
        // カヌーの回転と移動
        canoe.transform.Translate(velocity.x * Time.deltaTime, 0, velocity.z * Time.deltaTime);
        canoe.transform.Rotate(0, rVelocity * Time.deltaTime, 0);
    }

    void ParametorUpdate()
    {
        // paddle angle (カヌーからのローカル軸に変換)
        paddleAngle = canoe.transform.rotation.eulerAngles - controller.transform.rotation.eulerAngles;

        //right paddle update
        paddle_right.update(canoe.transform.InverseTransformPoint(rightObject.transform.position)); // ローカル軸に変換
        paddle_right.sinkLevel = stayJudge.rightSinkCounter;

        //left paddle update
        paddle_left.update(canoe.transform.InverseTransformPoint(leftObject.transform.position));
        paddle_left.sinkLevel = stayJudge.leftSinkCounter;
    }

    void Control()
    {
        // パドルの変化量
        float pdx_r = paddle_right.getVariation().x * 10.0f;
        float pdz_r = paddle_right.getVariation().z * 10.0f;

        float pdx_l = paddle_left.getVariation().x * 10.0f;
        float pdz_l = paddle_left.getVariation().z * 10.0f;

        // パドルの速さ
        float paddle_xvel_r = paddle_right.getVariation().x / Time.deltaTime;
        float paddle_zvel_r = paddle_right.getVariation().z / Time.deltaTime;

        float paddle_xvel_l = paddle_left.getVariation().x / Time.deltaTime;
        float paddle_zvel_l = paddle_left.getVariation().z / Time.deltaTime;


        // 左右のパドルの中心とカヌーの重心の距離
        float distance_r = Vector3.Distance(Vector3.zero, paddle_right.position);
        float distance_l = Vector3.Distance(Vector3.zero, paddle_left.position);

        float constXZ = 0.00023f; // xv zvを求めるための定数
        float constY = 0.085f; // rvを求めるための定数
        float constFliction = 0.05f; //水の抵抗
        float paddleAngleZ_rad = paddleAngle.z * (Mathf.PI / 180.0f); //paddleAngle.zは360度法の値
        // x,z方向の速度を求める
        velocity.x -= constXZ * ((velocity.x + paddle_xvel_r) * paddle_right.sinkLevel * Mathf.Abs(Mathf.Cos(paddleAngleZ_rad)) + ((velocity.x + paddle_xvel_l) * paddle_left.sinkLevel * Mathf.Abs(Mathf.Sin(paddleAngleZ_rad))));
        velocity.z -= constXZ * ((velocity.z + paddle_zvel_r) * paddle_right.sinkLevel * Mathf.Abs(Mathf.Sin(paddleAngleZ_rad)) + ((velocity.z + paddle_zvel_l) * paddle_left.sinkLevel * Mathf.Abs(Mathf.Cos(paddleAngleZ_rad))));
        
        velocity.z *= 0.995f;
        velocity.x *= 0.995f;
        
        // y軸の角速度を求める
        float theta_r = Mathf.Atan2(paddle_right.position.x, paddle_right.position.z);
        float theta_l = Mathf.Atan2(paddle_left.position.x, paddle_left.position.z);
        //Debug.Log(theta_r);
        rVelocity -= constY * distance_r * (pdz_r * Mathf.Sin(theta_r) + pdx_r * Mathf.Cos(theta_r)) * paddle_right.sinkLevel;
        rVelocity += constY * distance_l * (pdz_l * Mathf.Sin(theta_l) - pdx_l * Mathf.Cos(theta_l)) * paddle_left.sinkLevel;
        //Debug.Log(rVelocity);
        rVelocity *= 0.90f;
    }
}
