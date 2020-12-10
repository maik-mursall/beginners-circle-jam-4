using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public GameObject lazerOrigin;
    public GameObject lazerTarget;

    private Vector3 originPos = Vector3.zero;
    private Vector3 targetPos = Vector3.zero;

    //Line renderer
    public LineRenderer lazerLine;

    // Start is called before the first frame update
    void Start()
    {
        lazerTarget.GetComponent<LineRenderer>();

        originPos = lazerOrigin.transform.position;
        targetPos = lazerTarget.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        lazerLine.SetPosition(0, originPos);
        lazerLine.SetPosition(1, targetPos);
    }
}
