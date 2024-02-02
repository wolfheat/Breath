using UnityEngine;
using UnityEngine.SceneManagement;

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
            Debug.Log("Going to Main Menu");
            Debug.Log("Setting playerPosition: "+ PlayerStats.Instance.transform.position.x);
            SavingUtility.playerGameData.PlayerPosition = PlayerStats.Instance.transform.position.x;
        
            Debug.Log("Calling Save data to file");
            SavingUtility.Instance.SavePlayerDataToFile();
        }
        SceneManager.LoadSceneAsync("StartMenu",LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("MainScene");
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
