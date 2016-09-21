using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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
            gameModes.Add(GameMode.Bomb);

        networkRoomController.JoinOrCreateGame(gameModes.ToArray());
    }
}
