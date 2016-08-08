using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShootingController : MonoBehaviour {

	[SerializeField]
	private Image fill;

	[SerializeField]
	private GameObject gFill;

    [SerializeField]
    private int powerIncrement;

    [SerializeField]
    private float timeToDisappear;


    private bool isActive = false;
	private float currentPower;



	public void SetActive(bool state) {
        if (state != isActive)
        {
            isActive = state;
            if (state)
                gFill.SetActive(true);
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
            fill.fillAmount = currentPower;
        }
    }

	IEnumerator delayedDisappear() {
		yield return new WaitForSeconds (timeToDisappear);
        gFill.SetActive(false);
    }

	public float getPower() {
        return currentPower;
	}
}
