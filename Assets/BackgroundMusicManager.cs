using UnityEngine;
using System.Collections;

[RequireComponent ( typeof(AudioSource)) ]
public class BackgroundMusicManager : MonoBehaviour {

    [SerializeField]
    AudioClip[] audioClips;

    AudioSource audioSource;

    int nextClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioClips.Length == 0)
        {
            Debug.Log("No audio clip attached");
            enabled = false;
        }
        else
        {
            nextClip = Random.Range(0, audioClips.Length);
            PlayBackgroundMusic();
        }
    }

    void PlayBackgroundMusic()
    {
        audioSource.PlayOneShot(audioClips[nextClip]);
        Invoke("PlayBackgroundMusic", audioClips[nextClip].length);
        nextClip = (nextClip + 1) % audioClips.Length;
    }
}
