using UnityEngine;

public class ToggleMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;

    public bool IsActive { get { return panel.activeSelf; }}

    private void Start()
    {
        panel.SetActive(false);

    }

    public void Toggle()
    {
        if (panel.activeSelf)
            DragObject.Instance.UnSetDragedItem();
        panel.SetActive(!panel.activeSelf);
        //Debug.Log("UIActive set to "+UIController.UIActive);
    }
}
