using System;
using System.Collections.Generic;
using UnityEngine;
public class CraftingUI : MonoBehaviour
{
    private List<CraftButton> baseCraftingButtons = new List<CraftButton>();
    [SerializeField] RecipeInfo recipeInfo;

    [SerializeField] GameObject mainButtonHolder;
    [SerializeField] CraftButton mainButtonPrefab;  
    [SerializeField] CraftButton subButtonPrefab;
    [SerializeField] AllRecipesData allRecipesData;
    [SerializeField] ToggleMenu toggle;

    private Workbench activeWorkbench;

    private RecipeData[][] allRecipes;
    private CraftButton[][] allRecipeButtons;

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
    private void OnDisable()
    {
        recipeInfo.CloseComplete -= RecipeHasBeenClosed;        
    }

    private void Start()
    {
        allRecipes = new RecipeData[][] { allRecipesData.armorRecipes, allRecipesData.toolRecipes, allRecipesData.foodRecipes, allRecipesData.resourceRecipes };
        allRecipeButtons = new CraftButton[allRecipes.Length][];
        // Create Entire Menu Here
        Debug.Log("Populating Crafting Menu. Menu has "+allRecipes.Length+" main otions.");
        for (int i = 0; i < allRecipes.Length; i++)
        {
            RecipeData[] recipeList = allRecipes[i];

            if (recipeList.Length == 0)
                continue;

            Debug.Log("  Adding Main option with "+recipeList.Length+" items");

            // Add main button
            var mainButton = Instantiate(mainButtonPrefab,mainButtonHolder.transform);
            // Set main button to image in first recipe
            mainButton.SetImage(recipeList[0].result.picture); // Use first recipe result image
            baseCraftingButtons.Add(mainButton);
            allRecipeButtons[i] = new CraftButton[recipeList.Length];
            for (int j = 0; j < recipeList.Length; j++)
            {
                RecipeData recipe = recipeList[j];
                var subButton = Instantiate(subButtonPrefab, mainButton.subMenu.transform);
                subButton.SetData(recipe);
                allRecipeButtons[i][j]=subButton;
            }
        }

        for (int i = 0; i < baseCraftingButtons.Count; i++)
        {
            baseCraftingButtons[i].ButtonID = i;
        }
        OpenSubMenu(-1);
    }

    private void DisableSubmenu(int id)
    {
        baseCraftingButtons[id].ActivateSubMenu(false);
    }

    public void OpenSubMenu(int id)
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
            if (id == i)
            {
                Debug.Log("Bubble all these buttons in menu "+id);
                foreach (var button in allRecipeButtons[id])
                    button.PlayBubbleAnimation();

            }
        }
    }

    public void Reset()
    {
        Debug.Log("RESET");
        WaitingForRecipeToHide = false;
        WaitingForMenuToClose = false;
        OpenSubMenu(-1);
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
        {
            toggle.Toggle();
            UpdateAvailableRecipes();

        }
    }

    private void UpdateAvailableRecipes()
    {
        Debug.Log("Update available Recipes");
        for (int i = 0; i < baseCraftingButtons.Count; i++)
        {
            RecipeData[] recipeList = allRecipes[i];
            bool hasCraftable = false;
            for (int j = 0; j < recipeList.Length; j++)
            {
                RecipeData recipe = recipeList[j];
                if (Inventory.Instance.CanAfford(recipe))
                {
                    allRecipeButtons[i][j].SetAfford(true);
                    hasCraftable = true;
                        //Debug.Log("Can afford" + recipe.result.itemName);
                }
                else
                {
                    allRecipeButtons[i][j].SetAfford(false);
                    Debug.Log("Can't afford " + recipe.result.itemName);
                }
            }
            baseCraftingButtons[i].SetAfford(hasCraftable);
        }
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
        Debug.Log("CRAFTING UI - Hiding Recipe Info, panel active:"+recipeInfo.panelOpen);
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

        // Closing Entire Crafting Menu
        if (WaitingForMenuToClose)
        {
            Debug.Log("Waiting for menu to Close, reset");
            Reset();
            toggle.HideMenu();
            return;
        }
    }

}
