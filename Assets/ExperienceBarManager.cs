using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExperienceBarManager : MonoBehaviour {

    Text nextLevel;
    Text currentLevel;
    Image experienceBar;

    [SerializeField]
    float startingDelay;

    [SerializeField]
    float levelUpDelay;

    [SerializeField]
    float experienceSpeed;

    void Awake()
    {
        /*PlayerExperience.SetLevel(1);
        PlayerExperience.SetExperience(300);*/
        nextLevel = GameObject.Find("NextLevel").GetComponent<Text>();
        currentLevel = GameObject.Find("CurrentLevel").GetComponent<Text>();
        experienceBar = GameObject.Find("ExperienceBar/Bar/Inside").GetComponent<Image>();
        InitializeValues();
    }

    void InitializeValues()
    {
        int playerLevel = PlayerExperience.GetLevel();
        SetLevels(playerLevel);
        experienceBar.fillAmount = PlayerExperience.GetExperiencePercentageOfLevel();
    }

    void SetLevels(int playerLevel)
    {
        currentLevel.text = playerLevel.ToString();
        nextLevel.text = (playerLevel + 1).ToString();
    }
    
    public void AddExperienceAndAnimate(int experience)
    {
        StartCoroutine(Coroutine_AddExperienceAndAnimate(experience));
    }

    IEnumerator Coroutine_AddExperienceAndAnimate(int experience)
    {
        yield return new WaitForSeconds(startingDelay);
        int level = PlayerExperience.GetLevel();
        PlayerExperience.AddExperience(experience);
        int endingPlayerLevel = PlayerExperience.GetLevel();
        float endingPlayerPercentageExperience = PlayerExperience.GetExperiencePercentageOfLevel();
        bool animateComplete = false;
        while(!animateComplete)
        {
            float newFillAmount;
            float fillAmountIncrease = experienceSpeed * Time.deltaTime;
            if ( level < endingPlayerLevel)
            {
                newFillAmount = Mathf.Clamp(experienceBar.fillAmount + fillAmountIncrease, 0f, 1f);
                experienceBar.fillAmount = newFillAmount;
                if ( newFillAmount == 1) // level up
                {
                    level += 1;
                    SetLevels(level);
                    yield return new WaitForSeconds(levelUpDelay);
                    experienceBar.fillAmount = 0;
                } else
                {
                    yield return null;
                }
            }
            else
            {
                newFillAmount = Mathf.Clamp(experienceBar.fillAmount + fillAmountIncrease, 0f, endingPlayerPercentageExperience);
                experienceBar.fillAmount = newFillAmount;
                if (newFillAmount == endingPlayerPercentageExperience)
                    animateComplete = true;
                else
                    yield return null;
            }
        }
    }
}
