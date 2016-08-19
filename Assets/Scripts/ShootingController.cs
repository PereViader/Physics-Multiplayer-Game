using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShootingController : MonoBehaviour {

    GameObject powerBar;

	private Image movingBar;

    [SerializeField]
    private int powerIncrement;

    [SerializeField]
    private float timeToDisappear;


    private bool isActive = false;
	private float currentPower;

    void Awake()
    {
        powerBar = GameObject.Find("Canvas").transform.Find("PowerBar").gameObject;
        movingBar = powerBar.transform.GetChild(0).GetComponent<Image>();
    }

	public void SetActive(bool state) {
        if (state != isActive)
        {
            isActive = state;
            if (state)
            {
                powerBar.SetActive(true);
                StopAllCoroutines();
            }
            else
            {
                currentPower = 0f;
                StartCoroutine(delayedDisappear());
            }
        }
    }

    public bool IsActive() { return isActive;  }


    void Update()
    {
        if ( isActive )
        {
            currentPower = Mathf.Clamp(currentPower + powerIncrement * Time.deltaTime, 0f, 1f);
            movingBar.fillAmount = currentPower;
        }
    }

	IEnumerator delayedDisappear() {
		yield return new WaitForSeconds (timeToDisappear);
        powerBar.SetActive(false);
    }

	public float getPower() {
        return currentPower;
	}
}
