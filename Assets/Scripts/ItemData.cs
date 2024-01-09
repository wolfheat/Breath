using UnityEngine;
public enum ItemType { Resource, Breakable, Other }
public enum Resource { Coin, Cu, Al, Ti, Plastic, Water, Textile };

[CreateAssetMenu(menuName = "Items/ItemData", fileName ="Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Resource resource;
    public ItemType itemType;
}
