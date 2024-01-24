using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public enum Resource { Al, Cu, Ti, Plastic, Textile, Water, Food };

public class Inventory : MonoBehaviour
{    
    private int[] heldResources = new int[7];
    [SerializeField] InventoryGrid grid;

    public static Inventory Instance;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

    }

    public int[] GetResources()
    {
        return heldResources;
    }
    public bool AddItem(PickableItem item)
    {
        // Add item depending on type?
        if(item.Data is ResourceData)
        {
            ResourceData data = (ResourceData)item.Data;
            bool didAdd = AddItem(data.resource, 1);
            return didAdd;
        }else if(item.Data is ObjectData)
        {
            ObjectData data = (ObjectData)item.Data;
            bool didAdd = AddItem(data);
            return didAdd;
        }
        return false;
    }
    public bool AddItem(ObjectData data)
    {
        Debug.Log("Trying to pick up object!");

        bool addedToInventory = grid.AddItemToInventory(data);
        return addedToInventory;
    }
    public bool AddItem(Resource type, int amt)
    {
        Debug.Log("Trying to pick up resource type "+type+" = index "+type);
        heldResources[(int)type] += amt;
        UpdateInventory();
        return true;
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

    public bool CanAfford(RecipeData recipeData)
    {
        Debug.Log("Checking if player can affor recipe "+recipeData.recipeName);
        foreach(var item in recipeData.ingredienses)
        {
            int owns = heldResources[(int)item.resource];
            //Debug.Log("Costs: "+item.itemName+" amt: ?"+" player owns amount: "+owns);

        }
        return true;
    }
}
