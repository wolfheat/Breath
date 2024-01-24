using UnityEngine;
public enum ItemType { Resource, Breakable, Other, Advanced, Equipable,Consumable}
public enum Resource { Al, Cu, Plastic, Textile, Ti, Water, Food };

public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public abstract ItemType itemType { get; }
    public Vector2Int size = Vector2Int.one;
    public Sprite picture;
}
