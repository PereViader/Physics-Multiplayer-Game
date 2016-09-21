using UnityEngine;
using System.Collections;

public class Capture_AreaManager : MonoBehaviour, ISetup {

    Transform[] areaPositions;

    void Awake()
    {
        GameObject container = GameObject.Find("Map/CapturePositions");
        areaPositions = new Transform[container.transform.childCount];
        for (int i = 0; i < container.transform.childCount; i++)
        {
            areaPositions[i] = container.transform.GetChild(i);
        }
    } 


    public void OnGameSetup()
    {
        if ( PhotonNetwork.isMasterClient )
            InstantiateNewRandomCapture();
    }

    void OnAreaCaptured()
    {
        InstantiateNewRandomCapture();
    }

    public void InstantiateNewRandomCapture()
    {
        Capture_AreaController currentArea = Component.FindObjectOfType<Capture_AreaController>();
        Transform currentAreaTransform = null;
        if (currentArea != null)
        {
            currentAreaTransform = currentArea.transform;
            PhotonNetwork.Destroy(currentArea.gameObject);
        }

        Transform newTransform = getDiferentRandomAreaPosition(currentAreaTransform);
        PhotonNetwork.InstantiateSceneObject("GameMode/Capture/Area", newTransform.position, newTransform.rotation, 0, new object[0]);
    }

    public Transform getDiferentRandomAreaPosition(Transform current)
    {
        Transform newCapturePosition;
        do
        {
            newCapturePosition = GetRandomCapturePosition();
        } while (newCapturePosition == current);
        return newCapturePosition;
    }

    public Transform GetRandomCapturePosition()
    {
        return areaPositions[Random.Range(0, areaPositions.Length)];
    }
}
