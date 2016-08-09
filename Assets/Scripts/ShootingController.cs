using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShootingController : MonoBehaviour {

    GameObject shootingControllerUI;

	private Image movingBar;

    [SerializeField]
    private int powerIncrement;

    [SerializeField]
    private float timeToDisappear;


    private bool isActive = false;
	private float currentPower;

    void Awake()
    {
        shootingControllerUI = GameObject.Find("Canvas").transform.Find("PowerBar").gameObject;
        movingBar = shootingControllerUI.transform.GetChild(0).GetComponent<Image>();
    }

	public void SetActive(bool state) {
        if (state != isActive)
        {
            isActive = state;
            if (state)
                shootingControllerUI.SetActive(true);
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
        shootingControllerUI.SetActive(false);
    }

	public float getPower() {
        return currentPower;
	}
}
