﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Wolfheat.StartMenu;

public class CraftButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject subMenu;
    [SerializeField] RecipeData recipeData;
    [SerializeField] Material grayscaleMaterial;
    [SerializeField] GameObject highLight;
    [SerializeField] Animator animator;

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
        PlayBubbleAnimation();

        if (subMenu)
        {
            // If Main Button - Open the submenu            
            CraftingUI.Instance.OpenSubMenu(ButtonID);
            SoundMaster.Instance.PlaySound(SoundName.MenuStep);
            return;
        }

        // Clicking on a recipe
        bool afford = Inventory.Instance.CanAfford(recipeData);
        if (afford)
        {
            // Player can afford this recipe - Remove resource requirements from player and start producing the item
            CraftingUI.Instance.CraftItem(recipeData.result);
            Inventory.Instance.RemoveItems(recipeData.ingredienses);
        }
        else
            HUDMessage.Instance.ShowMessage("Insufficent resources");
    }

    public void ActivateSubMenu(bool set) => subMenu.SetActive(set);

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!subMenu)
        {
            // Mouse Entering a Recipe
            CraftingUI.Instance.ShowInfo(recipeData);
        }
        // HighLight button
        HighLight(true);
    }

    private void HighLight(bool v) => highLight.SetActive(v);

    public void OnPointerExit(PointerEventData eventData)
    {
        // Mouse Exiting a Recipe
        if (!subMenu)
            CraftingUI.Instance.HideInfoOnly();

        HighLight(false);
    }

    public void SetAfford(bool afford)
    {
        image.material = afford ? null : grayscaleMaterial;
        imageBackground.color = afford ? greenColor : Color.white;
    }

    public void PlayBubbleAnimation() => animator.CrossFade("BubbleTransit", 0.1f);
}
