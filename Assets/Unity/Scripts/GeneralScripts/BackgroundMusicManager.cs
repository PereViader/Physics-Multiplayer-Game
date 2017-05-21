using UnityEngine;
using System.Collections;

[RequireComponent ( typeof(AudioSource)) ]
public class BackgroundMusicManager : MonoBehaviour {

    [SerializeField]
    private string[] audioClips;

    private AudioSource audioSource;

    private int nextClip;

    private static GameObject instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);


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
