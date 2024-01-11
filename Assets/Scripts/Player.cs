using System.Collections;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static UnityEngine.InputSystem.InputAction;
public class Player : MonoBehaviour
{
    [SerializeField] UIController uiController;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Inventory inventory;
    [SerializeField] PlayerPickupAreaController pickupController;
    [SerializeField] Collider itemCollider;
    // Free movement of camera with sensitivity

    [SerializeField] PlayerAnimationController playerAnimationController;


    private void Start()
    {
        Debug.Log("Created Player");
        // set up input actions
        Inputs.Instance.Controls.Player.Click.performed += Click;
        Inputs.Instance.Controls.Player.E.performed += EPickup;       

    }

    
    public void EPickup(CallbackContext context)
    {
        Debug.Log("E - Pick up nearby item or interact");
        // Interact with closest visible item 
        if(pickupController.ActiveItem != null)
        {
            inventory.AddItem(pickupController.ActiveItem);
            
            bool didPickUp = pickupController.PickUpActiveItem();
            Debug.Log("Did Pick Up = "+didPickUp);
            if (didPickUp)
                SoundMaster.Instance.PlaySFX(SoundMaster.SFX.PickUp);

            StartCoroutine(ResetItemCollider());
            // No active item here
        }
    }

    public void Click(CallbackContext context)
    {
        Debug.Log("CLICK");
        // Raycast from mouse into screen get first item thats interactable

        // If holding shift
        if (Inputs.Instance.Controls.Player.Shift.ReadValue<float>() != 0)
            playerAnimationController.SetState(PlayerState.Drill);
        else
            playerAnimationController.SetState(PlayerState.Hit);


    }

    public IEnumerator ResetItemCollider()
    {
        itemCollider.enabled = false;
        yield return null;
        itemCollider.enabled = true;
    }
}
