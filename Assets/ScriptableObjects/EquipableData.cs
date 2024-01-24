using UnityEngine;

[CreateAssetMenu(menuName = "Items/EquipableData", fileName ="Equippable")]
public class EquipableData : ObjectData
{
    public override ItemType itemType { get; } = ItemType.Equipable;
    public EquipType equipType;
}
