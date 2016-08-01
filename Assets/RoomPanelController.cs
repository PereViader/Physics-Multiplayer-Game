using UnityEngine;
using System.Collections;

public class RoomPanelController : MonoBehaviour {

    [SerializeField]
    private GameObject roomPanel;

    [SerializeField]
    private GameObject searchingText;

    [SerializeField]
    private GameObject waitingText;

    public void HideRoomPanel()
    {
        roomPanel.SetActive(false);
    }

    public void DisplayWaitingText()
    {
        roomPanel.SetActive(true);
        searchingText.SetActive(false);
        waitingText.SetActive(true);
    }

    public void DisplaySearchingText()
    {
        roomPanel.SetActive(true);
        searchingText.SetActive(true);
        waitingText.SetActive(false);
    }
}
