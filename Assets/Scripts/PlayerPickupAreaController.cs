using System;
using UnityEngine;

public class PlayerPickupAreaController : MonoBehaviour
{
    public PickableItem ActiveItem { get; private set; }
    [SerializeField] Player player;

    public bool PickUpActiveItem()
    {
        if(ActiveItem == null) return false;
        
        ActiveItem.PickUp();
        ActiveItem = null;
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PickableItem>() != null)
        {
            ActiveItem = other.gameObject.GetComponent<PickableItem>();
            ActiveItem.SetAsSelected(true);
            Debug.Log("Collided with pickable item?");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PickableItem>() != null)
        {
            if(ActiveItem == other.gameObject.GetComponent<PickableItem>())
            {
                ActiveItem.SetAsSelected(false);
                ActiveItem = null;
            }
            other.gameObject.GetComponent<PickableItem>().SetAsSelected(false);
            Debug.Log("Leaving collision with pickable item?");
            StartCoroutine(player.ResetItemCollider());
        }
    }
}
