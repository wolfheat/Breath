using System;
using System.Collections;
using UnityEngine;

public class Workbench : Facility
{
    private CraftingUI craftingUI;
    private ToggleMenu craftingMenu;
    [SerializeField] private ParticleSystem craftingEffects;
    [SerializeField] private PickableItem genericPrefab;
    [SerializeField] private GameObject craftingPoint;

    private bool isCrafting = false;
    private bool HasItem { get { return craftingPoint.transform.childCount > 0;} }

    private void OnEnable()
    {
        craftingUI = FindObjectOfType<CraftingUI>();
        craftingMenu = craftingUI.GetComponent<ToggleMenu>();
    }
    public override void InteractWith()
    {
        if (isCrafting)
        {
            Debug.Log("Can not interact with workbench, crafting item!");
            HUDMessage.Instance.ShowMessage("Workbench is busy");
            return;
        }else if (HasItem)
        {
            Debug.Log("Can not interact with workbench, item on plate!");
            HUDMessage.Instance.ShowMessage("Remove item before crafting");
            return;
        }
        // if(!craftingMenu.IsActive) // Used to make E only open the menu not close it, changed it so it can be both opened and closed with E
        craftingUI.SetActiveWorkbench(this);
        craftingMenu.Toggle();
    }

    public void CraftItem(ItemData itemData)
    {
        Debug.Log("Workbench is now crafting "+itemData.itemName);
        craftingEffects.gameObject.SetActive(true);
        craftingEffects.Play();
        isCrafting = true;
        StartCoroutine(CraftingDelay(itemData));
    }

    private const float CraftingTime = 2f;
    private IEnumerator CraftingDelay(ItemData itemData)
    {
        yield return new WaitForSeconds(CraftingTime);

        Debug.Log("Crafting Complete");
        CraftCompleted();


        Debug.Log("Placing Item "+itemData.itemName+" ont the plate");
        PickableItem item = Instantiate(genericPrefab, craftingPoint.transform);
        item.Data = itemData;
    }

    public void CraftCompleted()
    {
        isCrafting =false;
        craftingEffects.Stop();
        craftingEffects.gameObject.SetActive(false);
        
    }
}
