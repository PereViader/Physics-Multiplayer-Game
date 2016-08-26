using UnityEngine;
using System.Collections;

public class PlayerScoreManager : MonoBehaviour {
    [SerializeField]
    static float scorePoints;

    [SerializeField]
    static float captureHelpScore;

    [SerializeField]
    static float killScore;

    [SerializeField]
    static float winGame;

    [SerializeField]
    static float lateJoin;

    float score;

    PhotonPlayer player;

    void Awake()
    {
        CaptureEvents.OnPlayerKilled += OnPlayerKilled;
        CaptureEvents.OnGameEnded += OnMatchEnded;
        CaptureEvents.OnJoinedStartedGame += OnJoinedSartedGame;
        CaptureEvents.OnTeamScored += OnTeamCaptured;
    }

    void OnDestroy()
    {
        CaptureEvents.OnPlayerKilled -= OnPlayerKilled;
        CaptureEvents.OnGameEnded -= OnMatchEnded;
        CaptureEvents.OnJoinedStartedGame -= OnJoinedSartedGame;
        CaptureEvents.OnTeamScored -= OnTeamCaptured;
    }

    void OnPlayerKilled(PhotonPlayer killer)
    {
        if (killer == PhotonNetwork.player)
        {
            score += killScore;
            Debug.Log("MoreScore");
        }
    }

    void OnMatchEnded(int team)
    {
        if (team == (int)PhotonNetwork.player.customProperties["Team"])
            score += winGame;
    }

    void OnJoinedSartedGame()
    {
        score += lateJoin;
    }

    void OnTeamCaptured(int team)
    {
        if (team == (int)PhotonNetwork.player.customProperties["Team"])
            score += scorePoints;
    }
}
