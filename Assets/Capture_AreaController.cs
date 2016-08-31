using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Capture_AreaController : MonoBehaviour {

    [SerializeField]
    float basicIncreaseRate;
    [SerializeField]
    float extraIncreaseRate;
    [SerializeField]
    float uncaptureDecreaseRate;
    [SerializeField]
    float fillSpeed;
    [SerializeField]
    int teamsInGame;
    [SerializeField]
    int areaValue;

    float[] captureDone;
    float[] lastCaptureValue;
    bool isCaptured;

    List<GameObject> playersInside;

    CaptureUI_CaptureBars captureBars;
    Capture_GameManager gameManager;

    void Awake()
    {
        playersInside = new List<GameObject>();

        captureDone = new float[teamsInGame];
        lastCaptureValue = new float[teamsInGame];
        captureBars = Component.FindObjectOfType<CaptureUI_CaptureBars>();
        gameManager = Component.FindObjectOfType<Capture_GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (PhotonNetwork.isMasterClient && !isCaptured)
        {
            UpdateCapture();
            CheckIfAreaIsCaptured();
        }
        UpdateCaptureUI();
    }

    void UpdateCapture()
    {
        int teamInside = -1;
        int playersFromTeam = 0;
        bool multipleTeamsInside = false;
        foreach (GameObject player in playersInside)
        {
            int playerTeam = (int)player.GetComponent<PhotonRemoteOwner>().GetPlayer().customProperties[PlayerProperties.team];
            if (teamInside == -1 || teamInside == playerTeam)
            {
                teamInside = playerTeam;
                playersFromTeam += 1;
            }
            else
            {
                multipleTeamsInside = true;
                break;
            }
        }

        if (!multipleTeamsInside)
        {
            for (int i = 0; i < captureDone.Length; i++)
            {
                float value = captureDone[i];
                if (teamInside == i)
                {
                    Debug.Log("PlayersFromTeam" + playersFromTeam);
                    value += (basicIncreaseRate + (extraIncreaseRate * (playersFromTeam - 1))) * Time.deltaTime;
                }
                else
                    value -= uncaptureDecreaseRate * Time.deltaTime;
                captureDone[i] = Mathf.Clamp(value, 0, 1);
            }
        }
    }

    void CheckIfAreaIsCaptured()
    {
        for (int team = 0; team < captureDone.Length; team++)
        {
            if (captureDone[team] == 1)
            {
                isCaptured = true;
                gameManager.Score(team,areaValue);
                break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
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
            lastCaptureValue = (float[])stream.ReceiveNext();
        }
    }

    void UpdateCaptureUI()
    {
        if (!isCaptured)
        {
            if (!PhotonNetwork.isMasterClient)
            {
                //lerp
                for (int i = 0; i < teamsInGame; i++)
                {
                    captureDone[i] = Mathf.Lerp(captureDone[i], lastCaptureValue[i], fillSpeed * Time.deltaTime);
                }
            }
            captureBars.SetCaptureValues(captureDone);
        }
    }
}
