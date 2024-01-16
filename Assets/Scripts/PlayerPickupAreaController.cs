using System;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class PlayerPickupAreaController : MonoBehaviour
{
    public Interactable ActiveInteractable { get; private set; }
    [SerializeField] Player player;
    [SerializeField] UIController uIController;

    List<Interactable> items = new List<Interactable>();

    private void FixedUpdate()
    {
        if (items.Count > 0)
        {
            SelectClosest();
        }
    }

    public bool InteractWithActiveItem()
    {
        if(ActiveInteractable == null) return false;
        
        ActiveInteractable.InteractWith();
        if(items.Contains(ActiveInteractable))
            items.Remove(ActiveInteractable);
        ActiveInteractable = null;
        SelectClosest();
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null)
        {
            Interactable newItem = other.gameObject.GetComponent<Interactable>();
            if (!items.Contains(newItem)) 
                items.Add(newItem);
            SelectClosest();
        }
    }
    private void SelectClosest()
    {
        if (items.Count == 0)
        {
            ActiveInteractable = null;
            SetSelected(null);
            return;
        }
        // Solution checks distance between an item and its associated point thats placed the same distance along the forward direction

        Interactable closest = items[0];
        Vector3 itemDirection = items[0].transform.position - player.transform.position;
        Vector3 pointOnSightLine = itemDirection.magnitude * transform.forward;
        float distanceFromSight = (itemDirection- pointOnSightLine).magnitude;
        //float distanceFromSight = Math.Abs(Vector3.Dot(itemDirection, transform.forward));
        float bestDistance = distanceFromSight;
        //Debug.Log("Item 0 distance: " + distanceFromSight);
        if (items.Count > 1)
        {
            for (int i = 1; i < items.Count; i++)
            {
                itemDirection = items[i].transform.position - player.transform.position;
                pointOnSightLine = itemDirection.magnitude * transform.forward;
                distanceFromSight = (itemDirection - pointOnSightLine).magnitude;
                //distanceFromSight = Math.Abs(Vector3.Dot(itemDirection, transform.forward));
                // Calculate the distance from the player to the infinite line

                //Debug.Log("Item " + i + " distance: " + distanceFromSight);
                if (distanceFromSight < bestDistance)
                {
                    closest = items[i];
                    bestDistance = distanceFromSight;
                }
            }
        }
        SetSelected(closest);        
        return;

    }

    private void SetSelected(Interactable closest)
    {

        bool same = ActiveInteractable == closest;
        ActiveInteractable = closest;
        if (ActiveInteractable != null)
        {
            if (!same)
            {
                ItemSelector.Instance.SetToPosition(closest.transform);
                // Determin screen position here

                if(ActiveInteractable is PickableItem)
                    uIController.ShowHUDIconAt(HUDIconType.PickUp, ActiveInteractable);
                else if(ActiveInteractable is DestructableItem)
                    uIController.ShowHUDIconAt(HUDIconType.LeftClick, ActiveInteractable);
                else if(ActiveInteractable is Facility)
                    uIController.ShowHUDIconAt(HUDIconType.Interact, ActiveInteractable);
                else
                    uIController.ShowHUDIconAt(HUDIconType.Generic, ActiveInteractable);
            }
        }
        else
        {
            ItemSelector.Instance.Disable();
            uIController.HideHUDIcon();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null)
        {
            Interactable item = other.gameObject.GetComponent<Interactable>();
            if (items.Contains(item))
                items.Remove(item);            
            StartCoroutine(player.ResetItemCollider());
            SelectClosest();
        }
    }
}
