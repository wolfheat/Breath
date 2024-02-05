using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemAdvancedData", fileName ="ItemX")]
public abstract class ObjectData : ItemData
{
    public override ItemType itemType { get; } = ItemType.Object;
    public abstract int SubType { get; }
}
