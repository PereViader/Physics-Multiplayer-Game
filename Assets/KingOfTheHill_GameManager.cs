using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PhotonView))]
public class KingOfTheHill_GameManager : MonoBehaviour {

    [SerializeField]
    private GridMeshGenerator gridMeshGenerator;

    [SerializeField]
    private GameObject map;

    [SerializeField]
    private int scoreToWin;

    [SerializeField]
    private int roundWinExperience;

    [SerializeField]
    private int gameWinExperience;

    [SerializeField]
    private float endGameDelay;

    void Start()
    {
        if (PhotonNetwork.isMasterClient)
            startGame();
    }

    private void startGame()
    {
        initializePlayers();
        prepareNewMap();
        startRound();
    }

    private void initializePlayers()
    {
        foreach( PhotonPlayer player in PhotonNetwork.playerList )
        {
            ExitGames.Client.Photon.Hashtable basicCustomProperties = new ExitGames.Client.Photon.Hashtable();
            basicCustomProperties["score"] = 0;
            basicCustomProperties["experience"] = 0;
            player.SetCustomProperties(basicCustomProperties);
        }
    }

    void prepareNewMap()
    {
        ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.room.customProperties;
        roomProperties["currentMapSeed"] = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        PhotonNetwork.room.SetCustomProperties(roomProperties);
        GetComponent<PhotonView>().RPC("createNewMap", PhotonTargets.All);
    }

    [PunRPC]
    void createNewMap()
    {
        Mesh mapMesh = gridMeshGenerator.generateMap((int)PhotonNetwork.room.customProperties["currentMapSeed"]);
        map.GetComponent<MeshFilter>().mesh = null;
        map.GetComponent<MeshCollider>().sharedMesh = null;

        map.GetComponent<MeshFilter>().mesh = mapMesh;
        map.GetComponent<MeshCollider>().sharedMesh = mapMesh;
    }

    void startRound()
    {
        foreach(PhotonPlayer player in PhotonNetwork.playerList)
        {
            spawnPlayer(player);
        }
    }

    private void spawnPlayer(PhotonPlayer player)
    {
        ExitGames.Client.Photon.Hashtable customProperties = player.customProperties;
        customProperties["isAlive"] = true;
        player.SetCustomProperties(customProperties);

        // TODO spawn player in game
    }


    public void playerDied(PhotonPlayer player)
    {
        ExitGames.Client.Photon.Hashtable customProperties = player.customProperties;
        customProperties["isAlive"] = true;
        player.SetCustomProperties(customProperties);

        // kill player
        PhotonPlayer winner;
        if (checkEndOfRound(out winner))
        {
            int experience = roundWinExperience;
            bool hasGameEnded = checkEndOfGame();
            if (hasGameEnded)
                experience += gameWinExperience;
            
            awardExperienceToPlayer(winner, experience);

            if(hasGameEnded)
                GetComponent<PhotonView>().RPC("endGame", PhotonTargets.All);
        }
    }

    [PunRPC]
    void endGame()
    {
        // Move to end game scene
        Debug.Log("End game");
    }

    private void awardExperienceToPlayer(PhotonPlayer player, int experience)
    {
        ExitGames.Client.Photon.Hashtable customProperties = player.customProperties;
        customProperties["experience"] = experience + (int)customProperties["experience"];
        player.SetCustomProperties(customProperties);
    }

    private bool checkEndOfRound(out PhotonPlayer roundWinner)
    {
        int playersAlive = 0;
        roundWinner = null;
        IEnumerator<PhotonPlayer> players = (IEnumerator<PhotonPlayer>)PhotonNetwork.playerList.GetEnumerator();
        while (players.MoveNext() && playersAlive<= 1)
        {
            if ( (bool)players.Current.customProperties["isAlive"])
            {
                playersAlive++;
                roundWinner = players.Current;
            }
        }

        if (playersAlive > 1) roundWinner = null;
        return playersAlive <= 1;
    }

    private bool checkEndOfGame(ref PhotonPlayer winner)
    {
        winner = null;
        foreach(PhotonPlayer player in PhotonNetwork.playerList)
        {
            if ( (int)player.customProperties["score"] == scoreToWin )
            {
                winner = player;
                return true;
            }
        }

        return false;
    }

    private bool checkEndOfGame()
    {
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            if ((int)player.customProperties["score"] == scoreToWin)
            {
                return true;
            }
        }

        return false;
    }
}