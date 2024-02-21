using UnityEngine;

public class ToggleHelp : MonoBehaviour
{
    [SerializeField] private GameObject panelToToggle;
    
    public void Toggle()
    {
        panelToToggle.SetActive(!panelToToggle.gameObject.activeSelf);
    }
}
