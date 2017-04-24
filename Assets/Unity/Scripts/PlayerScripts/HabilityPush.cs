using UnityEngine;
using System.Collections.Generic;
using System;

public class HabilityPush : Hability {

    [SerializeField]
    float pushRange = 7f;

    [SerializeField]
    float pushForce = 20f;

    //NewPlayerManager playerManager;

    void Awake()
    {
        cooldown = 1f;
        habilityName = "Push";
    }

    public override void ExecuteHability()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, pushRange);
        foreach (Collider collider in objects)
        {
            if (collider.gameObject != gameObject && collider.tag == "Player" /*&& !playerManager.IsFriendly(collider.gameObject, gameObject)*/)
            {
                Vector3 pushVector = collider.gameObject.transform.position - transform.position;
                pushVector.y = 0;
                pushVector.Normalize();
                pushVector *= pushForce;
                collider.GetComponent<Rigidbody>().AddExplosionForce(pushForce, transform.position, pushRange, 0f, ForceMode.VelocityChange);
            }
        }
    }
}