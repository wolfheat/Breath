using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Items/AllRecipesData", fileName = "AllRecipesData")]
public class AllRecipesData : ScriptableObject
{
    [SerializeField] public RecipeData[] toolRecipes;
    [SerializeField] public RecipeData[] foodRecipes;
    [SerializeField] public RecipeData[] armorRecipes;
    [SerializeField] public RecipeData[] resourceRecipes;
}