using UnityEngine;
using System.Collections;
using System;

public class TeleportPipeExit : MonoBehaviour {

    Vector3 exitStart;
    Vector3 exitDirection;

    [SerializeField]
    float exitForce;

    void Awake()
    {
        exitStart = transform.Find("exit").position;
        exitDirection = (transform.Find("exitNext").position - exitStart).normalized;
    }

    public void TeleportObject(GameObject other)
    {
        other.transform.position = exitStart;
        Rigidbody otherRb = other.GetComponent<Rigidbody>();
        otherRb.velocity = exitDirection * exitForce;
    }
}
