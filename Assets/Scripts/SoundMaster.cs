using UnityEngine;
using UnityEngine.InputSystem;
public enum MusicTrack { Indoor, OutDoor };

public class SoundMaster : MonoBehaviour
{
    [SerializeField] private AudioClip[] menu;
    [SerializeField] private AudioClip[] sfx;
    [SerializeField] private AudioClip[] footstep;
    [SerializeField] private AudioClip[] music;

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private bool doPlayMusic = true;
    private bool doPlaySFX=true;

    private float presetVolume = 0.03f;
    private float presetSFXStepVolume = 0.3f;

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
        musicSourceHolder.transform.SetParent(this.transform);
        sfxSourceHolder.transform.SetParent(this.transform);

        musicSource = musicSourceHolder.AddComponent<AudioSource>();
        sfxSource = sfxSourceHolder.AddComponent<AudioSource>();
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
        PlayMusic();

        Inputs.Instance.Controls.Player.MusicToggle.performed += MuteToggle;// = _.ReadValue<float>();
	}

    private void MuteToggle(InputAction.CallbackContext context)
    {
		doPlayMusic = !doPlayMusic;
        Debug.Log("Music: "+doPlayMusic);
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


    public void StopStepSFX()
    {
		sfxSource.Stop();
	}

    public void PlayStepSFX()
    {
		sfxSource.PlayOneShot(footstep[Random.Range(0, footstep.Length)]);
	}

    public void StopSFX()
    {
        sfxSource.Stop();
    }


    public enum SFX { MenuStep, MenuSelect, MenuError, NoAir, ToolSwing, HitWood, HitMetal, HitPlastic, BreakObject, PickUp, PlayerDeath, Footstep }

    public void PlaySFX(SFX type, bool playMulti=true)
	{

        // If not able to play multiple sounds exit if already playing
        if (!playMulti) if (sfxSource.isPlaying) return;

        if (!doPlaySFX) return;


        switch (type)
		{
            case SFX.Footstep: 
                sfxSource.PlayOneShot(PlayRandomFromArray(footstep));
                break;
            case SFX.PickUp: 
                sfxSource.PlayOneShot(sfx[2]);
                break;
            case SFX.ToolSwing: 
                //sfxSource.PlayOneShot(PlayRandomFromArray(axe));
                break;
            case SFX.PlayerDeath: 
                break;
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

}
