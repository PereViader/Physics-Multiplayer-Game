using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CaptureUI_CaptureBars : MonoBehaviour {

    [SerializeField]
    int teamsInGame;

    Image[] captureBar;

    void Awake()
    {
        GameObject captureUI = GameObject.Find("Canvas/GameUI/CaptureUI");
        captureBar = new Image[teamsInGame];
        for (int i = 0; i < teamsInGame; i++)
            captureBar[i] = captureUI.transform.Find("Team" + i+"/Inside").GetComponent<Image>();
    }

    public void SetCaptureValues(float[] values)
    {
        for ( int i = 0; i< teamsInGame; i++)
        {
            captureBar[i].fillAmount = values[i];
        }
    }
}
