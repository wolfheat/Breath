using UnityEngine;
using UnityEngine.SceneManagement;
using Wolfheat.StartMenu;

public class PauseController : MonoBehaviour
{
    [SerializeField] UIController UIController;
    [SerializeField] GameObject panel;

    public void ToMainMenu()
    {

        Time.timeScale = 1f;
        // Save player data here
        if(SavingUtility.playerGameData == null)
        {
            Debug.LogWarning("Going to Main Menu, saving but playerGameData is null");
        }
        else
        {
            Debug.Log("** SAVING LEVEL **");
            LevelLoader.Instance.SetGameData();
            SavingUtility.Instance.SavePlayerDataToFile();
            
        }
        
        SoundMaster.Instance.ResetMusic();

        SceneManager.UnloadSceneAsync("MainScene");
        SceneChanger.Instance.ChangeScene("StartMenu");
    }
    public void SetActive(bool doSetActive)
    {
        Debug.Log("Setting pause menu active: "+doSetActive+" Savingutility.playerGameData: "+SavingUtility.playerGameData);
        panel.SetActive(doSetActive);
        
    }

    public void CloseClicked()
    {
        UIController.UnPause();
    }
}