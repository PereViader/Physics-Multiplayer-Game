using UnityEngine;
using System.Collections;

public class HabilityJump : Hability {

    [SerializeField]
    private float jumpForce = 600f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cooldown = 0.5f;
    }

    protected override void Update()
    {
        base.Update();
        if (!onCooldown && Input.GetButtonDown(virtualKey))
        {
            SetOnCooldown();
            photonView.RPC("ExecuteJumpServer", PhotonTargets.MasterClient);
        }
    }

    [PunRPC]
    void ExecuteJumpServer()
    {
        photonView.RPC("ExecuteJump", PhotonTargets.All);
    }

    [PunRPC]
    void ExecuteJump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public override string GetHabilityName()
    {
        return "Jump";
    }
}
