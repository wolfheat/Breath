using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Wolfheat.StartMenu
{
    public class SettingsController : MonoBehaviour
{
    [SerializeField] Slider master;
    [SerializeField] Slider music;
    [SerializeField] Slider sfx;
    [SerializeField] TextMeshProUGUI masterPercent;
    [SerializeField] TextMeshProUGUI musicPercent;
    [SerializeField] TextMeshProUGUI sfxPercent;
    private bool listenForSliderValues = false;
    private void OnEnable()
    {
        Debug.Log("Settings COntroller enabled, read data from file");
        //Read data from file
        SoundSettings settings = SavingUtility.gameSettingsData.soundSettings;
        if (settings != null)
        {
            master.value = settings.MasterVolume;
            music.value = settings.MusicVolume;
            sfx.value = settings.SFXVolume;
            if (!settings.UseMusic)
                    music.value = 0.0001f; 
        }

        UpdateSoundPercent();
    }

    private void Start()
    {
        listenForSliderValues = true;        
    }

    public void UpdateSoundPercent()
    {
        Debug.Log("Update Sound percent");
        // Update percent
        masterPercent.text = (master.value*100).ToString("F0");
        musicPercent.text = (music.value*100).ToString("F0");
        if(music.value<0.01f)
        musicPercent.text = "MUTED";

        sfxPercent.text = (sfx.value*100).ToString("F0");
    }
    public void UpdateSound()
    {
        if (!listenForSliderValues)
        {
            Debug.Log("Slider value changed but disregarded");
            return;
        }
        

        SoundMaster.Instance.UpdateVolume(master.value,music.value, sfx.value);
        UpdateSoundPercent();
        SavingUtility.gameSettingsData.SetSoundSettings(master.value, music.value, sfx.value);


    } 
    public void SFXSliderChange()
    {
        SoundMaster.Instance.PlaySound(SoundName.MenuClick);
    }
}
}
