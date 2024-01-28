using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject subMenu;
    [SerializeField] RecipeData recipeData;
    public Image image;
    public int ButtonID { get; set; }


    public void SetData(RecipeData data)
    {
        recipeData = data;
        SetImage(data.result.picture);
    }
    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
    public void Click()
    {
        if (subMenu) return;


        bool afford = Inventory.Instance.CanAfford(recipeData);
        if (afford)
        {
            Debug.Log("Player can afford Remove requirements and start producing item");
            CraftingUI.Instance.CraftItem(recipeData.result);
            Inventory.Instance.RemoveItems(recipeData.ingredienses);

        }
        else
        {
            HUDMessage.Instance.ShowMessage("Insufficent resources");
        }
        
    }

    public void ActivateSubMenu(bool set)
    {
        subMenu.SetActive(set);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!subMenu)
        {
            CraftingUI.Instance.ShowInfo(recipeData);
        }
        else
        {
            //Open submenu
            CraftingUI.Instance.EnableBaseCrafting(ButtonID);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {        
        if (!subMenu)
        {
            CraftingUI.Instance.HideInfoOnly();
        }
    }
}
