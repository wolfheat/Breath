using System.Collections;
using UnityEngine;
using Wolfheat.StartMenu;

public class Workbench : Facility
{
    private CraftingUI craftingUI;
    private ToggleMenu craftingMenu;
    [SerializeField] private PickableItem genericPrefab;
    [SerializeField] private GameObject craftingPoint;
    [SerializeField] private PlateItemDetector plateItemDetector;
    private FacilityType FacilityType = FacilityType.Workbench;
    public override int Type => (int)FacilityType;

    private bool isCrafting = false;
    public bool IsCrafting { get { return isCrafting;} }

    private void OnEnable()
    {
        craftingUI = FindObjectOfType<CraftingUI>();
        craftingMenu = craftingUI.GetComponent<ToggleMenu>();
    }
    public override void InteractWith()
    {
        Debug.Log("Interact with workbench!");

        if (isCrafting)
        {
            Debug.Log("Can not interact with workbench, crafting item!");
            HUDMessage.Instance.ShowMessage("Workbench is busy");
            return;
        }else if (plateItemDetector.HasItem)
        {
            Debug.Log("Can not interact with workbench, item on plate!");
            HUDMessage.Instance.ShowMessage("Remove item before crafting");
            return;
        }

        craftingUI.SetActiveWorkbench(this);
        craftingUI.ToggleCraftingMenu();

    }

    public void CraftItem(ItemData itemData)
    {
        Debug.Log("Workbench is now crafting "+itemData.itemName);
        ParticleEffects.Instance.PlayTypeAt(ParticleType.Creation, craftingPoint.transform.position);
        isCrafting = true;
        StartCoroutine(CraftingDelay(itemData));
    }

    private const float CraftingTime = 2f;
    private IEnumerator CraftingDelay(ItemData itemData)
    {

        SoundMaster.Instance.PlaySound(SoundName.Crafting);
        yield return new WaitForSeconds(0.2f);
        //SoundMaster.Instance.PlaySound(SoundName.CraftingB);
        yield return new WaitForSeconds(CraftingTime);

        SoundMaster.Instance.PlaySound(SoundName.CraftComplete);

        Debug.Log("Crafting Complete");
        CraftCompleted();
        ItemCreator.Instance.InstantiateGenericItemAt(itemData,craftingPoint.transform.position,craftingPoint.transform.rotation);
    }

    public void CraftCompleted()
    {
        isCrafting =false;
    }
}
