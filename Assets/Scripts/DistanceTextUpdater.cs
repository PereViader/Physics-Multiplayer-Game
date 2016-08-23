using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DistanceTextUpdater : MonoBehaviour {
    /*
    Text distanceText;
    GameObject distanceObject;

    void Awake()
    {
        //distanceText = GameObject.Find("Canvas").transform.Find("DistanceText").GetComponent<Text>();
        CaptureEvents.OnCaptureZoneCreated += SetObject;
        SetObject(CaptureZoneManager.captureZoneManager.GetCurrentCaptureZone());
    }

    void Start()
    {
        if (CaptureZoneManager.captureZoneManager == null)
            Debug.Log("null");
    }

    void OnDestroy()
    {
        if ( distanceText != null)
            distanceText.gameObject.SetActive(false);
    }

	void FixedUpdate()
    {
        if (distanceObject)
            distanceText.text = Mathf.Clamp(Vector3.Distance(distanceObject.transform.position, transform.position) - 5f, 0f, 1000f).ToString("0");
        else
            distanceText.gameObject.SetActive(false);
    }

    public void SetObject(GameObject other)
    {
        //distanceText.gameObject.SetActive(true);
        distanceObject = other;
    }*/
}
