using System;
using System.Collections;
using System.Resources;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    ResourceManager resourceManager;
    public AudioMixer myMixer;
    public AudioSource musicSource, sfxSource;

    public float MasterValue { get; private set; }
    public float MusicValue { get; private set; }
    public float SFXValue { get; private set; }

    private IEnumerator Start()
    {
        yield return (new WaitUntil(() => ResourceManager.Instance != null));
        resourceManager = ResourceManager.Instance;

        resourceManager.LoadAllResources<AudioClip>("Audios\\Musics", "Music");
        resourceManager.LoadAllResources<AudioClip>("Audios\\Player\\Step", "Step");
        resourceManager.LoadResource<AudioClip>("Audios\\Player", "Die");
        resourceManager.LoadResource<AudioClip>("Audios\\Player", "Fire");


        PlayMusic("Music2");
    }

    public void PlayMusic(string name)
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            MasterValue = PlayerPrefs.GetFloat("MasterVolume");
            myMixer.SetFloat("master", Mathf.Log10(MasterValue) * 20);
        }
        else
        {
            MasterValue = 0f;
            myMixer.SetFloat("master", Mathf.Log10(MasterValue) * 20);
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            MusicValue = PlayerPrefs.GetFloat("MusicVolume");
            myMixer.SetFloat("music", Mathf.Log10(MusicValue) * 20);
        }
        else
        {
            MusicValue = 0.3f;
            myMixer.SetFloat("music", Mathf.Log10(MusicValue) * 20);
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SFXValue = PlayerPrefs.GetFloat("SFXVolume");
            myMixer.SetFloat("sfx", Mathf.Log10(PlayerPrefs.GetFloat("SFXValue")) * 20);
        }
        else
        {
            SFXValue = 0.3f;
            myMixer.SetFloat("sfx", Mathf.Log10(PlayerPrefs.GetFloat("SFXValue")) * 20);
        }


        AudioClip clip = resourceManager.GetResource<AudioClip>(name);
        if (clip == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name, bool isOneShot = true)
    {
        AudioClip clip = resourceManager.GetResource<AudioClip>(name);
        if (clip == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            if(isOneShot)
            {
                sfxSource.PlayOneShot(clip);
            }
            else
            {
                sfxSource.clip = clip;
                sfxSource.loop = false;
                sfxSource.Play();
            }
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
