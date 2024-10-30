using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�͂�����")]
    float flipPower = 1.0f;
    [SerializeField]
    [Tooltip("�͂����A�N�V�����̍ۂɎg�p����L�[")]
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
        // �L�[���͂ŁA�t���b�p�[����
        if(Input.GetKeyDown(flipKey))
        {
            rb.velocity = new Vector3(0f, flipPower, 0f);
        }
    }
}
