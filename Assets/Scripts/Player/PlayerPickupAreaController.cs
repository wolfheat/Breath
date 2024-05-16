using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupAreaController : MonoBehaviour
{
    public Interactable ActiveInteractable { get; private set; }
    [SerializeField] Player player;
    [SerializeField] UIController uIController;

    List<Interactable> items = new List<Interactable>();

    // Pickup controller is constantly checking for the closest item and setting the item selector to this item

    private void FixedUpdate()
    {
        if (items.Count > 0)
            SelectClosest();
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

    private void OnTriggerStay(Collider other)
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
        float bestDistance = distanceFromSight;

        if (items.Count > 1)
        {
            for (int i = 1; i < items.Count; i++)
            {
                itemDirection = items[i].transform.position - player.transform.position;
                pointOnSightLine = itemDirection.magnitude * transform.forward;
                distanceFromSight = (itemDirection - pointOnSightLine).magnitude;

                // Calculate the distance from the player to the infinite line in the forward direction
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
            if (same) return;

            // Determine HUD Icons screen position
            ItemSelector.Instance.SetToPosition(closest.transform);
            uIController.ShowHUDIconAt(HUDIconType.LeftClick, ActiveInteractable);
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
            SelectClosest();
        }
    }
}
