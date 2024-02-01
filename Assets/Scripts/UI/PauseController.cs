using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] UIController UIController;
    [SerializeField] GameObject panel;

    public void ToMainMenu()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }
    public void SetActive(bool doSetActive)
    {
        panel.SetActive(doSetActive);
    }

    public void CloseClicked()
    {
        UIController.UnPause();
    }
}
