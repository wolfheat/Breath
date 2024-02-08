using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Items/InventoryItemsDatabase", fileName = "InventoryItemsDatabase")]
public class InventoryItemsDatabase : ScriptableObject
{
    [SerializeField] public ObjectData[] consumables;
    [SerializeField] public ObjectData[] equipables;

    public ItemData GetData(ItemType mainType, int subType)
    {
        switch (mainType)
        {
            case ItemType.Equipable:
                return equipables[subType];
            case ItemType.Consumable:
                return consumables[subType];
            default: 
                return null;
        }
    }
}