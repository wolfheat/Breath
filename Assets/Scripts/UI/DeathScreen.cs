using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] UIController UIController;
    [SerializeField] GameObject panel;

    public void ShowScreen() => panel.SetActive(true);
    public void CloseClicked()
    {
        UIController.ResetPlayer();
        panel.SetActive(false);
    }
}
