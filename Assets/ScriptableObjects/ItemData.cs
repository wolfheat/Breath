using UnityEngine;
public enum ItemType { Resource, Breakable, Other, Advanced}
public enum Resource { Al, Cu, Ti, Plastic, Textile, Water, Food };

public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public abstract ItemType itemType { get; }
    public Vector2Int size = Vector2Int.one;
    public Sprite picture;
}
