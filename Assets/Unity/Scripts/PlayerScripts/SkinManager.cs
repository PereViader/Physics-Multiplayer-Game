using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour {

    [SerializeField]
    private Text playerNameText;

    [SerializeField]
    private Image playerTeam;

    [SerializeField]
    private Color team0Color;

    [SerializeField]
    private Color team1Color;

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        int playerID = (int)GetComponent<PhotonView>().photonView.instantiationData[0];
        string playerTexture = "DefaultMaterial";
        PhotonPlayer player = PhotonPlayer.Find(playerID);
        if ( player != null )
        {
            if (player.customProperties[PlayerProperties.skin] != null)
            {
                playerTexture = (string)player.customProperties[PlayerProperties.skin];
                GetComponent<MeshRenderer>().material = (Material)Resources.Load("PlayerTextures/" + playerTexture);
            }
            playerNameText.text = player.name;

            if ( player.customProperties.ContainsKey(PlayerProperties.team))
            {
                setTeamVisuals((int)player.customProperties[PlayerProperties.team]);
            }
            //transform.GetComponentInChildren<Text>().text = player.name;
        }
    }

    void setTeamVisuals(int team)
    {
        if (team == 0)
            playerTeam.color = team0Color;
        else if (team == 1)
            playerTeam.color = team1Color;
    }
}
