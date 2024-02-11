using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

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

    private List<RecipeItem> recipeItems = new();

    public Action CloseComplete;
    public bool IsActive { get { return panel.activeSelf; } }

    public bool panelOpen = false;
    RecipeData activeRecipe;

    public void CloseMenu()
    {
        MakeVisible(false);
    }
    public void ShowRecipe(RecipeData data)
    {
        activeRecipe = data;
        // Display this recipe
        recipeName.text = data.recipeName;
        recipeInfo.text = data.recipeInfo;
        bool canCreateResult = PlaceIngrediences();

        recipeResult.OccupyByData(activeRecipe.result,canCreateResult);

        MakeVisible(true);

    }
    private void MakeVisible(bool doMakeVisible)
    {
        Debug.Log("* "+(doMakeVisible?"SHOW":"HIDE")+" RECIPE *");
        panelOpen = doMakeVisible;
        //Debug.Log("Panel Open set to: "+panelOpen);

        Debug.Log(" Crossfade to "+(doMakeVisible? "MakeVisible" : "MakeInVisible"));

        storedCrossfades.Enqueue(doMakeVisible ? "MakeVisible" : "MakeInVisible");
    }
    private void Update()
    {
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
        bool canCreate = true;
        ClearItems();
        for (int i=0; i<activeRecipe.ingredienses.Length; i++) 
        {
            RecipeAmount ingredienceData = activeRecipe.ingredienses[i];
            RecipeItem item = GetNextRecipeItem(i);
            item.gameObject.SetActive(true);
            int playerHas = inventory.GetResources()[(int)ingredienceData.resourceData.resource];
            item.OccupyByData(ingredienceData.resourceData, ingredienceData.amount, playerHas);
            if(ingredienceData.amount>playerHas)
                canCreate = false;
        }
        return canCreate;
    }

    private RecipeItem GetNextRecipeItem(int i)
    {
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
        {
            item.gameObject.SetActive(false);
        }
    }
    
    // ANIMATIONS
    public void AnimationComplete()
    {
        Debug.Log(" MakeInvisible Completed - Close Complete invoked from "+activeRecipe.result.itemName);
        CloseComplete?.Invoke();
    }

}
