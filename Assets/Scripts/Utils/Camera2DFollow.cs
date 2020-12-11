using System;
using UnityEngine;

public class Camera2DFollow : MonoBehaviour
{
    public Transform target;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;

    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    public Vector3 m_LookAheadPos;

    private float startDistanceY;
    private float startDistanceZ;
    public  float zoomFactor = 0;
    public Vector3 zoomedDistance= new Vector3();

    public AnimationCurve zoomLookAheadCurve;

    [Header("Setup Values")]
    public float zoomFactorLowerWall = 0.5f;
    public float zoomFactorHighWall = -2.4f;
    public float lookAheadPosLowWall = 1f;
    public float lookAheadPosHighWall = 1000f;
    public float lookAheadZmultiplier = 1.5f;

    private void Start()
    {
        m_LastTargetPosition = target.position;
        m_OffsetZ = (transform.position - target.position).z;
        transform.parent = null;
        startDistanceY = (transform.position - target.position).y;
        startDistanceZ = target.position.z - transform.position.z;
    }


    private void Update()
    {
        // only update lookahead pos if accelerating or changed direction
        float xMoveDelta = (target.position - m_LastTargetPosition).x;
        float zMoveDelta = (target.position - m_LastTargetPosition).z;
        //Debug.Log(xMoveDelta + " :: " + Mathf.Sign(xMoveDelta));

        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold || Mathf.Abs(zMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget)
        {
            float zoomFactorNormalised = (zoomFactor - zoomFactorLowerWall) / (zoomFactorHighWall - zoomFactorLowerWall);

            float m_LookAheadPosX = lookAheadFactor * Mathf.Sign(xMoveDelta);
            float m_LookAheadPosZ = lookAheadFactor * lookAheadZmultiplier * Mathf.Sign(zMoveDelta);
            //Debug.Log("x :: " + xMoveDelta + "    y :: " + zMoveDelta);
            Vector3 m_lookAheadPosi = new Vector3(m_LookAheadPosX, 0, m_LookAheadPosZ);

            // Debug.Log("zoomLookAheadCurve.Evaluate(zoomFactorNormalised) :: " + zoomLookAheadCurve.Evaluate(zoomFactorNormalised));

            m_lookAheadPosi *= zoomLookAheadCurve.Evaluate(zoomFactorNormalised);

            //Debug.Log("zoomFactorNormalised :: " + zoomFactorNormalised);
            if (xMoveDelta == 0)
                m_lookAheadPosi.x = 0;
            if (zMoveDelta == 0)
                m_lookAheadPosi.z = 0;



            

            m_LookAheadPos = m_lookAheadPosi;
        }
        else
        {
            m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
        }

        //Zoomfactor
        zoomFactor+= Input.GetAxis("Mouse ScrollWheel");
        zoomFactor = Mathf.Clamp(zoomFactor, zoomFactorHighWall, zoomFactorLowerWall);
        zoomedDistance = new Vector3(0, startDistanceY, startDistanceZ) * (1- zoomFactor);


        //setting Position
        Vector3 aheadTargetPos = new Vector3(target.position.x, zoomedDistance.y + target.position.y, target.position.z - zoomedDistance.z) + m_LookAheadPos;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

        transform.position = newPos;

        m_LastTargetPosition = target.position;
    }
}
