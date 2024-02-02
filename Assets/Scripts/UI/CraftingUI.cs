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
        EnableBaseCrafting(-1);
    }

    public void EnableBaseCrafting(int id)
    {
        for (int i = 0;i < baseCraftingButtons.Count;i++)
        {
            baseCraftingButtons[i].ActivateSubMenu(i==id);
        }
    }

    public void Reset()
    {
        WaitingForRecipeToHide = false;
        EnableBaseCrafting(-1);
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
            WaitingForRecipeToHide = true;
            recipeInfo.CloseMenu();    
        }else
            toggle.HideMenu();
    }

    public void HideInfoOnly()
    {
        Debug.Log("Hiding Recipe Info, panel active:"+recipeInfo.panelOpen);
        if (recipeInfo.panelOpen)
        {

            WaitingForRecipeToHide=false;
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

        EnableBaseCrafting(-1);

        if (WaitingForRecipeToHide)
            toggle.HideMenu();
    }

}
