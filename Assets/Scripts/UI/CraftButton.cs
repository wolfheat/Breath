using UnityEngine;

public class CraftButton : MonoBehaviour
{
    [SerializeField] GameObject subMenu;
    public int ButtonID { get; set; }

    public void Click()
    {
        if(subMenu)
            CraftingUI.Instance.EnableBaseCrafting(ButtonID);
        else
            Debug.Log("This button crafts!");
    }

    public void ActivateSubMenu(bool set)
    {
        subMenu.SetActive(set);
    }
}
