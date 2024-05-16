using UnityEngine;

public class WinScreen : MonoBehaviour
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
