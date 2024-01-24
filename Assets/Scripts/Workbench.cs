using System;
using System.Collections;
using UnityEngine;

public class Workbench : Facility
{
    private CraftingUI craftingUI;
    private ToggleMenu craftingMenu;
    [SerializeField] private ParticleSystem craftingEffects;

    private bool isCrafting = false;
    private bool hasItem = false;

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
            return;
        }else if (hasItem)
        {
            Debug.Log("Can not interact with workbench, item on plate!");
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
        hasItem = true;

    }

    public void CraftCompleted()
    {
        isCrafting =false;
        craftingEffects.Stop();
        craftingEffects.gameObject.SetActive(false);
        
    }
}
