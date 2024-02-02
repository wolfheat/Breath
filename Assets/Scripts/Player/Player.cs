using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Wolfheat.StartMenu;
using static UnityEngine.InputSystem.InputAction;
public class Player : MonoBehaviour
{
    [SerializeField] UIController uiController;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerStats playerHealth;
    [SerializeField] PlayerShootController playerShootController;
    [SerializeField] Inventory inventory;
    [SerializeField] PlayerPickupAreaController pickupController;
    [SerializeField] Collider itemCollider;
    [SerializeField] ToolHolder toolHolder;
    // Free movement of camera with sensitivity

    [SerializeField] PlayerAnimationController playerAnimationController;


    private void Start()
    {
        Debug.Log("Created Player");
        // set up input actions
        Inputs.Instance.Controls.Player.Click.performed += Click;
        Inputs.Instance.Controls.Player.Click.started += MouseDown;
        Inputs.Instance.Controls.Player.E.performed += InterractWith;       

    }

    
    public void InterractWith(CallbackContext context)
    {
        Debug.Log("E - Pick up nearby item or interact ");
        // Interact with closest visible item 
        if(pickupController.ActiveInteractable != null)
        {
            if (pickupController.ActiveInteractable is Facility)
            {
                pickupController.ActiveInteractable.InteractWith();
            }
            else if (pickupController.ActiveInteractable is PickableItem)
            {
                Debug.Log("Interactable is " + pickupController.ActiveInteractable.name);
                PickableItem item = (PickableItem)pickupController.ActiveInteractable;
                Debug.Log("Pickable is "+ item.name);

                bool didPickUp = inventory.AddItem(item);
                if (didPickUp)
                {
                    pickupController.InteractWithActiveItem();
                    SoundMaster.Instance.PlaySound(SoundName.PickUp);
                    Debug.Log("Did Pick Up = " + didPickUp);
                }
                else
                    Debug.Log("Could Not Pick Up "+item.Data.itemName);

                StartCoroutine(ResetItemCollider());
            }
            // No active item here
        }
        else
        {
            Debug.Log("This is not pickable");
        }
    }

    private bool LimitShots = false;
    private void MouseDown(CallbackContext context)
    {
        if (pickupController.ActiveInteractable != null && pickupController.ActiveInteractable is DestructableItem)
        {
            LimitShots = true;
        }
        else
        {
            LimitShots = false;
        }
    }
    
    private void Update()
    {
        if (!Inputs.Instance.Controls.Player.MouseHeld.IsPressed() || LimitShots) return;

        // Detect if there is an object to interact with in front of player
        if (pickupController.ActiveInteractable != null && pickupController.ActiveInteractable is DestructableItem) return;

        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("Clicked on the UI");
            return;
        }


        //Try shoot if having a gun equipped
        if (inventory.PlayerHasEquipped(DestructType.Flesh))
        {
            if (playerShootController.RequestShoot())
            {
                //playerAnimationController.SetState(PlayerState.Hit);
                toolHolder.ChangeTool(DestructType.Flesh);
            }

        }
    }
    public void Click(CallbackContext context)
    {
        // Raycast from mouse into screen get first item thats interactable
                
        // Detect what tool to use
        if (pickupController.ActiveInteractable != null)        
        {
            
            // Check what itemtype it is and if player has the tool
            if(pickupController.ActiveInteractable is DestructableItem)
            {

                DestructableItem destructable = pickupController.ActiveInteractable as DestructableItem;

                // Check what type the object is and if player has the tool
                if (inventory.PlayerHasEquipped(destructable.Data.destructType))
                {
                    Debug.Log("Player can break this object");
                }
                else
                {
                    HUDMessage.Instance.ShowMessage("Equip a "+ Enum.GetName(typeof(EquipType), (int)destructable.Data.destructType+5));
                    return;
                }

                if (destructable.Data.destructType == DestructType.Breakable)
                {
                    Debug.Log("Is Breakable change to hammer");
                    SoundMaster.Instance.PlaySound(SoundName.HitMetal);
                    playerAnimationController.SetState(PlayerState.Hit);
                    toolHolder.ChangeTool(DestructType.Breakable);
                }
                else if (destructable.Data.destructType == DestructType.Drillable)
                {
                    Debug.Log("Is Drillable change to drill");
                    SoundMaster.Instance.PlaySound(SoundName.Drill);
                    playerAnimationController.SetState(PlayerState.Drill);
                    toolHolder.ChangeTool(DestructType.Drillable);
                }

                pickupController.InteractWithActiveItem();

                StartCoroutine(ResetItemCollider());
            }


        }
        else
        {
            
        }
        
        


    }

    public IEnumerator ResetItemCollider()
    {
        itemCollider.enabled = false;
        yield return null;
        itemCollider.enabled = true;
    }

    public void Reset()
    {
        playerHealth.Reset();
        playerMovement.SetToSafePoint();        
    }
}
