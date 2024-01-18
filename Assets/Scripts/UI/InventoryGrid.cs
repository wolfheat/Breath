using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public enum UIItemSizes{size3x3, size3x2, size2x2, size2x1, size1x1 }

public class InventoryGrid : MonoBehaviour
{
    [SerializeField] private GameObject inventoryGridTilePrefab;
    [SerializeField] private GameObject tileParent;
    [SerializeField] private GameObject itemHolder;
    [SerializeField] private UIItem uiItemPrefab;
    [SerializeField] private UIItem[] uiItemPrefabs;
    
    [SerializeField] private List<ItemData> heldItemsData;
    [SerializeField] private List<UIItem> heldItems;

    private UIItem[,] grid = new UIItem[8,6];
    private Transform[,] gridTiles = new Transform[8,6];
    private const float Tilesize = 89;
    private void Start()
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

        // Occupy With held items
        Occupy();
    }

    private void Occupy()
    {
        foreach (var item in heldItemsData)
        {
            Debug.Log("Creating held item of size "+ item.size);
            UIItem newItem = Instantiate(uiItemPrefab,itemHolder.transform);
            newItem.SetData(item);
            Debug.Log("item is: " + newItem);
            heldItems.Add(newItem);
        }

        foreach (var item in heldItems)
        {
            Debug.Log("item is: "+item);
            PlaceItemAtFirstFreeSpot(item);
        }
    }

    private void PlaceItemAtFirstFreeSpot(UIItem item)
    {
        for (int row = 0; row < grid.GetLength(0) ; row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                //Check grid position
                if (grid[row, col] != null)
                    continue;
                Debug.Log("Spot " + row + "," + col + " is free check rest of item size");
                if (!ItemFits(row,col,item.data.size.x, item.data.size.y))
                {
                    Debug.Log("Spot " + row + "," + col + " did not fit");
                    continue;
                }
                Debug.Log("Spot " + row + "," + col + " fits!");
                PlaceAtSpot(row,col,item);
                return;
            }
        }
        
    }

    private void PlaceAtSpot(int row, int col, UIItem item)
    {
        for (int k = 0; k < item.data.size.x; k++)
        {
            for (int l = 0; l < item.data.size.y; l++)
            {
                //Debug.Log("Object "+item.data.itemName+" occupy ("+(row+k)+","+(col+l)+")");
                grid[row + k, col + l] = item;
            }
        }
        item.SetHomePosition(gridTiles[row, col].localPosition);
    }

    private bool ItemFits(int row, int col, int x, int y)
    {
        // Outside
        if (row + x - 1 >= grid.GetLength(0) || col + y - 1 >= grid.GetLength(1)) return false;
        Debug.Log("Object fits inside because ending at "+(row+x-1)+","+ (col + y - 1));
        Debug.Log("size is  "+grid.GetLength(0)+","+ grid.GetLength(1));
        for (int k = 0; k < x; k++)
        {
            for (int l = 0; l < y; l++)
            {
                if (grid[row+k,col+l]!=null) 
                    return false;
            }     
        }
        return true;
    }
}
