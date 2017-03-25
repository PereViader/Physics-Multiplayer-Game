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
            Debug.Log("Waiting until music request finishes");
            yield return new WaitUntil(() => musicRequest.isDone);
            Debug.Log("Music request finished");
            AudioClip audioClip = (AudioClip)musicRequest.asset;
            Debug.Log("Waiting until audio ends reproducing");
            yield return new WaitWhile(() => audioSource.isPlaying);
            Debug.Log("Audio ended reproducing");
            audioSource.PlayOneShot(audioClip);
            yield return new WaitForSeconds(audioClip.length-10);
        }
    }
}
