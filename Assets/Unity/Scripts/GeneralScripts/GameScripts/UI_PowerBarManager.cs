using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_PowerBarManager: MonoBehaviour {
    [SerializeField]
    private Image bar;
    [SerializeField]
    private GameObject powerBar;

    PlayerMovementController playerMovement;


    public void setFill(float value)
    {
        bar.fillAmount = value;
    }

    public void setDisplay(bool state)
    {
        powerBar.SetActive(state);
    }

    void Update()
    {
        GameObject player = (GameObject)PhotonNetwork.player.TagObject;
        if (player != null)
        {
            if (playerMovement == null)
                playerMovement = player.GetComponent<PlayerMovementController>();

            setDisplay(playerMovement.isChargingMove);
            setFill(playerMovement.currentPercentage);
        } else
        {
            playerMovement = null;
            setDisplay(false);
        }
    }
}
