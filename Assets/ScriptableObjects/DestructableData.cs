using UnityEngine;

public enum DestructType { Breakable, Drillable };

[CreateAssetMenu(menuName = "Items/DestructableData", fileName ="Destructable")]
public class DestructableData : ScriptableObject
{
    public string itemName;
    public Resource[] resources;
    public DestructType destructType = DestructType.Breakable;
    public ItemType itemType = ItemType.Breakable;
}
