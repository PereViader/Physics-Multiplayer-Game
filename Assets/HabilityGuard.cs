using UnityEngine;
using System.Collections;

public class HabilityGuard : Photon.MonoBehaviour {

    [SerializeField]
    private float guardDuration;

    [SerializeField]
    private float cooldown;

    private bool isBlocked = false;
    private bool isGuarding = false;
    private bool onCooldown = false;
    private float currentCooldown = 0;

    private int habilityNumber = 2;

    private Rigidbody rb;
    private string virtualKeyName;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        virtualKeyName = "Hability" + habilityNumber;
    }


    void Update()
    {
        if (onCooldown && !isGuarding)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0f)
                onCooldown = false;
        }
        else if (!isBlocked && Input.GetButtonDown(virtualKeyName))
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
        isGuarding = true;
        rb.isKinematic = true;
        GetComponent<MeshRenderer>().material.color = Color.black;
        Invoke("EndHability", guardDuration);
    }

    private void EndHability()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
        isGuarding = false;
        rb.isKinematic = false;
    }
}
