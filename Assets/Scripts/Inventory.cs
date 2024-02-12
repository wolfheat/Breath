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
    public bool AddResource(ResourceItem item)
    {
        // Add item depending on type?        
        bool didAddResource = AddResource((item.Data as ResourceData).resource, 1);
        return didAddResource;
    }
    public bool AddItem(ItemData data)
    {
        bool didAddToInventory = grid.AddItemToInventory(data);
        return didAddToInventory;
    }
    public bool AddResource(Resource type, int amt)
    {
        Debug.Log("Trying to pick up resource type "+type+" = index "+type);
        heldResources[(int)type] += amt;
        UpdateInventory();
        return true;
    }
    public void RemoveResource(Resource type, int amt)
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
            RemoveResource(recipeAmount.resourceData.resource,recipeAmount.amount);
        }
    }

    public void DefineGameDataBeforSave()
    {
        // Player position and looking direction (Tilt is disregarder, looking direction is good enough)
        SavingUtility.playerGameData.Inventory = UpdateStoredInventoryBeforeSave();

    }

    private InventorySave UpdateStoredInventoryBeforeSave()
    {
        InventorySave save = new InventorySave();
        save.resources = heldResources;

        UIItem[] uIItems = grid.GetAllItems().ToArray();
        InventorySaveItem[] itemsSave = new InventorySaveItem[uIItems.Length];
        // Fill with items
        for (int i = 0; i < itemsSave.Length; i++)
        {
            UIItem item = uIItems[i];
            ObjectData objectData = item.data as ObjectData;
            itemsSave[i] = new InventorySaveItem() { mainType = objectData.Type, subType = objectData.SubType , gridPosition = new int[2] { item.Spot[0], item.Spot[1] }};
        }
        save.inventorySaveItems = itemsSave;
        Debug.Log("  * Saving inventory *");
        Debug.Log("    Saving " + save.resources.Length+" resources.");
        Debug.Log("    Saving " + uIItems.Length + " inventory items.");
        return save;
    }

    public void LoadFromFile()
    {
        Debug.Log("  * Loading inventory *");
        InventorySave inv = SavingUtility.playerGameData.Inventory;
        heldResources = inv.resources;

        Debug.Log("    Loading " + heldResources.Length + " resources.");
        Debug.Log("    Loading " + inv.inventorySaveItems.Length + " inventory items.");

        grid.AddItemsToInventory(inv.inventorySaveItems);

        UpdateInventory();
    }
}
