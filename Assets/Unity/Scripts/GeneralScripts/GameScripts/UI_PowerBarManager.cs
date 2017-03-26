using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_PowerBarManager: MonoBehaviour {
    [SerializeField]
    private Image bar;
    [SerializeField]
    private GameObject powerBar;
	
	public void setFill(float value)
    {
        bar.fillAmount = value;
    }

    public void setDisplay(bool state)
    {
        powerBar.SetActive(state);
    }
}
