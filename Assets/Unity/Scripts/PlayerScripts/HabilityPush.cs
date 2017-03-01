using UnityEngine;
using System.Collections.Generic;

public class HabilityPush : Hability {

    [SerializeField]
    float pushRange = 7f;

    [SerializeField]
    float pushForce = 20f;

    //NewPlayerManager playerManager;

    void Awake()
    {
        //playerManager = Component.FindObjectOfType<NewPlayerManager>();
        cooldown = 1f;
    }

    protected override void Update () {
        base.Update();
        if (!onCooldown && Input.GetButtonDown(virtualKey))
        {
            SetOnCooldown();
            photonView.RPC("ExecutePush", PhotonTargets.AllViaServer);
        }
    }

    [PunRPC]
    void ExecutePush()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, pushRange);
        foreach( Collider collider in objects) {
            if ( collider.tag == "Player" /*&& !playerManager.IsFriendly(collider.gameObject, gameObject)*/)
            {
                Vector3 pushVector = collider.gameObject.transform.position - transform.position;
                pushVector.y = 0;
                pushVector.Normalize();
                pushVector *= pushForce;
                collider.GetComponent<Rigidbody>().AddExplosionForce(pushForce, transform.position, pushRange,0f,ForceMode.VelocityChange);
            }
        }
    }

    public override string GetHabilityName()
    {
        return "Push";
    }
}