using System;
using System.Collections;
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
        [SerializeField] GameObject muted;
        [SerializeField] TextMeshProUGUI masterPercent;
        [SerializeField] TextMeshProUGUI musicPercent;
        [SerializeField] TextMeshProUGUI sfxPercent;
        private bool listenForSliderValues = false;

        private SoundSettings settings = new SoundSettings();
        private void OnEnable()
        {
            listenForSliderValues = false;
            Debug.Log("Settings Controller enabled, read data from file");
            //Read data from file
            settings = SavingUtility.gameSettingsData.soundSettings;

            if (settings != null)
            {
                UpdateUISettingsPage(settings);
            }

            UpdateSoundPercent();
            StartCoroutine(EnableSliderListeners());
            SoundMaster.Instance.GlobalMuteChanged += MuteChanged;
        }


        private void OnDisable()
        {
            SoundMaster.Instance.GlobalMuteChanged -= MuteChanged;
        }
        private void MuteChanged()
        {
            Debug.Log("Recieved change in mute from soundmaster");
            muted.SetActive(!SavingUtility.gameSettingsData.soundSettings.GlobalMaster);
        }

        private void UpdateUISettingsPage(SoundSettings settings)
        {
            master.value = settings.MasterVolume;
            music.value = settings.MusicVolume;
            sfx.value = settings.SFXVolume;
            muted.SetActive(!settings.GlobalMaster);
        }

        private IEnumerator EnableSliderListeners()
        {
            yield return null;
            yield return null;
            listenForSliderValues = true;
        }

        public void UpdateSoundPercent()
        {
            Debug.Log("SETTINGSCONTROLLER - Update Sound percent, Music Slider value: " + music.value);

            // Update percent
            masterPercent.text = master.value <= SoundMaster.MuteBoundary ? "MUTED" : (master.value * 100).ToString("F0");
            musicPercent.text = music.value <= SoundMaster.MuteBoundary ? "MUTED":(music.value*100).ToString("F0");
            sfxPercent.text = sfx.value <= SoundMaster.MuteBoundary ? "MUTED" : (sfx.value*100).ToString("F0");
        }

        public void UpdateSound()
        {
            if (!listenForSliderValues)
            {
                Debug.Log("Slider value changed but disregarded");
                return;
            }
        
            Debug.Log("SETTINGSCONTROLLER - Slider value changed");
         

            Debug.Log("SAVINGUTILITY - update dound setting values");
            SavingUtility.gameSettingsData.SetSoundSettings(master.value, music.value, sfx.value);

            SoundMaster.Instance.UpdateVolume();
            UpdateSoundPercent();
        } 

        public void SFXSliderChange()
        {
            SoundMaster.Instance.PlaySound(SoundName.MenuClick);
        }

        public void UnMute()
        {
            Debug.Log("Request Unmute");
            settings.GlobalMaster = true;
            UpdateSound();
        }
    }
}
