using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeInfo : MonoBehaviour
{
    [SerializeField] RecipeItem recipeItemPrefab;
    [SerializeField] TextMeshProUGUI recipeName;
    [SerializeField] TextMeshProUGUI recipeInfo;
    [SerializeField] GameObject ingredienceHolder;
    [SerializeField] GameObject panel;
    [SerializeField] RecipeItem recipeResult;
    [SerializeField] Animator animator;
    [SerializeField] Inventory inventory;

    public bool IsActive { get { return panel.activeSelf; } }
    public bool panelOpen = false;

    private List<RecipeItem> recipeItems = new();
    private RecipeData activeRecipe;

    public Action CloseComplete;

    public void CloseMenu() => MakeVisible(false);

    public void ShowRecipe(RecipeData data)
    {
        // Display this recipe
        activeRecipe = data;
        recipeName.text = data.recipeName;
        recipeInfo.text = data.recipeInfo;

        // Check if player can produce this
        bool canProduceItem = PlaceIngrediences();

        // Update the info
        recipeResult.OccupyByData(activeRecipe.result,canProduceItem);

        // Show this panel
        MakeVisible(true);
    }

    private void MakeVisible(bool doMakeVisible)
    {
        // Define panel to open or closed and queue up the transition to this state
        panelOpen = doMakeVisible;
        storedCrossfades.Enqueue(doMakeVisible ? "MakeVisible" : "MakeInVisible");
    }

    private void Update()
    {
        // If there is queued transitions do next transition
        if (storedCrossfades.Count>0) 
            animator.CrossFade(storedCrossfades.Dequeue(), 0.1f);
    }

    Queue<string> storedCrossfades = new Queue<string>(); 

    private IEnumerator RunAnimatorDelay(string name)
    {
        yield return new WaitForFixedUpdate();        
        animator.CrossFade("MakeInVisible", 0.1f);
        animator.CrossFade("MakeVisible", 0.1f);
    }

    private bool PlaceIngrediences()
    {
        // Updates the Ingrediences requirements for the recipe
        bool canCreate = true;
        ClearItems();
        for (int i=0; i<activeRecipe.ingredienses.Length; i++) 
        {
            RecipeAmount ingredienceData = activeRecipe.ingredienses[i];
            RecipeItem item = GetNextRecipeItem(i);
            item.gameObject.SetActive(true);
            int playerHas = inventory.GetResources()[(int)ingredienceData.resourceData.resource];

            // Update info for the resource
            item.OccupyByData(ingredienceData.resourceData, ingredienceData.amount, playerHas);

            // Keep track if player affords all
            if(ingredienceData.amount>playerHas)
                canCreate = false;
        }
        // Return If player had all resources
        return canCreate;
    }

    private RecipeItem GetNextRecipeItem(int i)
    {
        // if there is to few items in the List add a new one
        if (i + 1 > recipeItems.Count)
        {
            RecipeItem item;
            item = Instantiate(recipeItemPrefab, ingredienceHolder.transform);            
            recipeItems.Add(item);
            return item;
        }
        return recipeItems[i];
    }

    private void ClearItems()
    {
        foreach (var item in recipeItems)
            item.gameObject.SetActive(false);
    }
    
    // ANIMATIONS
    public void AnimationComplete()
    {
        Debug.Log(" MakeInvisible Completed - Close Complete invoked from "+activeRecipe.result.itemName);
        CloseComplete?.Invoke();
    }

}
