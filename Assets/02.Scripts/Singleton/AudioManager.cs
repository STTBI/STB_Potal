using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] musicSounds, sfxSounds;
    public AudioMixer myMixer;
    public AudioSource musicSource, sfxSource;

    public float MasterValue { get; private set; }
    public float MusicValue { get; private set; }
    public float SFXValue { get; private set; }

    private void Awake()
    {
        PlayMusic("BGM");
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


        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name, bool isOneShot = true)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            if(isOneShot)
            {
                sfxSource.PlayOneShot(s.clip);
            }
            else
            {
                sfxSource.clip = s.clip;
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
