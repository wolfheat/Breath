using System;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    public void UpdateInventory()
    {
        Debug.Log("Update inventory");
    }
}
