using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu_PlayManager : MonoBehaviour {
    [SerializeField]
    Toggle captureToggle;

    [SerializeField]
    Toggle bombToggle;

    public void OnSearchButtonPressed()
    {
        List<GameMode> gameModes = new List<GameMode>();
        if (captureToggle.isOn)
            gameModes.Add(GameMode.Capture);

        if (bombToggle.isOn)
            gameModes.Add(GameMode.KingOfTheHill);
        
        if(gameModes.Count > 0)
        {
            GameLobbyManager.desiredGameModes = gameModes.ToArray();
            SceneManager.LoadScene("GameLobby");
        }
    }
}
