using UnityEngine;
using System.Collections;

public class Capture_AreaManager : MonoBehaviour {

    Transform[] areaPositions;

    GameObject currentArea;
    Transform currentPosition;



    void Awake()
    {
        GameObject container = GameObject.Find("Map/CapturePositions");
        areaPositions = new Transform[container.transform.childCount];
        for (int i = 0; i < container.transform.childCount; i++)
        {
            areaPositions[i] = container.transform.GetChild(i);
        }
    } 


    public void OnGameModeSetup()
    {
        InstantiateNewRandomCapture();
    }

    public void OnGameModeEnded()
    {

    }

    void OnAreaCaptured()
    {
        InstantiateNewRandomCapture();
    }

    public void InstantiateNewRandomCapture()
    {
        if (currentArea != null)
        {
            PhotonNetwork.Destroy(currentArea.gameObject);
        }

        currentPosition = getDiferentRandomAreaPosition();
        currentArea = (GameObject)PhotonNetwork.Instantiate("GameMode/Capture/Area", currentPosition.position, Quaternion.identity, 0);
    }

    public Transform getDiferentRandomAreaPosition()
    {
        Transform newCapturePosition;
        do
        {
            newCapturePosition = GetRandomCapturePosition();
        } while (newCapturePosition == currentPosition);
        return newCapturePosition;
    }

    public Transform GetRandomCapturePosition()
    {
        return areaPositions[Random.Range(0, areaPositions.Length)];
    }

    public GameObject GetCurrentCaptureZone()
    {
        return currentArea;
    }
}
