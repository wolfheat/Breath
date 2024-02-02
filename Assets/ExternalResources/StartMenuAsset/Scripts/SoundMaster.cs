using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Wolfheat.StartMenu
{
    public enum SoundName {MenuStep, MenuError, MenuClick, MenuOver, DropItem, Shoot, HUDPositive, HUDError,
        BulletImpact,
        Drowning,
        Crafting,
        CraftComplete,
        HitMetal,
        Drill,
        PickUp
    }
    public enum MusicName {MenuMusic, OutDoorMusic, IndoorMusic, DeadMusic}

[Serializable]
public class Music : BaseSound
{
    public MusicName name;
    public void SetSound(AudioSource source)
    {
        audioSource = source;
    }
}

[Serializable]
public class Sound: BaseSound
{ 
    public SoundName name;
    public void SetSound(AudioSource source)
    {
        audioSource = source;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;
    }
}
    
[Serializable]
public class BaseSound
    {
    public AudioClip clip;
    [Range(0f,1f)]
    public float volume;
    [Range(0.8f, 1.2f)]
    public float pitch=1f;
    public bool loop=false;
    [HideInInspector] public AudioSource audioSource;

}

public class SoundMaster : MonoBehaviour
{
    public static SoundMaster Instance { get; private set; }

    public AudioMixer mixer;
    public AudioMixerGroup masterGroup;  
    public AudioMixerGroup musicMixerGroup;  
    public AudioMixerGroup SFXMixerGroup;  
    [SerializeField] private Sound[] sounds;
    [SerializeField] private Music[] musics;
    [SerializeField]private AudioClip[] footstep;
    private Dictionary<SoundName,Sound> soundsDictionary = new();
    private Dictionary<MusicName,Music> musicDictionary = new();
    AudioSource musicSource;
    MusicName activeMusic;
    AudioSource stepSource;

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
            sound.audioSource.outputAudioMixerGroup = SFXMixerGroup;
            soundsDictionary.Add(sound.name, sound);
        }

        //Steps
        stepSource = gameObject.AddComponent<AudioSource>();
        stepSource.volume = 0.3f;

        // And Music
        musicSource = gameObject.AddComponent<AudioSource>();
        
        foreach (var music in musics)
        {
            // All music use same source (since only one will be playing at a time)
            music.SetSound(musicSource); 
            music.audioSource.outputAudioMixerGroup = musicMixerGroup;
            musicDictionary.Add(music.name, music);
        }

        // Play theme sound
        PlayMusic(MusicName.MenuMusic);
    }
    
    public void PlayMusic(MusicName name)
    {
        Debug.Log("Playing Music: "+name+" at:" + Time.realtimeSinceStartup);
        if (activeMusic == name)
        {
            Debug.Log("Trying to play music that is already playing");
            return;
        }
        if (musicDictionary.ContainsKey(name))
        {
            if (musicDictionary[name].audioSource.isPlaying && !musicDictionary[name].loop)
                return;
            musicSource.clip = musicDictionary[name].clip;
            musicSource.volume = musicDictionary[name].volume;
            musicSource.pitch = musicDictionary[name].pitch;
            musicSource.loop = musicDictionary[name].loop;
            musicSource.Play();
            activeMusic = name;
        }
        else
            Debug.LogWarning("No clip named "+name+" in dictionary.");

    }
    public void PlaySound(SoundName name, bool allowInterupt= false)
    {

        Debug.Log("Play Sound: "+name+" at:" + Time.realtimeSinceStartup);
        if (soundsDictionary.ContainsKey(name))
        {
            if (!allowInterupt && soundsDictionary[name].audioSource.isPlaying && !soundsDictionary[name].loop)
            {
                Debug.Log("Sound is playing and should not loop: "+name+" at:" + Time.realtimeSinceStartup);
                return;
            }
            Debug.Log("Start Sound: "+name);
            Debug.Log("soundsDictionary[name].audioSource.clip: " + soundsDictionary[name].audioSource.clip   );
            soundsDictionary[name].audioSource.Play();
        }
        else
            Debug.LogWarning("No clip named "+name+" in dictionary.");

    }

    public void FadeMusic(float time = 1f)
    {
        StartCoroutine(MusicFade(time));
    }
    public IEnumerator MusicFade(float time)
    {
        float changePerSecond = musicSource.volume / time;
        while (musicSource.volume > 0)
        {
            musicSource.volume -= changePerSecond * Time.deltaTime;
            yield return null;
        }
        musicSource.Stop();
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

    public void StopSound(SoundName name)
    {
        //Debug.Log("Play Sound: "+name+" at:" + Time.realtimeSinceStartup);
        if (soundsDictionary.ContainsKey(name))
        {
            soundsDictionary[name].audioSource.Stop();
        }
        else
            Debug.LogWarning("No clip named " + name + " in dictionary.");
        }

    public void ResumeMusic()
    {
        musicSource.Play();
    }

    public void PlayStepSound()
    {
        if (stepSource.isPlaying)
            return;
        if(footstep.Length>0)
            stepSource.PlayOneShot(footstep[Random.Range(0, footstep.Length)]);
    }
    }
}
