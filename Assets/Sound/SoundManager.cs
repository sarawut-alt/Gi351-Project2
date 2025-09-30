using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSourcePrefab; 

    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> musicClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> sfxClips = new List<AudioClip>();

    private Dictionary<string, AudioClip> musicDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadClips();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadClips()
    {
        foreach (var clip in musicClips)
            if (!musicDict.ContainsKey(clip.name))
                musicDict.Add(clip.name, clip);

        foreach (var clip in sfxClips)
            if (!sfxDict.ContainsKey(clip.name))
                sfxDict.Add(clip.name, clip);
    }

    

    public void PlayMusic(string clipName, bool loop = true)
    {
        if (musicDict.TryGetValue(clipName, out AudioClip clip))
        {
            if (musicSource.clip == clip && musicSource.isPlaying) return; 
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Music clip '{clipName}' not found!");
        }
    }

    public void StopMusic() => musicSource.Stop();

    public void FadeMusic(float targetVolume, float duration)
    {
        StartCoroutine(FadeAudio(musicSource, targetVolume, duration));
    }

    

    public void PlaySFX(string clipName, float volume = 1f, float pitch = 1f)
    {
        if (sfxDict.TryGetValue(clipName, out AudioClip clip))
        {
            AudioSource tempSource = Instantiate(sfxSourcePrefab, transform);
            tempSource.clip = clip;
            tempSource.volume = Mathf.Clamp01(volume);
            tempSource.pitch = pitch;
            tempSource.Play();
            Destroy(tempSource.gameObject, clip.length / pitch); // cleanup
        }
        else
        {
            Debug.LogWarning($"SFX clip '{clipName}' not found!");
        }
    }

    

    public void SetMusicVolume(float volume) => musicSource.volume = Mathf.Clamp01(volume);
    public void SetSFXVolume(float volume)
    {
        sfxSourcePrefab.volume = Mathf.Clamp01(volume);
    }

    

    private System.Collections.IEnumerator FadeAudio(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = source.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }

        source.volume = targetVolume;
    }
}
