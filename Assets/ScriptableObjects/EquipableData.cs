using System.Collections.Generic;
using UnityEngine;

public enum EquippableType { BlasterGun, Drill, SledgeHammer, JetPack, OxygenTankSmall, OxygenTank, OxygenTankMax, SpaceBoots, SpaceHelmet, SpaceSuit}

[CreateAssetMenu(menuName = "Items/EquipableData", fileName ="Equippable")]
public class EquipableData : ObjectData
{
    public override ItemType itemType { get; } = ItemType.Equipable;
    public EquipType equipType;

    public EquippableType subType;
    public override int SubType { get { return (int)subType; } }

    public List<BenefitData> benefits;
}

