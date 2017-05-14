using UnityEngine;
using System.Collections;
using System;

public class Capture_AreaManager : MonoBehaviour, IGame {

    // Transform de les posicions on es poden crear areas de captura
    [SerializeField]
    private Transform captureAreaParent;

    private Transform previousArea;

    public void OnGameSetup()
    {
    }

    public void OnGameStart()
    {
    }

    public void OnRoundSetup()
    {
        if (PhotonNetwork.isMasterClient)
            InstantiateNewRandomCapture();
    }

    public void OnRoundStart()
    {
    }

    public void OnRoundEnd()
    {
        if (PhotonNetwork.isMasterClient)
            RemoveOldArea();
    }

    public void OnGameEnd()
    {
    }

    void RemoveOldArea()
    {
        Capture_AreaController currentArea = Component.FindObjectOfType<Capture_AreaController>();
        if (currentArea != null)
        {
            PhotonNetwork.Destroy(currentArea.gameObject);
        }
        previousArea = currentArea.transform;
    }

    void InstantiateNewRandomCapture()
    {
        Transform newTransform = getDiferentRandomAreaPosition(previousArea);
        PhotonNetwork.InstantiateSceneObject("GameMode/Area", newTransform.position, newTransform.rotation, 0, new object[0]);
    }

    Transform getDiferentRandomAreaPosition(Transform previousArea)
    {
        Transform newCapturePosition;
        do
        {
            newCapturePosition = GetRandomCapturePosition();
        } while (newCapturePosition == previousArea);
        return newCapturePosition;
    }

    Transform GetRandomCapturePosition()
    {
        return captureAreaParent.GetChild(UnityEngine.Random.Range(0, captureAreaParent.childCount));
    }
}
