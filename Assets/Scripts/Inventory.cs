using UnityEngine;

// Resource reference { Al, Cu, Ti, Plastic, Textile, Water, Food };

public class Inventory : MonoBehaviour
{    
    private int[] heldResources = new int[7];
    [SerializeField] InventoryGrid grid;
    [SerializeField] EquipedGrid equiped;

    public static Inventory Instance;



    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        // player starts with 4 water
        heldResources[5] = 4;
    }

    public bool PlayerHasEquipped(DestructType destructType)
    {
        int destructTypeAsToolIndex = (int)destructType + 5;
        return equiped.HasItemOfTypeEquipped(destructTypeAsToolIndex);
    }

    public int[] GetResources() => heldResources;
    public bool AddResource(ResourceItem item)
    {
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
        heldResources[(int)type] += amt;
        UpdateInventory();
        return true;
    }
    public void RemoveResource(Resource type, int amt)
    {
        if(heldResources[(int)type]>=amt) heldResources[(int)type] -= amt;
        UpdateInventory();
    }

    private void UpdateInventory() => UIController.Instance.InventoryChanged();

    public bool CanAfford(RecipeData recipeData)
    {
        // Checking if player can afford recipe
        foreach(var resourceAmt in recipeData.ingredienses)
        {
            int owns = heldResources[(int)resourceAmt.resourceData.resource];
            
            if (resourceAmt.amount > owns)
                return false;
        }
        return true;
    }

    public void RemoveItems(RecipeAmount[] ingredienses)
    {
        foreach(var recipeAmount in ingredienses)
            RemoveResource(recipeAmount.resourceData.resource,recipeAmount.amount);
    }

    public void DefineGameDataBeforSave() => SavingUtility.playerGameData.Inventory = UpdateStoredInventoryBeforeSave();

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
        return save;
    }

    public void LoadFromFile()
    {
        InventorySave inv = SavingUtility.playerGameData.Inventory;

        heldResources = inv.resources;

        grid.AddItemsToInventory(inv.inventorySaveItems);

        UpdateInventory();
    }
}
