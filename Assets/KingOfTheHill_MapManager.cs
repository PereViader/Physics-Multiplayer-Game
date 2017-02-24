using UnityEngine;
using System.Collections;
using System;

public class KingOfTheHill_MapManager : MonoBehaviour, IGame {

    [SerializeField]
    private GridMeshGenerator gridMeshGenerator;

    [SerializeField]
    private GameObject map;

    [SerializeField]
    private GameObject deadZone;

    public void OnGameSetup() { }

    public void OnGameStart() { }

    public void OnRoundSetup()
    {
        prepareNewMap();
    }

    public void OnRoundStart() { }

    public void OnRoundEnd() { }

    public void OnGameEnd() { }


    public void prepareNewMap()
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

        deadZone.transform.localScale = new Vector3(gridMeshGenerator.getMeshWidth(), gridMeshGenerator.getMeshHeight(), 1);
    }


}
