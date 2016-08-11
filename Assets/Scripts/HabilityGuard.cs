using UnityEngine;
using System.Collections;

public class HabilityGuard : Hability {

    [SerializeField]
    private float guardDuration = 1.4f;

    private bool isBlocked = false;
    //private bool isGuarding = false;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cooldown = 4f;
    }

    protected override void Update()
    {
        base.Update();
        if (!onCooldown && !isBlocked && Input.GetButtonDown(virtualKey))
        {
            isBlocked = true;
            photonView.RPC("ExecuteGuardServer", PhotonTargets.MasterClient);
        }
    }

    [PunRPC]
    void ExecuteGuardServer()
    {
        photonView.RPC("ExecuteGuard", PhotonTargets.All);
    }

    [PunRPC]
    void ExecuteGuard()
    {
        onCooldown = true;
        currentCooldown = cooldown;
        isBlocked = false;
        //isGuarding = true;
        rb.isKinematic = true;
        GetComponent<MeshRenderer>().material.color = Color.black;
        Invoke("EndHability", guardDuration);
    }

    private void EndHability()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
        //isGuarding = false;
        rb.isKinematic = false;
    }

    public override string GetHabilityName()
    {
        return "Guard";
    }
}
