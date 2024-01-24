using UnityEngine;

[CreateAssetMenu(menuName = "Items/RecipeData", fileName = "Recipe")]
public class RecipeData : ScriptableObject
{
    public string recipeName;
    public ResourceData[] ingredienses;
    public ItemData result;
}
