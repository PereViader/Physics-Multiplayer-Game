using UnityEngine;
using System.Collections.Generic;

public class HabilityPush : Hability {

    [SerializeField]
    float pushRange = 7f;

    [SerializeField]
    float pushForce = 35f;

    private bool isBlocked = false;

    PhotonPlayer player;

    // Update is called once per frame
    void Awake()
    {
        player = GetComponent<PhotonRemoteOwner>().GetPlayer();
        cooldown = 1f;
    }

    protected override void Update () {
        base.Update();
        if (!onCooldown && !isBlocked && Input.GetButtonDown(virtualKey))
        {
            isBlocked = true;
            photonView.RPC("ExecutePush", PhotonTargets.AllViaServer);
        }
    }

    [PunRPC]
    void ExecutePush()
    {
        currentCooldown = cooldown;
        onCooldown = true;
        isBlocked = false;
        int playerTeam = (int)player.customProperties["Team"];
        foreach (GameObject other in PlayerManager.GetOtherTeamsPlayers(playerTeam))
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(transform.position, other.transform.position - transform.position, out hit, pushRange);
            if ( hasHit )
            {
                int otherTeam = (int)other.GetComponent<Capture_Initializer>().GetPlayer().customProperties["Team"];
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

    public override string GetHabilityName()
    {
        return "Push";
    }
}
