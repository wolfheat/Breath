using UnityEngine;
public enum ItemType { Resource, Breakable, Object, Advanced, Equipable, Consumable,
    ResourceAmount
}
public enum Resource { Al, Cu, Plastic, Textile, Ti, Water, Food };

public abstract class ItemData : BaseData
{
    public string itemName;
    public abstract ItemType itemType { get; }
    public override int Type => (int)itemType;
    public Vector2Int size = Vector2Int.one;
    public Sprite picture;
}
