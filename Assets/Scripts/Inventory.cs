using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{    
    private int[] heldResources = new int[] {0,0,0,0,0};

    public int[] GetResources()
    {
        return heldResources;
    }
    public void AddItem(PickableItem item)
    {
        AddItem(item.Data.resource, 1);
    }
    public void AddItem(Resource type, int amt)
    {
        heldResources[(int)type] += amt;
        UpdateInventory();
    }
    public void RemoveItem(Resource type, int amt)
    {
        if(heldResources[(int)type]>=amt) heldResources[(int)type] -= amt;
        UpdateInventory();
    }

    private void UpdateInventory()
    {
        UIController.Instance.InventoryChanged();
    }
}
