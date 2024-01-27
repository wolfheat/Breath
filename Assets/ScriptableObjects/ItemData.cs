using UnityEngine;
public enum ItemType { Resource, Breakable, Object, Advanced, Equipable, Consumable,
    ResourceAmount
}
public enum Resource { Al, Cu, Plastic, Textile, Ti, Water, Food };

public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public abstract ItemType itemType { get; }
    public Vector2Int size = Vector2Int.one;
    public Sprite picture;
}
