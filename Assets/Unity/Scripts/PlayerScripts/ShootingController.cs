using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShootingController : MonoBehaviour {

    [SerializeField]
    private int powerIncrement;

    [SerializeField]
    private float timeToDisappear;

    CaptureUI_PowerBarManager powerBar;

    private bool isActive = false;
	private float currentPower;

    void Awake()
    {
        powerBar = Component.FindObjectOfType<CaptureUI_PowerBarManager>();
    }

    void OnDestroy()
    {
        SetVisible(false);
        StopAllCoroutines();
    }

	public void SetActive(bool state) {
        if (state != isActive)
        {
            isActive = state;
            if (isActive)
            {
                StopAllCoroutines();
                SetVisible(true);
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
            powerBar.SetFill(currentPower);
        }
    }

    void SetVisible(bool state)
    {
        if ( powerBar != null )
            powerBar.SetActive(state);
    }

	IEnumerator delayedDisappear() {
		yield return new WaitForSeconds (timeToDisappear);
        SetVisible(false);
    }

	public float getPower() {
        return currentPower;
	}
}
