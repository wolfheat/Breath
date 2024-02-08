using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wolfheat.StartMenu;
using static UnityEditor.Progress;

public enum UIItemSizes{size3x3, size3x2, size2x2, size2x1, size1x1 }

public class InventoryGrid : MonoBehaviour
{
    [SerializeField] private EquipedGrid equipped;
    [SerializeField] private GameObject inventoryGridTilePrefab;
    [SerializeField] private GameObject tileParent;
    [SerializeField] private GameObject itemHolder;
    [SerializeField] private UIItem uiItemPrefab;
    [SerializeField] private UIItem[] uiItemPrefabs;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] InventoryItemsDatabase database;

    [SerializeField] private List<ItemData> heldItemsData;
    [SerializeField] private List<UIItem> heldItems;

    private UIItem[,] grid;
    private Transform[,] gridTiles = new Transform[8,6];
    private const float Tilesize = 89;
    private void Start()
    {
        ResetGrid();

        CreateInventoryBackgroundGrid();

        // Occupy With held items
        OccupyWithInitialItems();
    }

    private void ResetGrid()
    {
        grid = new UIItem[8, 6];
    }

    private void CreateInventoryBackgroundGrid()
    {
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                var gridTile = Instantiate(inventoryGridTilePrefab, tileParent.transform);
                gridTile.transform.localPosition = new Vector3(col * Tilesize, -row * Tilesize);
                gridTiles[row, col] = gridTile.transform;
            }
        }
    }

    private void OccupyWithInitialItems()
    {
        foreach (var data in heldItemsData)
        {
            bool didAd = AddItemToInventory(data);
        }
    }

    public bool AddItemsToInventory(InventorySaveItem[] items)
    {
        ClearInventory();

        // Load in all items here
        foreach (var item in items)
        {
            ItemData data = (ItemType)item.mainType switch
            {
                ItemType.Equipable => database.equipables[item.subType],
                ItemType.Consumable => database.consumables[item.subType],
                _ => null
            };
            AddItemToInventoryAt(data, item.gridPosition);            
        }
        return true;
    }

    private void AddItemToInventoryAt(ItemData data, int[] gridPosition)
    {
        UIItem newItem = Instantiate(uiItemPrefab, itemHolder.transform);
        if (data is EquipableData)
        {
            Vector2 rectSize = equipped.GetItemRectSize(data);
            newItem.SetData(data, rectSize);
        }
        else
        {
            newItem.SetData(data);
        }
        if (gridPosition[0] != -1)
            PlaceAtSpot(gridPosition[0], gridPosition[1], newItem);
        else
            equipped.ForcedEquip(newItem);

        heldItems.Add(newItem);
    }

    public bool AddItemToInventory(ItemData data, bool place=true)
    {
        UIItem newItem = Instantiate(uiItemPrefab, itemHolder.transform);
        if(data is EquipableData)
        {
            Vector2 rectSize = equipped.GetItemRectSize(data);
            newItem.SetData(data, rectSize);
        }
        else
        {
            // Add resources differently?
            newItem.SetData(data);
        }
        if (place)
        {            
            bool didPlace = PlaceItemAtFirstFreeSpot(newItem);
            if (!didPlace)
            {
                Debug.Log("Could not place item " + newItem.data.itemName + " in inventory");
                HUDMessage.Instance.ShowMessage("Not enough inventory space");
                Destroy(newItem.gameObject);
                return false;
            }
        }
        heldItems.Add(newItem);
        return true;
    }

    public bool PlaceItemAtFirstFreeSpot(UIItem item)
    {
        for (int row = 0; row < grid.GetLength(0) ; row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                //Check if grid start position is free
                if (grid[row, col] != null)
                    continue;
                // Check if entire item fits here, if so place it
                if (ItemFits(row,col,item.data.size.x, item.data.size.y,item))
                {
                    return true;
                }
            }
        }
        return false;        
    }

    public void PlaceAtSpot(int row, int col, UIItem item)
    {
        if (item.IsInInventory())
        {
            //Debug.Log("Removing Items placement Spot: "+item.Spot);
            RemovePlacement(item);
        }
        for (int k = 0; k < item.data.size.x; k++)
        {
            for (int l = 0; l < item.data.size.y; l++)
            {
                grid[row + k, col + l] = item;
            }
        }
        item.SetHomePositionAndSpot(gridTiles[row, col].localPosition,new Vector2Int(row,col));
        equipped.RemoveIfEquipped(item);
    }
    
    public void RemovePlacement(UIItem item)
    {
        for (int k = 0; k < item.data.size.x; k++)
        {
            for (int l = 0; l < item.data.size.y; l++)
            {
                //Debug.Log("Object "+item.data.itemName+" occupy ("+(row+k)+","+(col+l)+")");
                grid[item.Spot.x + k, item.Spot.y + l] = null;
            }
        }
    }

    public bool PlaceItemAnywhere(UIItem item, bool alsoPlace = true)
    {
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                if (ItemFits(row, col, item.data.size.x, item.data.size.y, item, alsoPlace))
                    return true;
            }
        }
        return false;
    }

    public bool ItemFits(int row, int col, int x, int y, UIItem item,bool alsoPlace=true)
    {
        // Outside
        if (row + x - 1 >= grid.GetLength(0) || col + y - 1 >= grid.GetLength(1)) return false;
        for (int k = 0; k < x; k++)
        {
            for (int l = 0; l < y; l++)
            {
                if (grid[row+k,col+l]!=null)
                    if(grid[row + k, col + l]!=item)
                        return false;

                //Debug.Log("No item at (" + (row + k) + "," + (col + l) + ") item: "+ grid[row + k, col + l]);
            }     
        }
        //Debug.Log("Item "+item.data.itemName+" fits at Spot ["+row +","+col+"] grid = ["+grid.GetLength(0)+","+ grid.GetLength(1)+"]");

        if (alsoPlace)
            PlaceAtSpot(row, col, item);
        return true;
    }
    public bool RequestEquip(UIItem item)
    {
        //Debug.Log("Try equip item "+item.data.itemName);
        if (equipped.IsEquipped(item))
        {
            if (!playerMovement.InGravity)
            {
                HUDMessage.Instance.ShowMessage("Can only swap equipments in gravity!");
                //item.ResetPosition();
                return false;
            }
            if (PlaceItemAnywhere(item))
            {
                //Debug.Log("Item is placed on grid "+item.data.itemName);

                equipped.RemoveIfEquipped(item);
                return true;
            }
            else
            {

            }
            HUDMessage.Instance.ShowMessage("Not enough space in inventory"); // Maybe drop on floor here?
        }
        equipped.TryPlaceItem(item);
        return true;
    }
    public void RequestMove(UIItem uIItem, Vector2 drop)
    {
        // Check if equipped

        // Drop Point (grid index)
        (int col, int row) = DeriveGridIndex(drop);

        // Outside grid
        if (col < 0 || row < 0 || col >= grid.GetLength(1) || row >= grid.GetLength(0))
        {
            // Equipping 
            if(col >= grid.GetLength(1))
            {
                bool placed = equipped.TryPlaceItem(uIItem);
                if (placed)
                {
                    Debug.Log("Item was Equiped");
                    return;
                }

            }
            
            // Not Equipping
            return;
        }

        // Same Spot 
        if(new Vector2Int(row,col) == uIItem.Spot)
            return;

        // New Spot (place or reset)
        ItemFits(row, col, uIItem.data.size.x, uIItem.data.size.y, uIItem);
        //if (!ItemFits(row, col, uIItem.data.size.x, uIItem.data.size.y,uIItem))
        //uIItem.ResetPosition();

    }

    private (int col, int row) DeriveGridIndex(Vector2 drop)
    {
        //Debug.Log("Grid recieved request of dropping item at " + drop + " GRID AT: " + transform.position);

        float scaleCorrection = Screen.height / 1080f;
        //Debug.Log("Reading current game scale:" + scaleCorrection);

        float diffx = (drop.x - transform.position.x) / scaleCorrection;
        float diffy = (drop.y - transform.position.y) / scaleCorrection;
        //Debug.Log("Drop at Spot difference (" + diffx + "," + diffy + ")");

        int col = (int)Math.Round(diffx / Tilesize);
        int row = -(int)Math.Round(diffy / Tilesize);
        return (col, row);

    }
    public bool ClickTimerLimited { get; private set; }
    public IEnumerator ClickTimerLimiter()
    {
        ClickTimerLimited = true;
        yield return new WaitForSeconds(0.15f);
        ClickTimerLimited = false;
    }

    public void ClearInventory()
    {
        foreach (var item in heldItems)
        {
            //Debug.Log("Trying to destroy "+item.data.itemName);
            Destroy(item.gameObject);
        }
        heldItems.Clear();
        ResetGrid();

    }
    public void RemoveFromInventory(UIItem item)
    {
        if (item.IsInInventory())
            RemovePlacement(item);
        if(heldItems.Contains(item))
            heldItems.Remove(item);
        DragObject.Instance.UnSetDragedItem();
        Destroy(item.gameObject);
    }
    public void DropItem(UIItem item)
    {
        Debug.Log("Dropping item "+item.data.itemName);
        // Remove item from inventory/equipped
        equipped.RemoveIfEquipped(item);

        // Place item in Box
        ItemCreator.Instance.InstantiateGenericItemAt(item.data, playerMovement.transform.position+ playerMovement.transform.forward * 2,playerMovement.transform.rotation);

        // Remove item form inventory   
        RemoveFromInventory(item);

        // PLay drop sound
        SoundMaster.Instance.PlaySound(SoundName.DropItem);

    }

    public List<UIItem> GetAllItems()
    {
        return heldItems;
    }
}
