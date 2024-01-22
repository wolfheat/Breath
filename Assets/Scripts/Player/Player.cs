using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
public class Player : MonoBehaviour
{
    [SerializeField] UIController uiController;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerHealth playerHealth;
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
        Inputs.Instance.Controls.Player.E.performed += InterractWith;       

    }

    
    public void InterractWith(CallbackContext context)
    {
        Debug.Log("E - Pick up nearby item or interact");
        // Interact with closest visible item 
        if(pickupController.ActiveInteractable != null)
        {
            if (pickupController.ActiveInteractable is PickableItem)
            {
                inventory.AddItem(pickupController.ActiveInteractable as PickableItem);
            
                bool didPickUp = pickupController.InteractWithActiveItem();
                Debug.Log("Did Pick Up = "+didPickUp);
                if (didPickUp)
                    SoundMaster.Instance.PlaySFX(SoundMaster.SFX.PickUp);

                StartCoroutine(ResetItemCollider());
            }
            else if (pickupController.ActiveInteractable is Facility)
            {
                pickupController.ActiveInteractable.InteractWith();
            }
            // No active item here
        }
        else
        {
            Debug.Log("This is not pickable");
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

                if (destructable.Data.destructType == DestructType.Breakable)
                {
                    SoundMaster.Instance.PlaySFX(SoundMaster.SFX.HitMetal);
                    playerAnimationController.SetState(PlayerState.Hit);
                    toolHolder.ChangeTool(DestructType.Breakable);
                }
                else if (destructable.Data.destructType == DestructType.Drillable)
                {
                    SoundMaster.Instance.PlaySFX(SoundMaster.SFX.Drill);
                    playerAnimationController.SetState(PlayerState.Drill);
                    toolHolder.ChangeTool(DestructType.Drillable);
                }

                pickupController.InteractWithActiveItem();

                StartCoroutine(ResetItemCollider());
            }
            
            
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
