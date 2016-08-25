using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DistanceTextUpdater : MonoBehaviour {
    
    Text distanceText;
    GameObject distanceObject;
    GameObject player;

    void Awake()
    {
        distanceText = GameObject.Find("Canvas").transform.Find("DistanceText").GetComponent<Text>();
        CaptureEvents.OnLocalPlayerSpawned += SetPlayer;
        CaptureEvents.OnLocalPlayerKilled += PlayerKilled;
        CaptureEvents.OnCaptureZoneCreated += SetObject;
        CaptureEvents.OnCaptureZoneDestroyed += ObjectKilled;
    }

    void OnDestroy()
    {
        CaptureEvents.OnLocalPlayerSpawned -= SetPlayer;
        CaptureEvents.OnLocalPlayerKilled -= PlayerKilled;
        CaptureEvents.OnCaptureZoneCreated -= SetObject;
        CaptureEvents.OnCaptureZoneDestroyed -= ObjectKilled;
    }

	void FixedUpdate()
    {
        if (IsVisible())
            UpdateText();
    }

    void UpdateText()
    {
        if ( IsVisible() )
            distanceText.text = Mathf.Clamp(Vector3.Distance(distanceObject.transform.position, player.transform.position) - 5f, 0f, 1000f).ToString("0");
    }

    public void SetObject(GameObject other)
    {
        distanceObject = other;
        UpdateText();
        SetVisible(true);
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
        UpdateText();
        SetVisible(true);
    }

    public void ObjectKilled(GameObject other)
    {
        if (other == distanceObject)
        {
            distanceObject = null;
            SetVisible(false);
        }   
    }    

    public void PlayerKilled(GameObject player)
    {
        if (this.player == player)
        {
            this.player = null;
            SetVisible(false);
        }
    }

    bool IsVisible()
    {
        return player != null && distanceObject != null;
    }

    void SetVisible(bool state)
    {
        if ( state && IsVisible() )
        {
            distanceText.gameObject.SetActive(true);
        }
        else
        {
            distanceText.gameObject.SetActive(false);
        }
    }
}
