using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulaterManager : MonoBehaviour {

    public GameObject canue;
    public GameObject paddle;

    public StayJudge stayJudge;

    private Paddle paddle_right;
    private Paddle paddle_left;

    private float flowLevel = 0;
    private Vector3 paddleAngle = new Vector3();

    struct Paddle {
        GameObject self;
        Vector3 position;
        Vector3 beforePosition;
        int sinkLevel;

        void update(GameObject paddleObject)
        {
            self = paddleObject;
            next(self.transform.position);
        }

        void next(Vector3 p)
        {
            beforePosition = position;
            position = p;
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        paddleAngle = paddle.transform.rotation.eulerAngles;
	}
}
