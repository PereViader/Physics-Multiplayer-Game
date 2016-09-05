using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour, ISetup , IEnd {

    void Awake()
    {
        Random.seed = ((int)System.DateTime.Now.Ticks) + ((int)Time.time) + Time.frameCount;
    }

    public void OnGameSetup()
    {
        foreach (ISetup component in GetComponents<ISetup>())
        {

            if ((Object)component != this)
                component.OnGameSetup();
        }
    }

    public void OnGameEnd()
    {
        foreach (IEnd component in GetComponents<IEnd>())
        {
            if ((Object)component != this)
                component.OnGameEnd();
        }

        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.LoadLevel("EndGameScene");
        }
    }
}
