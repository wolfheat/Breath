using System;
using UnityEngine;

public enum EquipType{Head,Body,Feet,Tank,Booster};
public class EquipedGrid : MonoBehaviour
{
    [SerializeField] InventoryGrid grid;
    [SerializeField] GameObject[] itemspots;
    private UIItem[] items = new UIItem[5];


    public bool TryPlaceItem(UIItem item)
    {
        //Check what type the item is
        //check if item is already placed (if so replace if item fits inventory?)
        if(item.data.itemType == ItemType.Equipable)
        {
            EquipableData data = (item.data as EquipableData);
            int itemType = (int)data.equipType;
            Debug.Log("Placing data that is equipable "+data.itemName);
            if (items[itemType] != null)
            {
                if(item == items[itemType])
                {
                    item.ResetPosition();
                    return true;
                }

                UIItem used = items[itemType];
                Debug.Log("Spot contains one item: "+ items[itemType].data.itemName);
                
                if (grid.PlaceItemAnywhere(used))
                {
                    EquipItem(item);
                    
                    return true;
                }
                return false;
            }
            Debug.Log("No item in equipable so can equip" + data.itemName);
            EquipItem(item);
            return true;
        }
        return false;
    }

    public void RemoveIfEquipped(UIItem item)
    {
        if (item.data.itemType != ItemType.Equipable)
            return;
        EquipableData data = (item.data as EquipableData);
        int type = (int)data.equipType;
        if (items[type] == item)
        {
            //remove
            items[type] = null;
        }

    }

    private void EquipItem(UIItem item)
    {
        if (item.data is not EquipableData) return;

        Debug.Log("Equipping item since its Equipable" + item.data.itemName);

        EquipableData data = (item.data as EquipableData);
        int itemType = (int)data.equipType;

        grid.RemovePlacement(item);

        Debug.Log("Item at pos" + item.transform.localPosition);

        Debug.Log("Item set to itemspot " + itemspots[itemType].name+" which is at "+ itemspots[itemType].transform.localPosition);
        item.SetHomePositionAndSpot(itemspots[itemType].transform.localPosition,new Vector2Int(-1,-1));

        Debug.Log("Item at pos after " + item.transform.localPosition);

        items[itemType] = item;
    }



}
