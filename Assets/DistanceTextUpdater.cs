using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DistanceTextUpdater : MonoBehaviour {

    Text distanceText;
    Transform distanceObject;


    void Awake()
    {
        distanceText = GameObject.Find("Canvas").transform.Find("DistanceText").GetComponent<Text>();
        distanceText.gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        distanceText.gameObject.SetActive(false);
    }

	void FixedUpdate()
    {
        if (distanceObject)
            distanceText.text = Vector3.Distance(transform.position, distanceObject.position).ToString("0");
        else
            distanceText.gameObject.SetActive(false);
    }

    public void SetObject(GameObject other)
    {
        distanceObject = other.transform;
    }
}
