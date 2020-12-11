using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.HypeMeter;

public class crowdAnimation : MonoBehaviour
{
    public List<GameObject> crowdArms;

    public Vector2 sinOffsetRange = new Vector2(0, 1);
    public Vector2 zScaleRange = new Vector2(0.8f, 1.2f);
    public float sinSpeed = 1f;

    public struct CrowdArmStruct
    {
        public CrowdArmStruct(Transform ArmTransform, float sinOffset)
        {
            this.ArmTransform = ArmTransform;
            this.sinOffset = sinOffset;
        }
        public Transform ArmTransform;
        public float sinOffset;
    }

    private List<CrowdArmStruct> crowdArmStructs = new List<CrowdArmStruct>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject g in crowdArms)
        {
            float sinOffset = Random.Range(sinOffsetRange[0], sinOffsetRange[1]);
            crowdArmStructs.Add(new CrowdArmStruct(g.transform, sinOffset));
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(CrowdArmStruct cas in crowdArmStructs)
        {
            float sinValue = Mathf.Sin((Time.time + cas.sinOffset) * sinSpeed * HypeMeter.Instance.HypePercent );
            sinValue = sinValue / 2 + 1;
            sinValue = sinValue * (zScaleRange[1] - zScaleRange[0]) + zScaleRange[0];
            cas.ArmTransform.localScale = new Vector3(1, 1, sinValue);
        }
    }
}
