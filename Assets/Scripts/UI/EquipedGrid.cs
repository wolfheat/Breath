﻿using System;
using UnityEngine;

public enum EquipType{Head,Body,Feet,Tank,Booster};
public class EquipedGrid : MonoBehaviour
{
    [SerializeField] InventoryGrid grid;
    [SerializeField] GameObject[] itemspots;
    private UIItem[] items = new UIItem[5];

    public Vector2 GetItemRectSize(ItemData data)
    {
        EquipableData equipableData = (EquipableData)data;
        float equipheight = itemspots[(int)equipableData.equipType].GetComponent<RectTransform>().rect.size.y;
        // Determine items scale in equipped
        float ratio = data.picture.rect.size.x/data.picture.rect.size.y;
        Debug.Log("Image ratio = "+ratio);
        return new Vector2(equipheight*ratio,equipheight);
            
    }

    public bool TryPlaceItem(UIItem item)
    {
        //Check what type the item is
        //check if item is already placed (if so replace if item fits inventory?)
        if(item.data.itemType == ItemType.Equipable)
        {
            EquipableData data = (item.data as EquipableData);
            int itemType = (int)data.equipType;
            //Debug.Log("Placing data that is equipable "+data.itemName);
            UIItem used = items[itemType];
            //Debug.Log("Used:  "+used);

            if (used)
            {
                if(item == used)
                {
                    item.ResetPosition();
                    return true;
                }

                // Debug.Log("Spot contains one item: "+ used.data.itemName);

                grid.RemovePlacement(item);

                if (grid.PlaceItemAnywhere(used))
                {
                    EquipItem(item);                    
                    return true;
                }
                // Reset the items placement
                grid.PlaceAtSpot(item.Spot.x,item.Spot.y, item);
                return false;
            }
            //Debug.Log("No item in equipable so can equip" + data.itemName);

            grid.RemovePlacement(item);
            EquipItem(item);
            return true;
        }
        return false;
    }

    public void RemoveIfEquipped(UIItem item)
    {
        //Debug.Log("Removing equipped item data if equipped" + item.data.itemName);

        // Check if even equippable item
        if (item.data.itemType != ItemType.Equipable)
            return;

        // Get item type
        EquipableData data = (item.data as EquipableData);
        int type = (int)data.equipType;

        // remove if present
        if (items[type] == item)
        {
            // Debug.Log("Removing equipped item data!");
            items[type] = null;
        }
    }

    public bool IsEquipped(UIItem item)
    {
        // Check if an equippable item
        if (item.data.itemType != ItemType.Equipable)
            return false;

        // Get item type
        EquipableData data = (item.data as EquipableData);
        int type = (int)data.equipType;

        // is this the item
        if (items[type] == item)
            return true;
        return false;
    }

    private void EquipItem(UIItem item)
    {
        if (item.data is not EquipableData)
        {
            Debug.LogWarning("This hould never happen");
            return; // should never happen
        }

        //Debug.Log("Equipping item since its Equipable" + item.data.itemName);

        EquipableData data = (item.data as EquipableData);
        int itemType = (int)data.equipType;

        //Debug.Log("Item at pos" + item.transform.localPosition);

        //Debug.Log("Item set to itemspot " + itemspots[itemType].name+" which is at "+ itemspots[itemType].transform.localPosition);

        // Determin offset


        item.SetHomePositionAndSpot(itemspots[itemType].transform.localPosition,new Vector2Int(-1,-1));

        //Debug.Log("Item at pos after " + item.transform.localPosition);

        items[itemType] = item;
    }



}
