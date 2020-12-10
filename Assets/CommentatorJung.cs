using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentatorJung : MonoBehaviour
{
    [Header("Inputs")]
    [Range(0, 10)]
    public float hypeMeter = 0;
    public Vector2 hypeMeterRange = new Vector2(0, 10);
    public Animator animator;

    [Header("Outputs")]
    [SerializeField]
    private float hypeStep = 0;
    [SerializeField]
    private int hypeUpDown = 0;

    [Header("Settings")]
    private int hypeStepCount = 3;
    [Range(0,6), SerializeField]
    private float hypeUpDownDeadzone = 2;
    [Range(0, 6), SerializeField]
    private float sustainedHypeBleedOff = 0.5f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    [Header("working Vars")]
    public bool pressToHype = false;
    [SerializeField]
    private float hypeDelta = 0;
    [SerializeField]
    private float lastFrameHype = 0;
    [SerializeField]
    private float hypeCombo = 0;
    public GameObject Commentator;


    void HypeCalculations()
    {
        hypeDelta = hypeMeter - lastFrameHype;
        hypeCombo += hypeDelta;

        if (hypeCombo > 0 + hypeUpDownDeadzone / 2)
        {
            hypeUpDown = 1;
        }
        else if (hypeCombo < 0 + hypeUpDownDeadzone / 2)
        {
            hypeUpDown = -1;
        }
        else
        {
            hypeUpDown = 0;
        }
        hypeCombo -= sustainedHypeBleedOff * Time.deltaTime;

        hypeCombo = Mathf.Clamp(hypeCombo, 0, hypeMeterRange[1]);

        float hRange = hypeMeterRange[1] - hypeMeterRange[0];


        Debug.Log(hypeMeter +", " + hRange + ", " + hypeStepCount);
        hypeStep = Mathf.Clamp( hypeMeter /(hRange / hypeStepCount), 0, hypeStepCount +1);

        lastFrameHype = hypeMeter;
    }

    void PressToHype()
    {
        if (pressToHype)
        {
            pressToHype = false;
            hypeMeter += 1;
        }
    }
    void Update()
    {
        // reset position
        Commentator.transform.localPosition = Vector3.zero;

        // debug hypefunction::
        hypeMeter -= (float)(0.3 * Time.deltaTime);
        hypeMeter = Mathf.Clamp(hypeMeter, hypeMeterRange[0], hypeMeterRange[1]);

        HypeCalculations();

        PressToHype();

        animator.SetFloat("hypeStep", hypeStep);
        animator.SetInteger("hypeUpDown", hypeUpDown);



    }
}
