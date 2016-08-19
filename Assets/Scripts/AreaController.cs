using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AreaController : MonoBehaviour
{



    [SerializeField]
    float captureToDo;
    [SerializeField]
    float basicIncreaseRate;
    [SerializeField]
    float extraIncreaseRate;
    [SerializeField]
    float uncaptureDecreaseRate;

    [SerializeField]
    float fillSpeed;

    List<GameObject> playersInside;
    float[] captureDone;
    CaptureGameController gameManager;

    bool isCaptured;
    int teamWhoCaptured;

    Image[] capture;
    float[] lastCaptureValue;


    void Awake()
    {
        isCaptured = false;
        teamWhoCaptured = -1;

        capture = new Image[2];
        lastCaptureValue = new float[2];
        capture[0] = GameObject.Find("Canvas/CaptureUI/RedCapture/Inside").GetComponent<Image>();
        capture[1] = GameObject.Find("Canvas/CaptureUI/GreenCapture/Inside").GetComponent<Image>();

        playersInside = new List<GameObject>();
        captureDone = new float[2];
    }

    void Update()
    {
        if (PhotonNetwork.isMasterClient && !isCaptured)
        {
            UpdateCapture();
            CheckIfAreaIsCaptured();
        }
    }

    void LateUpdate()
    {
        UpdateCaptureUI();
    }

    void UpdateCaptureUI()
    {
        if (!isCaptured)
            if (PhotonNetwork.isMasterClient)
            {
                for (int i = 0; i < capture.Length; i++)
                    capture[i].fillAmount = captureDone[i] / 10f;
            }
            else
            {
                //lerp
                for (int i = 0; i < capture.Length; i++)
                {
                    float newValue = Mathf.Lerp(lastCaptureValue[i], captureDone[i] / 10f, fillSpeed * Time.deltaTime);
                    capture[i].fillAmount = newValue;
                    lastCaptureValue[i] = newValue;
                }
            }
    }

    void ResetCaptureUI()
    {
        for (int i = 0; i < capture.Length; i++)
        {
            lastCaptureValue[i] = 0f;
            capture[i].fillAmount = 0f;
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
                    value += (basicIncreaseRate + (extraIncreaseRate * playersFromTeam - 1)) * Time.deltaTime;
                else
                    value -= uncaptureDecreaseRate * Time.deltaTime;
                captureDone[i] = Mathf.Clamp(value, 0f, captureToDo);
            }
        }
    }

    void CheckIfAreaIsCaptured()
    {
        for (int i = 0; i < captureDone.Length; i++)
        {
            if (captureDone[i] == captureToDo)
            {
                teamWhoCaptured = i;
                isCaptured = true;
                CaptureEvents.CallOnTeamScored(teamWhoCaptured);
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
            captureDone = (float[])stream.ReceiveNext();
        }
    }
}
