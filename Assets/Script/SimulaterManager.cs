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
            //position.x = p.x;
            //position.y = -p.z;
            //position.z = p.y;
            beforePosition = Vector3.zero;
            sinkLevel = 0;
        }

        public void update(Vector3 p)
        {
            beforePosition = position;
            position = p;
            //position.x = p.x;
            //position.y = -p.z;
            //position.z = p.y;
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

        Debug.Log("sqrt 0 " + Mathf.Sqrt(0));
    }
    
    void Update () {
        ParametorUpdate();
        Control();

        // Debug.Log(velocity);

        // カヌーの回転と移動
        canoe.transform.Translate(velocity.x * Time.deltaTime, 0, velocity.z * Time.deltaTime);
        //canoe.transform.Rotate(0, rVelocity * Time.deltaTime, 0);
        //canoe.transform.Translate(velocity.x * Time.deltaTime, 0, 0);
        //canoe.transform.Translate(0, 0, velocity.z * Time.deltaTime);
        //canoe.transform.Translate(0, 0, 0);
    }

    void ParametorUpdate()
    {
        // paddle angle (カヌーからのローカル軸に変換)
        paddleAngle = canoe.transform.rotation.eulerAngles - controller.transform.rotation.eulerAngles;

        //right paddle update
        paddle_right.update(canoe.transform.InverseTransformPoint(rightObject.transform.position)); // FixMe グローバル座標での座標では?
        paddle_right.sinkLevel = stayJudge.rightSinkCounter;

        //left paddle update
        paddle_left.update(canoe.transform.InverseTransformPoint(leftObject.transform.position));
        paddle_left.sinkLevel = stayJudge.leftSinkCounter;

        //Debug.Log("canoe" + transform.TransformPoint(canoe.transform.position).ToString());
        //Debug.Log("paddle right" + paddle_right.position.ToString());
        //Debug.Log("paddle left" + paddle_left.position.ToString());
        //Debug.Log("canoe" + canoe.transform.position);
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
        float constR = 0.1f; // rvを求めるための定数
        float constFliction = 0.05f; //水の抵抗
        //paddleAngle.zは360度法の値が返ってきます.ラジアンに変換しましょう
        float paddleAngleZ_rad = paddleAngle.z * (Mathf.PI / 180.0f);
        //Debug.Log(rightObject.transform.position);
        // x,z方向の速度を求める
        //velocity.z += (constXZ * paddle_right.sinkLevel * pdx_r) * Mathf.Abs(Mathf.Cos(paddleAngleZ_rad)) + (constXZ * paddle_left.sinkLevel * -pdx_l) * Mathf.Abs(Mathf.Sin(paddleAngleZ_rad));// - constFliction * velocity.x; // paddleAngle.zをかける 
        //velocity.x += (constXZ * paddle_right.sinkLevel * pdz_r) * Mathf.Abs(Mathf.Cos(paddleAngleZ_rad)) + (constXZ * paddle_left.sinkLevel * pdz_l) * Mathf.Abs(Mathf.Sin(paddleAngleZ_rad));// - constFliction * velocity.z; // paddleAngle.zをかける

        //velocity.x += (constXZ * paddle_right.sinkLevel * -pdx_r) * Mathf.Abs(Mathf.Cos(paddleAngleZ_rad)) + (constXZ * paddle_left.sinkLevel * -pdx_l) * Mathf.Abs(Mathf.Sin(paddleAngleZ_rad));// - constFliction * velocity.x; // paddleAngle.zをかける 
        //velocity.z += (constXZ * paddle_right.sinkLevel * -pdz_r) * Mathf.Abs(Mathf.Sin(paddleAngleZ_rad)) + (constXZ * paddle_left.sinkLevel * -pdz_l) * Mathf.Abs(Mathf.Cos(paddleAngleZ_rad));// - constFliction * velocity.z; // paddleAngle.zをかける

        velocity.x -= constXZ * ((velocity.x + paddle_xvel_r) * paddle_right.sinkLevel * Mathf.Abs(Mathf.Cos(paddleAngleZ_rad)) + ((velocity.x + paddle_xvel_l) * paddle_left.sinkLevel * Mathf.Abs(Mathf.Sin(paddleAngleZ_rad))));
        velocity.z -= constXZ * ((velocity.z + paddle_zvel_r) * paddle_right.sinkLevel * Mathf.Abs(Mathf.Sin(paddleAngleZ_rad)) + ((velocity.z + paddle_zvel_l) * paddle_left.sinkLevel * Mathf.Abs(Mathf.Cos(paddleAngleZ_rad))));
        //Debug.Log("velocity.x = " + velocity.x );
        //Debug.Log("pdx_r = " + pdx_r);
        Debug.Log(velocity.x + pdx_l);
        //Debug.Log("v_x paddleAngle" + Mathf.Abs(Mathf.Sin(paddleAngleZ_rad)).ToString());
        //Debug.Log("v x" + velocity.x.ToString());
        //Debug.Log("v z" + velocity.z.ToString());

        velocity.z *= 0.995f;
        velocity.x *= 0.995f;

        Debug.Log(paddleAngle);

        //if (velocity.z != 0) {
        //    velocity.z = Mathf.Sqrt(Mathf.Abs(velocity.z)) * (velocity.z / Mathf.Abs(velocity.z));
        //}
        //if (velocity.x != 0) {
        //    velocity.x = Mathf.Sqrt(Mathf.Abs(velocity.x)) * (velocity.x / Mathf.Abs(velocity.x));
        //}
        //Debug.Log("pdx_r" + pdx_r.ToString());
        //Debug.Log("pdz_r" + pdz_r.ToString());
        //Debug.Log( (constXZ * paddle_left.sinkLevel * -pdx_l) * Mathf.Abs(Mathf.Sin(paddleAngleZ_rad)));

        //前z + 後ろz- 右x+ 左x-

        //Debug.Log("v x" + velocity.x.ToString());
        //Debug.Log("v z" + velocity.z.ToString());

        // y軸の角速度を求める
        //rVelocity += constR / distance_l * Mathf.Sqrt(pdx_l * pdx_l + pdz_l * pdz_l);
        //rVelocity -= constR / distance_r * Mathf.Sqrt(pdx_r * pdx_r + pdz_r * pdz_r);
    }
}
