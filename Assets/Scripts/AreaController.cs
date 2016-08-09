using UnityEngine;
using System.Collections.Generic;

public class AreaController : MonoBehaviour {

    

    [SerializeField]
    float captureToDo;
    [SerializeField]
    float basicIncreaseRate;
    [SerializeField]
    float extraIncreaseRate;
    [SerializeField]
    float uncaptureDecreaseRate;

    List<GameObject> playersInside;
    Dictionary<int, float> captureDone;

    CaptureGameController gameManager;

    bool isCaptured;
    int teamWhoCaptured;

    void Awake()
    {
        isCaptured = false;
        teamWhoCaptured = -1;

        playersInside = new List<GameObject>();
        captureDone = new Dictionary<int, float>();
        captureDone.Add(1, 0f);
        captureDone.Add(2, 0f);
    }

    void Update()
    {
        if (PhotonNetwork.isMasterClient && !isCaptured)
        {
            UpdateCapture();
            CheckIfAreaIsCaptured();
        }
    }

    void UpdateCapture()
    {
        int teamInside = -1;
        int playersFromTeam = 0;
        bool multipleTeamsInside = false;
        foreach (GameObject player in playersInside)
        {
            PhotonPlayer photonPlayer = player.GetComponent<PhotonPlayerOwner>().GetOwner();
            int playerTeam = (int)photonPlayer.customProperties["Team"];
            if (teamInside == -1)
            {
                teamInside = playerTeam;
                playersFromTeam += 1;
            }
            else if (teamInside == playerTeam)
            {
                playersFromTeam += 1;
            }
            else
            {
                multipleTeamsInside = true;
                break;
            }
        }
        
        if ( !multipleTeamsInside )
        {
            for (int i = 1; i<=captureDone.Count; i++) 
            {
                float value = captureDone[i]; 
                if ( teamInside == i )
                    value += (basicIncreaseRate + (extraIncreaseRate * playersFromTeam - 1)) * Time.deltaTime;
                else
                    value -= uncaptureDecreaseRate*Time.deltaTime;
                captureDone[i] = Mathf.Clamp(value, 0f, captureToDo);
            }
        }
    }

    void CheckIfAreaIsCaptured()
    {
        foreach (var team in captureDone)
        {
            if ( team.Value == captureToDo)
            {
                teamWhoCaptured = team.Key;
                isCaptured = true;
                gameManager.GetComponent<PhotonView>().RPC("TeamScored", PhotonTargets.AllBufferedViaServer, teamWhoCaptured);
                break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "Player")
        {
            playersInside.Add(other.gameObject);
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playersInside.Remove(other.gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo message)
    {
        if (stream.isWriting)
        {
            stream.SendNext(captureDone);
        }
        else
        {
            captureDone = (Dictionary<int, float>)stream.ReceiveNext();
        }
    }

    public void SetGameManager(CaptureGameController gameManager)
    {
        this.gameManager = gameManager;
    }

    void OnGUI()
    {
        string message = "";
        foreach (var team in captureDone)
            message += team.Key + ": " + team.Value + "\n";
        GUI.Box(new Rect(0, 120, 150, 60), message);
    }
}
