using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Resource{Coin,Cu,Al,Ti,Plastic};

public class Inventory : MonoBehaviour
{
    private int coinsAmount = 0;
    private int copper = 0;
    private int aluminium = 0;
    private int plastic = 0;
    private int titanium = 0;
    
    private int[] heldResources = new int[] {0,0,0,0,0};

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
