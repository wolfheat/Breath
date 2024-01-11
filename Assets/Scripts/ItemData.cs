using UnityEngine;
public enum ItemType { Resource, Breakable, Other }
public enum Resource { Al, Cu, Ti, Plastic, Textile, Water, Food };

[CreateAssetMenu(menuName = "Items/ItemData", fileName ="Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Resource resource;
    public ItemType itemType;
    public Sprite picture;
}
