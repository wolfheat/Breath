using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Items/InventoryItemsDatabase", fileName = "InventoryItemsDatabase")]
public class InventoryItemsDatabase : ScriptableObject
{
    [SerializeField] public ObjectData[] consumables;
    [SerializeField] public ObjectData[] equipables;
}