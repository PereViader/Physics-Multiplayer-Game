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
            previousArea = currentArea.transform;
            PhotonNetwork.Destroy(currentArea.gameObject);
        } else {
            Debug.LogError("No area was found when trying to remove old area");
        }
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
        } while (previousArea != null && newCapturePosition.position == previousArea.position);
        return newCapturePosition;
    }

    Transform GetRandomCapturePosition()
    {
        return captureAreaParent.GetChild(UnityEngine.Random.Range(0, captureAreaParent.childCount));
    }
}