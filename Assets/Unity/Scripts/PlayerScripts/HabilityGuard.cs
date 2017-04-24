using UnityEngine;
using System.Collections;
using System;

public class HabilityGuard : Hability {

    [SerializeField]
    private float guardDuration = 1.4f;

    private Rigidbody rb;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        rb = gameObject.GetComponent<Rigidbody>();
        habilityName = "Guard";
        cooldown = 5f;
    }

    public override void ExecuteHability()
    {
        StartCoroutine(HabilityCoroutine());
    }

    IEnumerator HabilityCoroutine()
    {
        meshRenderer.material.color = Color.black;
        rb.isKinematic = true;
        yield return new WaitForSeconds(guardDuration);
        meshRenderer.material.color = Color.white;
        rb.isKinematic = false;
    }
}
