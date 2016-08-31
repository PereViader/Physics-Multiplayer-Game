using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CaptureUI_PowerBarManager : MonoBehaviour {
    GameObject powerBar;
    Image inside;


    void Awake()
    {
        powerBar = GameObject.Find("Canvas").transform.Find("PowerBar").gameObject;
        inside = powerBar.transform.Find("Inside").GetComponent<Image>();
    }

	public void SetActive(bool state)
    {
        if ( powerBar )
            powerBar.SetActive(state);
    }

    public void SetFill(float fill)
    {
        inside.fillAmount = fill;
    }
}
