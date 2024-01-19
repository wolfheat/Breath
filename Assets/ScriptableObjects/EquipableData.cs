using UnityEngine;

[CreateAssetMenu(menuName = "Items/EquipableData", fileName ="ItemX")]
public class EquipableData : ItemAdvancedData
{
    public override ItemType itemType { get; } = ItemType.Equipable;
    public EquipType equipType;
}
