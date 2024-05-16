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
        // Setting the Full Recipe Jagged Array
        allRecipes = new RecipeData[][] { allRecipesData.armorRecipes, allRecipesData.toolRecipes, allRecipesData.foodRecipes, allRecipesData.resourceRecipes };
        // All Ccreated Buttons are Placed in Separate array
        allRecipeButtons = new CraftButton[allRecipes.Length][];

        // Create Entire Menu Here
        for (int i = 0; i < allRecipes.Length; i++)
        {
            RecipeData[] recipeList = allRecipes[i];

            // Empty Recipe List?
            if (recipeList.Length == 0)
                continue;

            // Add main button
            var mainButton = Instantiate(mainButtonPrefab,mainButtonHolder.transform);

            // Set main button to image in first recipe
            mainButton.SetImage(recipeList[0].result.picture); // Use first recipe result image
            baseCraftingButtons.Add(mainButton);
            allRecipeButtons[i] = new CraftButton[recipeList.Length];

            // Add all sub buttons
            for (int j = 0; j < recipeList.Length; j++)
            {
                RecipeData recipe = recipeList[j];
                var subButton = Instantiate(subButtonPrefab, mainButton.subMenu.transform);
                subButton.SetData(recipe);
                allRecipeButtons[i][j]=subButton;
            }
        }

        for (int i = 0; i < baseCraftingButtons.Count; i++)
            baseCraftingButtons[i].ButtonID = i;

        // Close all menues to begin with
        OpenSubMenu(-1);
    }

    private void DisableSubmenu(int id) => baseCraftingButtons[id].ActivateSubMenu(false);

    public void OpenSubMenu(int id)
    {
        if (WaitingForMenuToClose && id != -1) return;

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
        // Enable Only the SubMenu by it's ID);
        for (int i = 0; i < baseCraftingButtons.Count; i++) {
            baseCraftingButtons[i].ActivateSubMenu(id==i);
            if (id == i)
            {
                // Bubble effect on all buttons in this sub menu
                foreach (var button in allRecipeButtons[id])
                    button.PlayBubbleAnimation();

            }
        }
    }

    public void Reset()
    {
        WaitingForRecipeToHide = false;
        WaitingForMenuToClose = false;

        // Close all menues
        OpenSubMenu(-1);
    }

    public void CraftItem(ItemData itemData)
    {
        if (!activeWorkbench)
        {
            // Cant create item, no active workbench
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
        // Update available Recipes
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
                }
                else
                {
                    // Can Not Afford
                    allRecipeButtons[i][j].SetAfford(false);
                }
            }
            baseCraftingButtons[i].SetAfford(hasCraftable);
        }
    }

    public void CloseCraftingMenu()
    {
        if (recipeInfo.IsActive)
        {
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
        // Hiding Recipe Info
        if (recipeInfo.panelOpen)
            recipeInfo.CloseMenu();
    }

    public void ShowInfo(RecipeData recipeData)
    {
        // Show Recipe Info
        if (WaitingForMenuToClose) return;
        recipeInfo.ShowRecipe(recipeData);
    }

    public void SetActiveWorkbench(Workbench workbench) => activeWorkbench = workbench;

    // ACTIONS
    public void RecipeHasBeenClosed()
    {
        WaitingForRecipeToHide = false;

        // Closing Entire Crafting Menu
        if (WaitingForMenuToClose)
        {
            Reset();
            toggle.HideMenu();
            return;
        }
    }

}
