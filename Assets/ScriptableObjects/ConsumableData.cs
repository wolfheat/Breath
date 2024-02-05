using UnityEngine;

public enum ConsumableType {Bread,Cake,FirstAid,Potion,WaterBottle}

[CreateAssetMenu(menuName = "Items/ConsumableData", fileName = "Consumable")]
public class ConsumableData : ObjectData
{
    public override ItemType itemType { get; } = ItemType.Consumable;
    public override int SubType { get { return (int)type; } }

    public int healthRegain;
    public ConsumableType type;
}
