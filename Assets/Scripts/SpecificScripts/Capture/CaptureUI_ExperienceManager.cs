using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CaptureUI_ExperienceManager : MonoBehaviour {

    [SerializeField]
    float displayTime;

    [SerializeField]
    float delayTime;

    WaitForSeconds displayDelay;
    WaitForSeconds delayBetween;

    Text experienceUI;
    List<int> toDisplay;
    bool isDisplaying;
    void Awake()
    {
        toDisplay = new List<int>();
        displayDelay = new WaitForSeconds(displayTime);
        delayBetween = new WaitForSeconds(delayTime);
        experienceUI = GameObject.Find("Canvas/GameUI").transform.Find("ExperienceText").GetComponent<Text>();
    }

    public void DisplayAddedExperience(int amount)
    {
        toDisplay.Add(amount);
        if ( !isDisplaying )
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
            yield return displayDelay;
            experienceUI.gameObject.SetActive(false);
            yield return delayBetween;
        }
        isDisplaying = false;
    }


}
