using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShootingController : MonoBehaviour {

	[SerializeField]
	private Image fill;

	[SerializeField]
	private GameObject gFill;


	private bool isActive = false;
	private bool isInnactive = true;

	private int power = 0;
	[SerializeField]
	private int STEP_POWER = 1;
	[SerializeField]
	private int MAX_POWER = 11;
	[SerializeField]
	private float timeBetween;
	[SerializeField]
	private float timeToDisappear;

	public bool activate() {
		if (isInnactive) {
			isActive = true;
			isInnactive = false;
			gFill.SetActive (true);
			StartCoroutine ("doPower");
			return true;
		} else {
			return false;
		}
	}

    public bool IsActive() { return isActive;  }

	IEnumerator doPower() {
		while (isActive && power < MAX_POWER) {
			power = power + STEP_POWER;
			fill.fillAmount = power / 10.0f;
			yield return new WaitForSeconds(timeBetween);
		}

        yield return new WaitWhile(IsActive);

		StartCoroutine ("delayedDisappear");
	}

	IEnumerator delayedDisappear() {
		yield return new WaitForSeconds (timeToDisappear);
		isInnactive = true;
		gFill.SetActive (false);
	}

	public float getPower() {
		isActive = false;
		float ret = power / 10.0f;
		power = 0;
		return ret;
	}
}
