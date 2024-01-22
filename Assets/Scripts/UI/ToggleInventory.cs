using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleInventory : MonoBehaviour
{
    [SerializeField] GameObject panel;

    private void Start()
    {
        Inputs.Instance.Controls.Player.Tab.started += Toggle;
        panel.SetActive(false);

    }

    public void Toggle(InputAction.CallbackContext context)
    {
        if (panel.activeSelf)
            DragObject.Instance.UnSetDragedItem();
        panel.SetActive(!panel.activeSelf);
        UIController.UIActive = panel.activeSelf;
        //Debug.Log("UIActive set to "+UIController.UIActive);
    }
}
