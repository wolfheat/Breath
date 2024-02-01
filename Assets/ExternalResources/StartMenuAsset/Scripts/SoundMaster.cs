using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Wolfheat.StartMenu
{
    public enum SoundName {MenuStep, MenuError, MenuClick,
    MenuOver, MenuMusic
}

[Serializable]
public class Sound
{
    public SoundName name;
    public AudioClip clip;
    [Range(0f,1f)]
    public float volume;
    [Range(0.8f, 1.2f)]
    public float pitch=1f;
    public bool loop=false;
    [HideInInspector] public AudioSource audioSource;
    private AudioSource musicSource;

    public void SetSound(AudioSource source)
    {
        audioSource = source;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;            
        audioSource.loop = loop;       
    }
}

public class SoundMaster : MonoBehaviour
{
    public static SoundMaster Instance { get; private set; }

    public AudioMixer mixer;
    public AudioMixerGroup masterGroup;  
    public AudioMixerGroup musicMixerGroup;  
    public AudioMixerGroup SFXMixerGroup;  
    [SerializeField] private Sound[] sounds;
    [SerializeField] private Sound[] musics;
    private Dictionary<SoundName,Sound> soundsDictionary = new();

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        // Define all sounds
        foreach (var sound in sounds)
        {
            sound.SetSound(gameObject.AddComponent<AudioSource>());
            Debug.Log("Setting gruop to SFX "+ sound.audioSource.outputAudioMixerGroup);
            sound.audioSource.outputAudioMixerGroup = SFXMixerGroup;
            Debug.Log("Setting gruop to SFX "+ sound.audioSource.outputAudioMixerGroup);
            soundsDictionary.Add(sound.name, sound);
        }
        // And Music
        foreach (var sound in musics)
        {
            sound.SetSound(gameObject.AddComponent<AudioSource>());
            sound.audioSource.outputAudioMixerGroup = musicMixerGroup;
            soundsDictionary.Add(sound.name, sound);
        }

        // Play theme sound
        PlaySound(SoundName.MenuMusic);
    }

    public void PlaySound(SoundName name)
    {
        //Debug.Log("Play Sound: "+name+" at:" + Time.realtimeSinceStartup);
        if (soundsDictionary.ContainsKey(name))
        {
            if (soundsDictionary[name].audioSource.isPlaying && !soundsDictionary[name].loop)
                return;
            soundsDictionary[name].audioSource.Play();
        }
        else
            Debug.LogWarning("No clip named "+name+" in dictionary.");

    }

    public void UpdateVolume(float masterVolume, float musicVolume,float sfxVolume)
    {
        //Debug.Log("Changing volumes ["+ masterVolume + ","+musicVolume+","+sfxVolume+"]");
        
        // Convert to dB
        mixer.SetFloat("Volume", Mathf.Log10(masterVolume) * 20);

        //Set Music
        mixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        
        // Set SFX
        mixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);


    }
}
}
