using UnityEngine;

public enum DestructType { Breakable, Drillable, Flesh}
public enum BoxType { Red, Blue, Yellow, Crystal}

[CreateAssetMenu(menuName = "Items/DestructableData", fileName ="Destructable")]
public class DestructableData : ScriptableObject
{
    public string itemName;
    public BoxType boxType;
    public Resource[] resources;
    public DestructType destructType = DestructType.Breakable;
    public ItemType itemType = ItemType.Breakable;
}
