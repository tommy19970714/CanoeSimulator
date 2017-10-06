using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using NWH;

public class ShipController : MonoBehaviour
{
    public Vector3 forceApplicationPoint = new Vector3();

    [SerializeField]
    public List<Transform> rudders = new List<Transform>();

    // Thrust
    private Vector3 thrustDirection = new Vector3();
    // 1hp ~= 100N thrust
    public float maxThrust = 30000;
    private float thrustVelocity;
    private float thrustPercent;
    public float thrustChangeSpeed = 0.5f;
    private float thrustAmount;
    private Vector3 thrustVect;
    public float thrustSteerPercent = 0.3f;

    // Rudder
    private float rudderVelocity;
    private float rudderPercent;
    public float rudderSpeed = 0.4f;
    public float maxRudderAngle = 45f;
    [HideInInspector]
    public float rudderAngle;

    public Rigidbody targetRigidbody;
    private float xAxis;
    private float yAxis;

    public FloatingObject relatedHull = null;
    private InputHandler input = new InputHandler();
    public bool mouseInput = false;

    private void Start()
    {
        targetRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(relatedHull.water != null)
        {
            if (mouseInput)
                input.inputMode = InputHandler.InputMode.Mouse;
            else
                input.inputMode = InputHandler.InputMode.Keyboard;

            input.Update();

            thrustDirection = transform.forward;

            xAxis = input.xAxis;
            yAxis = input.yAxis;

            rudderPercent = Mathf.SmoothDamp(rudderPercent, xAxis, ref rudderVelocity, rudderSpeed);
            thrustPercent = Mathf.Clamp(Mathf.SmoothDamp(thrustPercent, yAxis, ref thrustVelocity, thrustChangeSpeed), -0.3f, 1f);

            rudderAngle = rudderPercent * maxRudderAngle;
            thrustAmount = thrustPercent * maxThrust;

            thrustVect = Quaternion.AngleAxis(-rudderAngle * thrustSteerPercent, Vector3.up) * thrustDirection * thrustAmount;

            if (relatedHull == null)
            {
                targetRigidbody.AddForceAtPosition(thrustVect, transform.TransformPoint(forceApplicationPoint));
            }
            else
            {
                if (relatedHull.GetComponent<FloatingObject>().PointInWater(transform.TransformPoint(forceApplicationPoint)))
                {
                    targetRigidbody.AddForceAtPosition(thrustVect, transform.TransformPoint(forceApplicationPoint));
                }
            }

            foreach (Transform rudder in rudders)
            {
                rudder.localRotation = Quaternion.Euler(0, -rudderAngle, 0);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(forceApplicationPoint), 0.3f);
        Gizmos.DrawRay(new Ray(transform.TransformPoint(forceApplicationPoint), thrustVect));
    }
}