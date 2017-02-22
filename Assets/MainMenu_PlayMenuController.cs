using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu_PlayMenuController : MonoBehaviour {

    [SerializeField]
    SQLMatchMaker networkRoomController;

    [SerializeField]
    Toggle captureToggle;

    [SerializeField]
    Toggle bombToggle;

    public void OnPlayButtonPressed()
    {
        List<GameMode> gameModes = new List<GameMode>();
        if (captureToggle.isOn)
            gameModes.Add(GameMode.Capture);

        if (bombToggle.isOn)
            gameModes.Add(GameMode.KingOfTheHill);
        
        if(gameModes.Count > 0)
        {
            GameLobyManager.desiredGameModes = gameModes.ToArray();
            GameLobyManager.playersInGame = 2; // TODO fer que sigui dinamic sengons el mode de joc
            SceneManager.LoadScene("GameLobby");
        }
            //networkRoomController.JoinOrCreateGame(gameModes.ToArray());
    }
}
