using UnityEngine;
using System.Collections;

public class HabilityStop : Hability {

    private bool isBlocked = false;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cooldown = 0.7f;
    }

    override protected void Update()
    {
        base.Update();
        if (!onCooldown && !isBlocked && Input.GetButtonDown(virtualKey))
        {
            isBlocked = true;
            photonView.RPC("ExecuteStop", PhotonTargets.AllViaServer);
        }
    }

    [PunRPC]
    void ExecuteStop()
    {
        onCooldown = true;
        currentCooldown = cooldown;
        isBlocked = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public override string GetHabilityName()
    {
        return "Stop";
    }
}
