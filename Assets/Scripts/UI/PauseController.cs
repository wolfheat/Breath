using UnityEngine;
using UnityEngine.SceneManagement;
using Wolfheat.StartMenu;

public class PauseController : MonoBehaviour
{
    [SerializeField] UIController UIController;
    [SerializeField] GameObject panel;

    public void ToMainMenu()
    {
        // Save player data when going to menu
        if(SavingUtility.playerGameData == null)
            Debug.LogWarning("Going to Main Menu, saving but playerGameData is null");
        else
        {
            // Saving Level
            LevelLoader.Instance.DefineGameDataForSave();
            SavingUtility.Instance.SavePlayerDataToFile();
        }
        
        // Stops drowning sound from playing and resets the music
        SoundMaster.Instance.ResetMusic();

        // Scene change
        SceneManager.UnloadSceneAsync("MainScene");
        SceneChanger.Instance.ChangeScene("StartMenu");
    }

    public void SetActive(bool doSetActive) => panel.SetActive(doSetActive);

    public void CloseClicked()
    {
        // Unpause and close menu
        UIController.Pause(false);
        SetActive(false);
    }
}
