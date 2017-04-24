using UnityEngine;
using System.Collections;
using System;

public class HabilityJump : Hability {

    [SerializeField]
    private float jumpForce = 30f;
    private Rigidbody rb;

    void Awake()
    {
        cooldown = 1;
        habilityName = "Jump";
        rb = GetComponent<Rigidbody>();
    }

    public override void ExecuteHability()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
