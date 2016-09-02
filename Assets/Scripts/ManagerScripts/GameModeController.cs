using UnityEngine;
using System.Collections;

public class GameModeController : MonoBehaviour {

    [SerializeField]
    private NetworkRoomController networkRoomController;

    [SerializeField]
    GameMode gameMode;

    public void OnGameModeDropdownChanged(int newGameMode)
    {
        switch (newGameMode)
        {
            case 0:
                gameMode = GameMode.Capture;
            break;
            case 1:
                gameMode = GameMode.Bomb;
                break;
            case 2:
                gameMode = GameMode.IA;
                break;
            default:
                Debug.Log("InvalidGameMode");
                break;
        }
    }

    public void OnPlayButtonPressed()
    {
        networkRoomController.joinGameOrCreate(gameMode);
    }
}
