using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Items/RecipeData", fileName = "Recipe")]
public class RecipeData : ScriptableObject
{
    public string recipeName;

    [SerializeField] public RecipeAmount[] ingredienses;
    public ItemData result;

    [TextArea(5, 10)]
    public string recipeInfo;
}

[System.Serializable]
public struct RecipeAmount
{
    public ResourceData resourceData;
    public int amount;
}
