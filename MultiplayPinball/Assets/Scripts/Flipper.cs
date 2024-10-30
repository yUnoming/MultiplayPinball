using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    [SerializeField]
    [Tooltip("はじく力")]
    float flipPower = 1.0f;
    [SerializeField]
    [Tooltip("はじくアクションの際に使用するキー")]
    KeyCode flipKey;

    HingeJoint joint;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // キー入力で、フリッパー操作
        if(Input.GetKeyDown(flipKey))
        {
            rb.velocity = new Vector3(0f, flipPower, 0f);
        }
    }
}
