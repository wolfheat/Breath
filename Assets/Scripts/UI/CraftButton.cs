using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject subMenu;
    [SerializeField] RecipeData recipeData;
    [SerializeField] Material grayscaleMaterial;

    //"93FF86"
    private Color greenColor = new Color32(0x93, 0xE7, 0x86, 0xFF);
    public Image image;
    public Image imageBackground;
    public int ButtonID { get; set; }


    public void SetData(RecipeData data)
    {
        recipeData = data;
        SetImage(data.result.picture);
    }
    public void SetImage(Sprite sprite, bool afford = false)
    {
        image.sprite = sprite;
        SetAfford(afford);
        
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
            // interrupt closing of menu here?
            CraftingUI.Instance.ShowInfo(recipeData);
        }
        else
        {
            //Open submenu            
            CraftingUI.Instance.RequestBaseCrafting(ButtonID);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!subMenu)
        {
            CraftingUI.Instance.HideInfoOnly();
        }
        //else
            //CraftingUI.Instance.Reset();
    }

    public void SetAfford(bool afford)
    {
        image.material = afford ? null : grayscaleMaterial;
        imageBackground.color = afford ? greenColor : Color.white;
    }
}
