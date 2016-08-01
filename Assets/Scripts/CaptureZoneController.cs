using UnityEngine;
using System.Collections.Generic;
using System;

public class CaptureZoneController : Photon.MonoBehaviour {
    [Serializable]
    private class TeamCapture
    {
        public static int VARIABLE_AMOUNT = 3;
        public int currentInside;
        public float timeEmpty;
        public float captureValue;


        public override string ToString()
        {
            return currentInside + "," + timeEmpty + "," + captureValue;
        }

        public TeamCapture() { }

        public TeamCapture(string[] parameters)
        {
            currentInside = int.Parse(parameters[0]);
            timeEmpty = float.Parse(parameters[1]);
            captureValue = float.Parse(parameters[2]);
        }

    }

    private char[] delimiterChars = {',', '-', ':'};
    private Dictionary<int,TeamCapture> captureDictionary;

    [SerializeField]
    private float captureToDo;
    [SerializeField]
    private float basicIncreaseRate;
    [SerializeField]
    private float extraIncreaseRate;
    [SerializeField]
    private float timeBeforeUncaptureStarts;
    [SerializeField]
    private float uncaptureDecreaseRate;

    private bool isCaptured = false;
    private int teamWhoCaptured;

	
    void Awake()
    {
        captureDictionary = new Dictionary<int, TeamCapture>();
        SetNumberOfTeams(GameObject.Find("GameManager(Clone)").GetComponent<CaptureGameController>().getTeamsInMatch());

    }

    public void SetNumberOfTeams(int amount)
    {
        captureDictionary.Clear();
        for ( int i = 0; i<amount; i++)
            captureDictionary.Add(i, new TeamCapture());
        
    }

	// Update is called once per frame
	void Update () {
        if (PhotonNetwork.isMasterClient && !isCaptured)
        {
            bool onlyOneTeamInside = false;
            TeamCapture teamCapturing = null;
            int teamCapturingId = -1;

            foreach (KeyValuePair<int,TeamCapture> team in captureDictionary)
            {
                TeamCapture tc = team.Value;
                if (tc.currentInside == 0)
                {
                    if (timeBeforeUncaptureStarts > tc.timeEmpty)
                        tc.timeEmpty += Time.deltaTime;
                    else if (tc.captureValue > 0f)
                    {
                        float operation = (tc.captureValue - uncaptureDecreaseRate * Time.deltaTime);
                        tc.captureValue = operation > 0f ? operation : 0f;
                    }
                }
                else {
                    if (!onlyOneTeamInside && (teamCapturing == null))
                    {
                        onlyOneTeamInside = true;
                        teamCapturing = tc;
                        teamCapturingId = team.Key;
                    }
                    else
                    {
                        onlyOneTeamInside = false;
                    }
                }
            }

            if (onlyOneTeamInside)
            {
                float operation = teamCapturing.captureValue + (basicIncreaseRate + extraIncreaseRate * (teamCapturing.currentInside - 1)) * Time.deltaTime;
                teamCapturing.captureValue = operation < captureToDo ? operation : captureToDo;
                if (teamCapturing.captureValue >= captureToDo && PhotonNetwork.isMasterClient)
                {
                    setCapturedTrue(teamCapturingId);
                }
            }
        }
	}

    public bool IsCaptured()
    {
        return isCaptured;
    }

    private void setCapturedTrue(int team)
    {
        isCaptured = true;
        teamWhoCaptured = team;
        captureDictionary[teamWhoCaptured].captureValue = captureToDo;
        GameObject.Find("GameManager(Clone)").GetComponent<PhotonView>().RPC("TeamScored", PhotonTargets.AllBufferedViaServer, teamWhoCaptured);
    }

    void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.isMasterClient && other.tag == "Player")
        {
            MatchOptions playerInfo = other.GetComponent<MatchOptions>();
            int playerTeam = playerInfo.GetTeam();
            TeamCapture tc = captureDictionary[playerTeam];
            tc.currentInside += 1;
            if (tc.currentInside == 1)
                tc.timeEmpty = 0f;
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (PhotonNetwork.isMasterClient && other.tag == "Player")
        {
            MatchOptions playerInfo = other.GetComponent<MatchOptions>();
            int playerTeam = playerInfo.GetTeam();
            TeamCapture tc = captureDictionary[playerTeam];
            tc.currentInside -= 1;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo message)
    {
        if (stream.isWriting)
        {
            if (!isCaptured)
            {
                string stringDictionary = "";
                foreach (var team in captureDictionary)
                {
                    if (stringDictionary != "")
                        stringDictionary += "-";
                    stringDictionary += team.Key + ":" + team.Value.ToString();
                }
                stream.SendNext(stringDictionary);
            }
        }
        else
        {
            string stringDictionary = (string)stream.ReceiveNext();
            string[] stringDictionaryWords = stringDictionary.Split(delimiterChars);
            int currentVar = -1;
            int key = -1;
            string[] valueString = new string[TeamCapture.VARIABLE_AMOUNT];
            TeamCapture value;
            captureDictionary = new Dictionary<int, TeamCapture>();
            foreach (string entry in stringDictionaryWords)
            {
                if ( currentVar == -1)
                {
                    key = int.Parse(entry);
                } else
                {
                    valueString[currentVar] = entry;
                }

                if (currentVar == TeamCapture.VARIABLE_AMOUNT - 1)
                {
                    value = new TeamCapture(valueString);
                    captureDictionary.Add(key, value);
                    currentVar = -1;
                }
                else currentVar++;
            }
                        
        }
    }

    void OnGUI()
    {
        string message = "Is Captured: "+isCaptured+"\n";
        foreach (var team in captureDictionary)
            message += team.Key + ": " + team.Value.captureValue + "\n";
        GUI.Box(new Rect(0, 120, 150, 60), message);
    }
}
