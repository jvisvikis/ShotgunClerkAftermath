using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {get;private set;}
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioSource audio3dPrefab;
    [SerializeField] private AudioSource audio2dPrefab;
    [SerializeField] [Range(0,1)] private float volume3d;
    [SerializeField] [Range(0,1)] private float volume2d;
    [SerializeField] [Range(0,1)] private float backgroundVolume;

    private int voiceIdx;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

    }

    public void Play3DAudio(AudioClip audioClip, Transform spawnTransform)
    {   
        AudioSource audio = Instantiate(audio3dPrefab, spawnTransform.position, Quaternion.identity);
        audio.clip = audioClip;
        audio.volume = volume3d;
        audio.Play();
        float clipLength = audio.clip.length;
        Destroy(audio.gameObject, clipLength);
    }

    public void Play2DAudio(AudioClip audioClip, float volume, bool fadeIn, bool loopable)
    {
        AudioSource audio = Instantiate(audio2dPrefab, transform.position, Quaternion.identity);
        audio.clip = audioClip;
        audio.loop = loopable;
        audio.Play();
        if(!loopable) Destroy(audio, audioClip.length);        
        if(fadeIn) StartCoroutine(FadeIn(audio, volume, 3f));
    }

    public IEnumerator FadeIn(AudioSource audio, float maxVolume, float duration)
    {
        float timer = 0;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            audio.volume = maxVolume * (timer/duration);
            yield return null;
        }
    }

}
