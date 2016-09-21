using UnityEngine;
using System.Collections;

public class HabilityGuard : Hability {

    [SerializeField]
    private float guardDuration = 1.4f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cooldown = 4f;
    }

    override protected void Update()
    {
        base.Update();
        if (!onCooldown && Input.GetButtonDown(virtualKey))
        {
            SetOnCooldown();
            photonView.RPC("ExecuteGuard", PhotonTargets.AllViaServer);
        }
    }

    [PunRPC]
    void ExecuteGuard()
    {
        rb.isKinematic = true;
        GetComponent<MeshRenderer>().material.color = Color.black;
        Invoke("EndHability", guardDuration);
    }

    private void EndHability()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
        rb.isKinematic = false;
    }

    public override string GetHabilityName()
    {
        return "Guard";
    }
}
