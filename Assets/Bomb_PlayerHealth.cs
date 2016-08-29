using UnityEngine;
using System.Collections;

public class Bomb_PlayerHealth : MonoBehaviour {

    [SerializeField]
    float countdownToExplode;
    [SerializeField]
    float lerpCountdown;

    float networkedCountdown;

    public void ChangeBombState()
    {
        enabled = !enabled;
    }

    void Update()
    {
        if ( PhotonNetwork.isMasterClient )
        {
            countdownToExplode = Mathf.Clamp(countdownToExplode - Time.deltaTime, 0, float.MaxValue);
            if (countdownToExplode == 0)
                PhotonNetwork.Destroy(gameObject);
        } else
        {
            countdownToExplode = Mathf.Lerp(countdownToExplode, networkedCountdown, lerpCountdown * Time.deltaTime);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo message)
    {
        if ( stream.isWriting )
        {
            stream.Serialize(ref countdownToExplode);
        }
        else {
            stream.Serialize(ref networkedCountdown);
        }
    }
}
