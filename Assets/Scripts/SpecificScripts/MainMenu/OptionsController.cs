using UnityEngine;
using System.Collections;

public class OptionsController : MonoBehaviour {

    private bool reproduceMusic = true;

    private bool reproduceSFX = true;

    public void OnMusicTogglePushed()
    {
        reproduceMusic = !reproduceMusic;
    }

    public void OnSFXTogglePushed()
    {
        reproduceSFX = !reproduceSFX;
    }
}
