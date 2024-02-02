using UnityEngine;


//public enum Resource { Al, Cu, Ti, Plastic, Textile, Water, Food };


public class Inventory : MonoBehaviour
{    
    private int[] heldResources = new int[7];
    [SerializeField] InventoryGrid grid;
    [SerializeField] EquipedGrid equiped;

    public static Inventory Instance;



    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        heldResources[5]= 4;

        equiped.EquipmentChanged += UpdateEquipment;

    }

    private void UpdateEquipment()
    {
        Debug.Log("Update equipment");


    }

    public bool PlayerHasEquipped(DestructType destructType)
    {
        int destructTypeAsToolIndex = (int)destructType + 5;
        return equiped.HasItemOfTypeEquipped(destructTypeAsToolIndex);
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
        Debug.Log("Checking if player can afford recipe "+recipeData.recipeName);
        foreach(var resourceAmt in recipeData.ingredienses)
        {

            int owns = heldResources[(int)resourceAmt.resourceData.resource];
            Debug.Log(resourceAmt.resourceData.itemName +" costs: "+ resourceAmt.amount+ " player owns amount: "+owns);
            if (resourceAmt.amount > owns)
            {
                return false;
            }

        }
        return true;
    }

    public void RemoveItems(RecipeAmount[] ingredienses)
    {
        foreach(var recipeAmount in ingredienses)
        {
            RemoveItem(recipeAmount.resourceData.resource,recipeAmount.amount);
        }
    }
}
