using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
public enum MusicTrack { Indoor, OutDoor };

public class SoundMaster : MonoBehaviour
{
    [SerializeField] private AudioClip[] menu;
    [SerializeField] private AudioClip[] sfx;
    [SerializeField] private AudioClip[] hud;
    [SerializeField] private AudioClip[] tools;
    [SerializeField] private AudioClip[] footstep;
    [SerializeField] private AudioClip[] music;
    [SerializeField] private AudioClip[] noises;
    [SerializeField] private AudioMixerGroup masterMixer;

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private AudioSource stepSource;
    private bool doPlayMusic = false;
    private bool doPlaySFX=true;

    private float presetVolume = 0.1f;
    private float presetSFXStepVolume = 0.1f;

    private float totalFadeOutTime = 3.5f;
    private float fadeOutMargin = 0.01f;
    private float currentFadeOutTime;

    public static SoundMaster Instance;
    MusicTrack activeMusicTrack = MusicTrack.OutDoor;

    private void Awake()
    {   
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;


        GameObject musicSourceHolder = new GameObject("Music");
        GameObject sfxSourceHolder = new GameObject("SFX");
        GameObject stepSourceHolder = new GameObject("Steps");
        musicSourceHolder.transform.SetParent(this.transform);
        sfxSourceHolder.transform.SetParent(this.transform);
        stepSourceHolder.transform.SetParent(this.transform);

        musicSource = musicSourceHolder.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = masterMixer;
        sfxSource = sfxSourceHolder.AddComponent<AudioSource>();
        sfxSource.outputAudioMixerGroup = masterMixer;
        stepSource = stepSourceHolder.AddComponent<AudioSource>();
        stepSource.outputAudioMixerGroup = masterMixer;

    }
    private void OnDestroy()
    {
        Inputs.Instance.Controls.Player.MusicToggle.performed -= MuteToggle;// = _.ReadValue<float>();
    }
    private void Start()
    {
        musicSource.loop = true;
        musicSource.volume = presetVolume;
        sfxSource.volume = presetSFXStepVolume;
        stepSource.volume = presetSFXStepVolume;
        sfxSource.loop = false;
        PlayMusic();

        Inputs.Instance.Controls.Player.MusicToggle.performed += MuteToggle;// = _.ReadValue<float>();
	}

    private void MuteToggle(InputAction.CallbackContext context)
    {
		doPlayMusic = !doPlayMusic;
        Debug.Log("Music Enabled: "+doPlayMusic);
		if (doPlayMusic) PlayMusic();
		else
		{
			musicSource.Stop();
		}
	}

	public void SetVolume(float vol)
	{
        musicSource.volume = vol;
	}
	public void SetSFXVolume(float vol)
	{
        sfxSource.volume = vol;
        sfxSource.volume = presetSFXStepVolume;
        stepSource.volume = presetSFXStepVolume;
	}
    public void ChangeMusicTrack(MusicTrack track = MusicTrack.OutDoor)
    {        
        if(activeMusicTrack != track)
        {
                activeMusicTrack = track;
                PlayMusic();
        }
    }
	private void PlayMusic()
	{
        if (doPlayMusic)
        {
            if (music.Length == 0 || musicSource == null) return;

            //float percentagePlayed = musicSource.timeSamples / musicSource.clip.samples * 100f;
            musicSource.clip = music[(int)activeMusicTrack];
            musicSource.Play();
            musicSource.volume = presetVolume;
        }
        else musicSource.Stop(); 
	}

    public void PlayStepSFX()
    {
        if (stepSource.isPlaying)
            return;
        stepSource.PlayOneShot(footstep[Random.Range(0, footstep.Length)]);
	}

    public void StopSFX()
    {
        sfxSource.Stop();
    }
    

    public enum SFX { MenuStep, MenuSelect, MenuError, HUDError, CraftComplete, Crafting, CraftingB, NoAir, ToolSwing, HitWood, 
        HitMetal, HitPlastic, Drill, SwordHit, BreakObject, PickUp, PlayerDeath, Footstep, Drowning, HUDPositive, ConsumeHealth, DropItem,
        Shoot, BulletImpact
    }

    public void PlaySFX(SFX type, bool playMulti=true)
	{
        Debug.Log("PLAY SFX CALLED");
        // If not able to play multiple sounds exit if already playing
        if (!playMulti) if (sfxSource.isPlaying) return;

        if (!doPlaySFX) return;


        switch (type)
		{
            // USED 
            case SFX.Drowning: 
                sfxSource.PlayOneShot(noises[0]);
                break;

            // TOOLS
            case SFX.ToolSwing:
                sfxSource.PlayOneShot(tools[0]);
                break;
            case SFX.HitMetal:
                sfxSource.PlayOneShot(tools[1]);
                break;
            case SFX.Drill: 
                sfxSource.PlayOneShot(tools[2]);
                break;
            case SFX.SwordHit: 
                sfxSource.PlayOneShot(tools[3]);
                break;

            case SFX.HUDError:
                sfxSource.PlayOneShot(hud[0]);
                break;
            case SFX.HUDPositive:
                sfxSource.PlayOneShot(hud[1]);
                break;
            case SFX.PickUp:
                sfxSource.PlayOneShot(sfx[0]);
                break;
            case SFX.Shoot: 
                sfxSource.PlayOneShot(sfx[1]);
                break;
            case SFX.Crafting:                
                sfxSource.PlayOneShot(sfx[2]);
                break;
            case SFX.CraftComplete: 
                sfxSource.PlayOneShot(sfx[3]);
                break;
            case SFX.BulletImpact:                
                sfxSource.PlayOneShot(sfx[4]);
                break;
            case SFX.DropItem:                
                sfxSource.PlayOneShot(sfx[5]);
                break;

            // --- OLD STUFF
            case SFX.PlayerDeath: 
                break;
            // --- OLD STUFF

            case SFX.MenuStep:
                sfxSource.PlayOneShot(menu[0]);
                break;
			case SFX.MenuSelect:
                sfxSource.PlayOneShot(menu[1]);
                break;
			case SFX.MenuError:
                sfxSource.PlayOneShot(menu[2]);
                break;
			default:
				break;
		}
	}

    private AudioClip PlayRandomFromArray(AudioClip[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

    public bool FootstepPlaying()
    {
        throw new System.NotImplementedException();
    }

    public void ResumeMusic()
    {
        PlayMusic();
    }
    public void FadeMusic(float time=1f)
    {
        StartCoroutine(MusicFade(time));
    }
    public IEnumerator MusicFade(float time)
    {
        float changePerSecond = musicSource.volume / time;
        while (musicSource.volume > 0)
        {
            musicSource.volume -= changePerSecond*Time.deltaTime;
            yield return null;
        }
        musicSource.volume = presetVolume;
        musicSource.Stop();

    }
}
