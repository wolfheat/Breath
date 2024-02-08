using System;
using System.Collections.Generic;
using UnityEngine;
public class CraftingUI : MonoBehaviour
{
    private List<CraftButton> baseCraftingButtons = new List<CraftButton>();
    [SerializeField] RecipeInfo recipeInfo;

    [SerializeField] CraftButton mainButtonPrefab;
    [SerializeField] GameObject mainButtonHolder;

    [SerializeField] CraftButton subButtonPrefab;
    [SerializeField] AllRecipesData allRecipesData;
    [SerializeField] ToggleMenu toggle;

    private Workbench activeWorkbench;

    private bool WaitingForRecipeToHide = false;
    private bool WaitingForMenuToClose = false;
    private int nextMenuToOpen = -1;

    public static CraftingUI Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        // Subscribe to Info Closing Event
        recipeInfo.CloseComplete += RecipeHasBeenClosed;
    }

    private void Start()
    {
        RecipeData[][] all = new RecipeData[][] { allRecipesData.armorRecipes, allRecipesData.toolRecipes, allRecipesData.foodRecipes, allRecipesData.resourceRecipes };
        // Create Entire Menu Here
        Debug.Log("Populating Crafting Menu. Menu has "+all.Length+" main otions.");
        foreach (var recipeList in all)
        {
            if (recipeList.Length == 0)
                continue;

            Debug.Log("  Adding Main option with "+recipeList.Length+" items");

            // Add main button
            var mainButton = Instantiate(mainButtonPrefab,mainButtonHolder.transform);
            // Set main button to image in first recipe
            mainButton.SetImage(recipeList[0].result.picture); // Use first recipe result image
            baseCraftingButtons.Add(mainButton);

            foreach (var recipe in recipeList)
            {
                var subButton = Instantiate(subButtonPrefab, mainButton.subMenu.transform);
                subButton.SetData(recipe);
            }
        }

        for (int i = 0; i < baseCraftingButtons.Count; i++)
        {
            baseCraftingButtons[i].ButtonID = i;
        }
        RequestBaseCrafting(-1);
    }

    private void DisableSubmenu(int id)
    {
        baseCraftingButtons[id].ActivateSubMenu(false);
    }

    public void RequestBaseCrafting(int id)
    {
        // Requesting action to show this subMenu
        // If any menu is open wait for it to close
        Debug.Log("Request base crafting "+id+ " WaitingForRecipeToHide: "+ WaitingForRecipeToHide);
        if (WaitingForRecipeToHide)
            nextMenuToOpen = id;
        else
            EnableOnlySubMenu(id); 
            
    }

    private void EnableOnlySubMenu(int id)
    {
        Debug.Log("Enable only sub menu: "+id);
        for (int i = 0; i < baseCraftingButtons.Count; i++) {
            baseCraftingButtons[i].ActivateSubMenu(id==i);
        }
    }

    public void Reset()
    {
        WaitingForRecipeToHide = false;
        WaitingForMenuToClose = false;
        RequestBaseCrafting(-1);
    }

    public void CraftItem(ItemData itemData)
    {
        if (!activeWorkbench)
        {
            Debug.LogWarning("Cant create item, no active workbench");
            HUDMessage.Instance.ShowMessage("No active workbench");
            return;
        }
        if (activeWorkbench.IsCrafting)
            return;

        CloseCraftingMenu();

        activeWorkbench.CraftItem(itemData);
    }

    public void ToggleCraftingMenu()
    {
        if (toggle.IsActive)
            CloseCraftingMenu();
        else
            toggle.Toggle();
    }
    public void CloseCraftingMenu()
    {
        if (recipeInfo.IsActive)
        {
            Debug.Log("Recipe Info is active");
            WaitingForMenuToClose = true;
            recipeInfo.CloseMenu();
        }
        else
        {
            WaitingForMenuToClose = false;
            Reset();
            toggle.HideMenu();
        }
    }

    public void HideInfoOnly()
    {
        Debug.Log("Hiding Recipe Info, panel active:"+recipeInfo.panelOpen);
        if (recipeInfo.panelOpen)
        {
            recipeInfo.CloseMenu();
        }
        else Debug.Log("Disregard mouse exit cause already closing menu");
    }
    public void ShowInfo(RecipeData recipeData)
    {
        recipeInfo.ShowRecipe(recipeData);
    }
    public void SetActiveWorkbench(Workbench workbench)
    {
         activeWorkbench = workbench;
    }

    // ACTIONS
    public void RecipeHasBeenClosed()
    {
        Debug.Log("Recipe has been closed");
        Debug.Log("WaitingForRecipeToHide: "+ WaitingForRecipeToHide+ " WaitingForMenuToClose: "+ WaitingForMenuToClose+ " nextMenuToOpen:"+ nextMenuToOpen);

        WaitingForRecipeToHide = false;
        if (WaitingForMenuToClose)
        {
            Reset();
            toggle.HideMenu(); 
        }

        if (nextMenuToOpen == -1)
            return;

        EnableOnlySubMenu(nextMenuToOpen);
        nextMenuToOpen = -1;
    }

}
