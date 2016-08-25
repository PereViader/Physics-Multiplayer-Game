using UnityEngine;
using System.Collections;

public class CaptureZoneManager : MonoBehaviour {
    public static CaptureZoneManager captureZoneManager;

    Transform[] capturePositions;

    public GameObject currentCapture;
    public Transform currentPosition;

    public delegate void ZoneCreation(GameObject newZone);


    void Awake()
    {
        if (captureZoneManager != null)
            Debug.Log("There can not be more than one capture zone managers");
        captureZoneManager = this;

        GameObject container = GameObject.Find("Map/CapturePositions");
        capturePositions = new Transform[container.transform.childCount];
        for (int i = 0; i < container.transform.childCount; i++)
        {
            capturePositions[i] = container.transform.GetChild(i);
        }
    }

    public void InstantiateNewRandomCapture()
    {
        if (currentPosition != null)
        {
            PhotonNetwork.Destroy(currentCapture.gameObject);
        }

        currentPosition = getDiferentRandomCapturePosition();
        currentCapture = (GameObject)PhotonNetwork.Instantiate("Capture Zone", currentPosition.position, Quaternion.identity, 0);
    }

    public Transform getDiferentRandomCapturePosition()
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
        return capturePositions[Random.Range(0, capturePositions.Length)];
    }

    public GameObject GetCurrentCaptureZone()
    {
        return currentCapture;
    }
}
