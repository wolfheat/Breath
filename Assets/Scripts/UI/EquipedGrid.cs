using System;
using UnityEngine;

public enum EquipType{Head,Body,Feet,Tank,JetPack,Hammer,Drill,Gun,Sword};
public class EquipedGrid : MonoBehaviour
{
    [SerializeField] InventoryGrid grid;
    [SerializeField] GameObject[] itemspots;
    [SerializeField] PlayerMovement playerMovement;

    private UIItem[] items = new UIItem[9];

    public Action EquipmentChanged;

    public Vector2 GetItemRectSize(ItemData data)
    {
        // Get the Data and Extract the Image Information
        EquipableData equipableData = (EquipableData)data;
        Rect equipmentRect = itemspots[(int)equipableData.equipType].GetComponent<RectTransform>().rect;
        float equipheight = equipmentRect.size.y;

        // Determine items scale in equipped
        float ratio = data.picture.rect.size.x/data.picture.rect.size.y;
        Vector2 equipSize = new Vector2(equipheight * ratio, equipheight);

        // Return the Calculated Rect Size
        return equipSize;            
    }

    public void ForcedEquip(UIItem item) => EquipItem(item);

    public bool PlaceItem(UIItem item)
    {
        // Check what type the item is
        // Check if the Item is Already Placed (if so replace if item fits inventory)
        if (item.data.itemType == ItemType.Equipable)
        {
            // Determine if this item type is equipped allready
            EquipableData data = (item.data as EquipableData);
            int itemType = (int)data.equipType;
            UIItem used = items[itemType];

            if (used)
            {
                if (item == used)
                {
                    item.ResetPosition();
                    return true;
                }
                // Spot has equipped item remove item to be placed from taking up a spot on the grid
                grid.RemovePlacement(item);

                // Place the previously equipped item on the grid
                if (grid.PlaceItemAnywhere(used))
                {
                    // Equipped item placed on grid, now equip the new one
                    EquipItem(item);
                    return true;
                }
                // Item could not be equipped due to no place in inventory
                grid.PlaceAtSpot(item.Spot.x, item.Spot.y, item);
                return false;
            }
            grid.RemovePlacement(item);
            EquipItem(item);
            return true;
        }
        return false;
    }
    public bool TryPlaceItem(UIItem item)
    {
        // Limit player from equipping if not in gravity
        if (!playerMovement.InGravity)
        {
            HUDMessage.Instance.ShowMessage("Can only swap equipments in gravity!");
            item.ResetPosition();
            return false;
        }
        return PlaceItem(item);
    }

    public void RemoveIfEquipped(UIItem item)
    {
        // Check if even equippable item
        if (item.data.itemType != ItemType.Equipable)
            return;

        // Get item type
        EquipableData data = (item.data as EquipableData);
        int type = (int)data.equipType;

        // Remove if present
        if (items[type] == item)
            items[type] = null;

        SetAllAdditions();
    }

    public bool HasItemOfTypeEquipped(int type) => items[type] != null;

    public bool IsEquipped(UIItem item)
    {
        // Check if an equippable item
        if (item.data.itemType != ItemType.Equipable)
            return false;

        // Get item type
        EquipableData data = (item.data as EquipableData);
        int type = (int)data.equipType;

        // Is this the item
        if (items[type] == item)
            return true;
        return false;
    }

    private void EquipItem(UIItem item)
    {
        if (item.data is not EquipableData)
            return; // should never happen

        // Determine item type
        EquipableData data = (item.data as EquipableData);
        int itemType = (int)data.equipType;
                
        // Determin offset
        item.SetHomePositionAndSpot(itemspots[itemType].transform.parent.localPosition,new Vector2Int(-1,-1));

        // place item in array as equipped
        items[itemType] = item;

        // Determine all equipped items stats effects
        SetAllAdditions();
    }

    public int Oxygen { get; set; }
    public int Health { get; set; }
    public int Speed { get; set; }

    public void SetAllAdditions()
    {
        // Determine how all equipped items affect player stats
        Oxygen = 0;
        Health = 0;
        Speed = 0;
        foreach (var item in items)
        {
            if (item != null)
            {
                EquipableData data = item.data as EquipableData;

                foreach (var benefit in data.benefits)
                {
                    if (benefit is OxygenBenefitData oxygenBenefit)
                        Oxygen += oxygenBenefit.oxygen;
                    if (benefit is HealthBenefitData healthBenefit)
                        Health += healthBenefit.health;
                    if (benefit is SpeedBenefitData speedBenefit)
                        Speed += speedBenefit.speed;
                }
            }
        }

        // Notify of stats change from equipments
        EquipmentChanged.Invoke();
    }
}
