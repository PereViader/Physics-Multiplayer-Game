using UnityEngine;
using System.Collections;

public class HabilityStop : Hability {

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cooldown = 0.7f;
        habilityName = "Stop";
    }

    public override void ExecuteHability()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
