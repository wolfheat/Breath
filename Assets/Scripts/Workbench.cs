using UnityEngine;

public class Workbench : Facility
{
    [SerializeField] private ToggleMenu craftingMenu;

    private void OnEnable()
    {
        craftingMenu = FindObjectOfType<CraftingUI>().GetComponent<ToggleMenu>();
    }
    public override void InteractWith()
    {
        Debug.Log("Interact with the workbench");
        if(!craftingMenu.IsActive)
            craftingMenu.Toggle();
    }
}
