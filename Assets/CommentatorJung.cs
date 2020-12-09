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
    private int hypeStep = 0;
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
    private float sustainedHypeIncrease = 0;


    void HypeCalculations()
    {
        hypeDelta = hypeMeter - lastFrameHype;
        sustainedHypeIncrease += hypeDelta;

        if (sustainedHypeIncrease > 0 + hypeUpDownDeadzone / 2)
        {
            hypeUpDown = 1;
        }
        else if (sustainedHypeIncrease < 0 + hypeUpDownDeadzone / 2)
        {
            hypeUpDown = -1;
        }
        else
        {
            hypeUpDown = 0;
        }
        sustainedHypeIncrease -= sustainedHypeBleedOff * Time.deltaTime;

        sustainedHypeIncrease = Mathf.Clamp(sustainedHypeIncrease, 0, hypeMeterRange[1]);

        float hRange = hypeMeterRange[1] - hypeMeterRange[0];



        hypeStep = Mathf.Clamp( Mathf.FloorToInt((hRange / hypeStepCount)), 0, hypeStepCount);

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
        // debug hypefunction::
        hypeMeter -= (float)(0.3 * Time.deltaTime);
        hypeMeter = Mathf.Clamp(hypeMeter, hypeMeterRange[0], hypeMeterRange[1]);

        HypeCalculations();

        PressToHype();

        animator.SetInteger("hypeStep", hypeStep);
        animator.SetInteger("hypeUpDown", hypeUpDown);



    }
}
