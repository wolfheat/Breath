using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class RecipeInfo : MonoBehaviour
{
    [SerializeField] RecipeItem recipeItemPrefab;
    [SerializeField] TextMeshProUGUI recipeName;
    [SerializeField] GameObject ingredienceHolder;
    [SerializeField] GameObject panel;
    [SerializeField] RecipeItem recipeResult;
    [SerializeField] Animator animator;

    private List<RecipeItem> recipeItems = new();

    RecipeData activeRecipe;

    public void HideRecipe()
    {
        MakeVisible(false);
    }
    public void ShowRecipe(RecipeData data)
    {
        MakeVisible(true);

        activeRecipe = data;
        // Display this recipe
        recipeName.text = data.recipeName;
        ClearItems();
        PlaceIngrediences();

        recipeResult.OccupyByData(activeRecipe.result);

    }
    private void MakeVisible(bool doMakeVisible)
    {
        if (doMakeVisible)
            panel.SetActive(true);

        animator.Play(doMakeVisible ? "MakeVisible" : "MakeInVisible");

    }


    public void AnimationComplete()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).speed < 0)
            panel.SetActive(false);

    }

    private void PlaceIngrediences()
    {
        foreach (var ingredience in activeRecipe.ingredienses) 
        {
            RecipeItem item = Instantiate(recipeItemPrefab,ingredienceHolder.transform);
            item.OccupyByData(ingredience);
            recipeItems.Add(item);
        }
    }

    private void ClearItems()
    {
        foreach (var item in recipeItems)
        {
            Destroy(item.gameObject);
        }
        recipeItems.Clear();
    }
}
