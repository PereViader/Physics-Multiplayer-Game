using UnityEngine;
using System.Collections;

public class HabilityStop : Hability {

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cooldown = 0.7f;
    }

    override protected void Update()
    {
        base.Update();
        if (!onCooldown && Input.GetButtonDown(virtualKey))
        {
            photonView.RPC("ExecuteStop", PhotonTargets.AllViaServer);
            SetOnCooldown();
        }
    }

    [PunRPC]
    void ExecuteStop()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public override string GetHabilityName()
    {
        return "Stop";
    }
}
