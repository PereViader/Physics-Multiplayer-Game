using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;


public class MainMenu_OptionsManager : MonoBehaviour {

    [SerializeField]
    AudioMixer masterMixer;

    [SerializeField]
    Slider masterSlider;

    [SerializeField]
    Slider musicSlider;

    [SerializeField]
    Slider SFXSlider;

    void Start()
    {
        float masterVolume, musicVolume, sfxVolume;

        if ( PlayerPrefs.HasKey("MasterVolume") ) // Player Prefs has audio set
        {
            masterVolume = PlayerPrefs.GetFloat("MasterVolume");
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume");

            masterMixer.SetFloat("MasterVolume", masterVolume);
            masterMixer.SetFloat("MusicVolume", musicVolume);
            masterMixer.SetFloat("SFXVolume", sfxVolume);
        } else // set audio values
        {
            masterMixer.GetFloat("MasterVolume", out masterVolume);
            masterMixer.GetFloat("MusicVolume", out musicVolume);
            masterMixer.GetFloat("SFXVolume", out sfxVolume);

            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        }
        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        SFXSlider.value = sfxVolume;
    }

    // Sliders
    public void OnMasterSliderChange(float volume)
    {
        masterMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void OnMusicSliderChange(float volume)
    {
        masterMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void OnSFXSliderChange(float volume)
    {
        masterMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }


}
