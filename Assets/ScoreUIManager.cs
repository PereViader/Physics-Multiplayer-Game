using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreUIManager : MonoBehaviour {
    Text text;

    [SerializeField]
    float displayTime;

    void Awake()
    {
        text = GameObject.Find("Canvas").transform.Find("ExperienceText").GetComponent<Text>();
    }

    void OnExperienceIncreased(int experience)
    {
        if (text.enabled)
            StopAllCoroutines();
        StartCoroutine(DisplayAndFade(experience));
    }

	IEnumerator DisplayAndFade(int experience)
    {
        text.text = "+ "+experience.ToString();
        text.enabled = true;
        yield return new WaitForSeconds(displayTime);
        text.enabled = false;
    }
}
