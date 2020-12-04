using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animScript : MonoBehaviour
{
    public Animator animator;
    [Range(0f,1f)]
    public float Hype = 0f;

    [SerializeField]
    private string standTransVar = "StandBlend";
    [SerializeField]
    private string armstransVar = "ArmsBlend";

    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(standTransVar, Hype);
        animator.SetFloat(armstransVar, Hype);
    }
}
