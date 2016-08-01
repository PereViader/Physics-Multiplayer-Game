using UnityEngine;
using System.Collections;

public class HabilityJump : Photon.MonoBehaviour {

    [SerializeField]
    private float jumpForce = 6f;

    [SerializeField]
    private float cooldown = 2f;

    private bool isBlocked = false;
    private bool isJumping = false;
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
                onCooldown = false; 
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
        isJumping = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other)
    {
        if ( isJumping && other.gameObject.name == "Floor")
        {
            isJumping = false;
            transform.position = new Vector3(transform.position.x, -0.15f, transform.position.z);
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY ;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
    }
}
