using UnityEngine;
using System.Collections.Generic;

public class HabilityPush : Photon.MonoBehaviour {

    [SerializeField]
    float cooldown = 1f;

    [SerializeField]
    float pushRange = 7f;

    [SerializeField]
    float pushForce = 30f;

    [SerializeField]
    private float currentCooldown;
    [SerializeField]
    private bool onCooldown = false;
    [SerializeField]
    private bool isBlocked = false;
    private string virtualKeyName;

    CaptureGameController gameManager;
    PhotonPlayer photonPlayer;

    // Update is called once per frame
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
        Debug.Log("Execute Push");
        currentCooldown = cooldown;
        photonPlayer = GetComponent<PhotonPlayerOwner>().GetOwner();
        onCooldown = true;
        isBlocked = false;
        if (photonPlayer== null)
        {
            Debug.Log("PhotonPlayer null");
            return;
        }
        int playerTeam = (int)photonPlayer.customProperties["Team"];
        Debug.Log("Own Player team" + playerTeam);
        List<GameObject> otherTeamPlayers = GameObject.Find("GameManager(Clone)").GetComponent<CaptureGameController>().GetOtherTeamsPlayers(playerTeam);

        foreach (GameObject other in otherTeamPlayers)
        {
            Debug.Log("Team other:" + (int)other.GetComponent<PhotonPlayerOwner>().GetOwner().customProperties["Team"]);
            Debug.Log("Player: "+other.transform.position);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(transform.position, other.transform.position - transform.position, out hit, pushRange);
            Debug.Log("Has HIt: " + hasHit);
            if ( hasHit )
            {
                int otherTeam = (int)other.GetComponent<PhotonPlayerOwner>().GetOwner().customProperties["Team"];
                if ( hit.transform.gameObject.tag == "Player" && playerTeam != otherTeam)
                {
                    Vector3 pushVector = ((other.transform.position - transform.position).normalized) * pushForce;
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
