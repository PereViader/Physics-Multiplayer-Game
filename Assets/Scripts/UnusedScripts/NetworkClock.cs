using UnityEngine;
using System.Collections;

public class NetworkClock : MonoBehaviour {

    [SerializeField]
    private float networkTickLenght;
    [SerializeField]
    private float networkSmoothingFactor;
    [SerializeField]
    private float syncThreshold;

    private long networkTick = 0;
    private float currentNetworkTickTime = 0;

    private float avarageClockDifference;
    private float receivedNetworkTickTime;
    private float estimatedNetworkTickTime;
    private int receivedPing;


    void Awake()
    {
        if (PhotonNetwork.isMasterClient)
            avarageClockDifference = 0;
        else
            avarageClockDifference = 500;
    }

    void Update()
    {
        currentNetworkTickTime += Time.deltaTime;
        long calculatedNetworkTick = CalculateNetworkTick(currentNetworkTickTime);
        if (calculatedNetworkTick != networkTick )
        {
            networkTick = calculatedNetworkTick;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo message)
    {
        if ( stream.isWriting)
        {
            stream.SendNext(currentNetworkTickTime);
            stream.SendNext(PhotonNetwork.GetPing());
        } else
        {
            receivedNetworkTickTime = (float)stream.ReceiveNext();
            receivedPing = (int)stream.ReceiveNext();

            estimatedNetworkTickTime = receivedNetworkTickTime + (receivedPing / 1000f);
            avarageClockDifference = (estimatedNetworkTickTime - currentNetworkTickTime + avarageClockDifference) / 2f;

            currentNetworkTickTime = currentNetworkTickTime + (estimatedNetworkTickTime - currentNetworkTickTime) / networkSmoothingFactor;
            long calculatedNetworkTick = CalculateNetworkTick(currentNetworkTickTime);
            if(calculatedNetworkTick != networkTick)
            {
                networkTick = calculatedNetworkTick;
            }
        }
    }

    private long CalculateNetworkTick(float networkTickTime)
    {
        return (long)(currentNetworkTickTime / networkTickLenght);
    }

    public long getNetworkTick()
    {
        return networkTick;
    }

    public float getAvarageClockDifference()
    {
        return avarageClockDifference;
    }

    public bool isClockSynced()
    {
        return Mathf.Abs(avarageClockDifference) < syncThreshold;
    }
}
