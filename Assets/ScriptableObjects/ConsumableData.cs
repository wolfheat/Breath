using UnityEngine;

[CreateAssetMenu(menuName = "Items/ConsumableData", fileName = "Consumable")]
public class ConsumableData : ItemData
{
    public override ItemType itemType { get; } = ItemType.Consumable;

    public int healthRegain;
}
