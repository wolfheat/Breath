using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemAdvancedData", fileName ="ItemX")]
public class ItemAdvancedData : ItemData
{
    public override ItemType itemType { get; } = ItemType.Advanced;
}
