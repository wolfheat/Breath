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

    private void OnEnable()
    {
        UpdateSoundPercent();
    }

    public void UpdateSoundPercent()
    {
        // Update percent
        masterPercent.text = (master.value*100).ToString("F0");
        musicPercent.text = (music.value*100).ToString("F0");
        sfxPercent.text = (sfx.value*100).ToString("F0");
    }
    public void UpdateSound()
    {
        SoundMaster.Instance.UpdateVolume(master.value,music.value, sfx.value);

        UpdateSoundPercent();

    } 
    public void SFXSliderChange()
    {
        SoundMaster.Instance.PlaySound(SoundName.MenuClick);
    }
}
}
