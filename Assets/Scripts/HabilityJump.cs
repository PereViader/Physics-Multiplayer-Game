using UnityEngine;
using System.Collections;

public class HabilityJump : Photon.MonoBehaviour {

    [SerializeField]
    private float jumpForce = 6f;

    [SerializeField]
    private float cooldown = 2f;

    private bool isBlocked = false;
    private bool onCooldown = false;
    private float currentCooldown = 0;

    private Rigidbody rb;
    private string virtualKeyName;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetVirtualKey(string virtualKeyName)
    {
        this.virtualKeyName = virtualKeyName;
    }

    void Update()
    {
        if ( onCooldown )
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0f)
            {
                onCooldown = false;
                currentCooldown = 0f;
            }
                
        } else if (!isBlocked && Input.GetButtonDown(virtualKeyName))
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
        onCooldown = true;
        currentCooldown = cooldown;
        isBlocked = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnGUI()
    {
        GUI.Box(new Rect(0f, 400f, 130, 20), "Jump: " + currentCooldown);
    }
}
