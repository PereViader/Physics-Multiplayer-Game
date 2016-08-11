using UnityEngine;
using System.Collections;

public class HabilityJump : Hability {

    [SerializeField]
    private float jumpForce = 100f;

    private bool isBlocked = false;

    private Rigidbody rb;
    private string virtualKeyName;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cooldown = 0.5f;
    }

    protected override void Update()
    {
        base.Update();
        if (!onCooldown && !isBlocked && Input.GetButtonDown(virtualKey))
        {
            isBlocked = true;
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
        SetOnCooldown();
        isBlocked = false;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public override string GetHabilityName()
    {
        return "Jump";
    }
}
