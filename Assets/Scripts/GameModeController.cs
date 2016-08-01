using UnityEngine;
using System.Collections;

public class GameModeController : MonoBehaviour {

    [SerializeField]
    private NetworkRoomController networkRoomController;

    private int gameMode = 0;

    public void OnGameModeDropdownChanged(int newGameMode)
    {
        gameMode = newGameMode;
    }

    public void OnPlayButtonPressed()
    {
        Debug.Log("Play");
        networkRoomController.joinGameOrCreate(gameMode);
    }
}
