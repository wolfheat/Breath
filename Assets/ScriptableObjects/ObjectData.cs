using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemAdvancedData", fileName ="ItemX")]
public class ObjectData : ItemData
{
    public override ItemType itemType { get; } = ItemType.Object;
}
