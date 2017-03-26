using UnityEngine;
using System.Collections;

[RequireComponent ( typeof(AudioSource)) ]
public class BackgroundMusicManager : MonoBehaviour {

    [SerializeField]
    string[] audioClips;

    AudioSource audioSource;

    int nextClip;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        nextClip = -1;
        if (audioClips.Length == 0)
        {
            Debug.Log("No audio clip attached");
            enabled = false;
        }
        else
        {
            StartCoroutine(PlayBackgroundMusic());
        }
    }

    int GetRandomMusicIndex(int previous)
    {
        int index;
        do
        {
            index = Random.Range(0, audioClips.Length);
        } while (previous == index);
        return index;
    }

    IEnumerator PlayBackgroundMusic()
    {
        while(true)
        {
            nextClip = GetRandomMusicIndex(nextClip);
            ResourceRequest musicRequest = Resources.LoadAsync<AudioClip>("backgroundMusic/" + audioClips[nextClip]);
            yield return new WaitWhile(() => audioSource.isPlaying);
            yield return new WaitUntil(() => musicRequest.isDone);
            audioSource.PlayOneShot((AudioClip)musicRequest.asset);
        }
    }
}
