using UnityEngine;
using System.Collections;
using System;

public class Capture_AreaManager : MonoBehaviour, IGame {

    // Transform de les posicions on es poden crear areas de captura
    [SerializeField] private Transform[] areaPositions;

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

    public void RemoveOldArea()
    {
        Capture_AreaController currentArea = Component.FindObjectOfType<Capture_AreaController>();
        if (currentArea != null)
        {
            PhotonNetwork.Destroy(currentArea.gameObject);
        }
    }

    public void InstantiateNewRandomCapture()
    {
        Transform newTransform = getDiferentRandomAreaPosition(null);
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
        return areaPositions[UnityEngine.Random.Range(0, areaPositions.Length)];
    }


}
