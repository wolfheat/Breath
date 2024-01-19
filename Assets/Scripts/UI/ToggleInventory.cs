using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleInventory : MonoBehaviour
{
    [SerializeField] GameObject panel;

    private void Start()
    {
        Inputs.Instance.Controls.Player.Tab.started += Toggle;
    }

    public void Toggle(InputAction.CallbackContext context)
    {
        if (panel.activeSelf)
            DragObject.Instance.UnSetDragedItem();
        panel.SetActive(!panel.activeSelf); 
    }
}
