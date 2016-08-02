using UnityEngine;
using System.Collections.Generic;

public class HabilityPush : Photon.MonoBehaviour {

    [SerializeField]
    float cooldown = 1f;

    [SerializeField]
    float pushRange = 7f;

    [SerializeField]
    float pushForce = 35f;

    [SerializeField]
    private float currentCooldown;
    [SerializeField]
    private bool onCooldown = false;
    [SerializeField]
    private bool isBlocked = false;
    private string virtualKeyName;

    CaptureGameController gameManager;
    PhotonPlayerOwner photonPlayerOwner;

    // Update is called once per frame
    void Awake()
    {
        photonPlayerOwner = GetComponent<PhotonPlayerOwner>();
        gameManager = GameObject.Find("GameManager(Clone)").GetComponent<CaptureGameController>();
    }

    void Update () {
        if (onCooldown)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0f)
            {
                onCooldown = false;
                currentCooldown = 0f;
            }
        }
        else if (!isBlocked && Input.GetButtonDown(virtualKeyName))
        {
            isBlocked = true;
            photonView.RPC("ExecutePushServer", PhotonTargets.MasterClient);
        }
    }

    [PunRPC]
    void ExecutePushServer()
    {
        photonView.RPC("ExecutePush", PhotonTargets.All);
    }

    [PunRPC]
    void ExecutePush()
    {
        currentCooldown = cooldown;
        onCooldown = true;
        isBlocked = false;
        int playerTeam = (int)photonPlayerOwner.GetOwner().customProperties["Team"];
        foreach (GameObject other in gameManager.GetOtherTeamsPlayers(playerTeam))
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(transform.position, other.transform.position - transform.position, out hit, pushRange);
            if ( hasHit )
            {
                int otherTeam = (int)other.GetComponent<PhotonPlayerOwner>().GetOwner().customProperties["Team"];
                if ( hit.transform.gameObject.tag == "Player" && playerTeam != otherTeam)
                {
                    Vector3 pushVector = other.transform.position - transform.position;
                    pushVector.y = 0;
                    pushVector.Normalize();
                    pushVector *= pushForce;
                   
                    other.GetComponent<PlayerControllerPast>().ShootPlayer(pushVector);
                }
            }
        }
    }

    public void SetVirtualKey(string virtualKeyName)
    {
        this.virtualKeyName = virtualKeyName;
    }

    void OnGUI()
    {
        GUI.Box(new Rect(0f, 460f, 130, 20), "Push: " + currentCooldown);
    }
}
