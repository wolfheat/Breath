using TMPro;
using UnityEngine;
public class InventoryUI : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] TextMeshProUGUI[] texts;
    [SerializeField] TextMeshProUGUI[] itemNames;
    [SerializeField] InventoryItem inventoryItemPrefab;
    [SerializeField] GameObject inventoryItemHolder;
    [SerializeField] ResourceData[] itemDatas;

    private void Start()
    {
        // Remove all older 
        RemoveExistingUI();

        // Create from the Data array
        CreateInventory();

        // Update
        UpdateInventory();
    }

    private void CreateInventory()
    {
        texts = new TextMeshProUGUI[itemDatas.Length];

        for (int i = 0; i < itemDatas.Length; i++)
        {
            InventoryItem item = Instantiate(inventoryItemPrefab, inventoryItemHolder.transform);
            item.SetItem(itemDatas[i].picture, "0", itemDatas[i].itemName);
            texts[i] = item.textField;
        }
    }

    private void RemoveExistingUI()
    {
        foreach (Transform child in inventoryItemHolder.transform)
            Destroy(child.gameObject);
    }

    public void UpdateInventory()
    {
        int[] resources = inventory.GetResources();

        // Name the resources in the UI
        for(int i=0; i<texts.Length; i++) {
            if (i >= resources.Length) break;
            texts[i].text = resources[i].ToString();
        }
    }
}
