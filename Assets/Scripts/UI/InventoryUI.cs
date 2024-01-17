using TMPro;
using UnityEngine;
public class InventoryUI : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] TextMeshProUGUI[] texts;
    [SerializeField] InventoryItem inventoryItemPrefab;
    [SerializeField] GameObject inventoryItemHolder;
    [SerializeField] ResourceData[] itemDatas;

    private void Start()
    {
        // remove all older 
        foreach(Transform child in inventoryItemHolder.transform)
            Destroy(child.gameObject);

        texts = new TextMeshProUGUI[itemDatas.Length];
        //Create inventory
        for (int i=0; i< itemDatas.Length; i++)
        {
            InventoryItem item = Instantiate(inventoryItemPrefab, inventoryItemHolder.transform);
            item.image.sprite = itemDatas[i].picture;
            texts[i] = item.textField;
        }
        UpdateInventory();
    }
    public void UpdateInventory()
    {
        int[] resources = inventory.GetResources();
        Debug.Log("Update inventory");
        for(int i=0; i<texts.Length; i++) {
            if (i >= resources.Length) break;
            texts[i].text = resources[i].ToString();
        }
    }
}
