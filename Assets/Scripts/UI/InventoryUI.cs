using System;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] TextMeshProUGUI[] texts;
    public void UpdateInventory()
    {
        int[] resources = inventory.GetResources();
        Debug.Log("Update inventory");
        for(int i=0; i<resources.Length; i++) {
            texts[i].text = resources[i].ToString();
        }
    }
}
