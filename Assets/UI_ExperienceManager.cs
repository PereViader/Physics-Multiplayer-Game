using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class UI_ExperienceManager : MonoBehaviour {

    [SerializeField]
    private Text experienceUI;

    [SerializeField]
    private float delayBetweenDisplays;

    [SerializeField]
    private float displayTimeForEach;

    private bool isDisplaying;
    List<int> toDisplay;
    int previousExperience;

    void Awake()
    {
        previousExperience = 0;
        toDisplay = new List<int>();
    }

    void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        PhotonPlayer player = playerAndUpdatedProps[0] as PhotonPlayer;
        ExitGames.Client.Photon.Hashtable props = (ExitGames.Client.Photon.Hashtable)playerAndUpdatedProps[1];

        int experience = (int)props[PlayerProperties.experience];
        if (PhotonNetwork.player == player && previousExperience < experience)
        {
            DisplayAddedExperience(experience - previousExperience);
            previousExperience = experience;
        }
    }

    public void DisplayAddedExperience(int amount)
    {
        toDisplay.Add(amount);
        if (!isDisplaying)
            StartCoroutine(DisplayWhileAviable());
    }

    IEnumerator DisplayWhileAviable()
    {
        isDisplaying = true;
        while (toDisplay.Count > 0)
        {
            int current = toDisplay[0];
            toDisplay.RemoveAt(0);
            experienceUI.text = "+" + current.ToString();
            experienceUI.gameObject.SetActive(true);
            yield return new WaitForSeconds(displayTimeForEach);
            experienceUI.gameObject.SetActive(false);
            yield return new WaitForSeconds(delayBetweenDisplays);
        }
        isDisplaying = false;
    }
}
