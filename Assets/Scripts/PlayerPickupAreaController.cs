using System;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class PlayerPickupAreaController : MonoBehaviour
{
    public Item ActiveItem { get; private set; }
    [SerializeField] Player player;

    List<Item> items = new List<Item>();

    private void FixedUpdate()
    {
        if (items.Count > 0)
        {
            SelectClosest();
        }
    }

    public bool InteractWithActiveItem()
    {
        if(ActiveItem == null) return false;
        
        ActiveItem.InteractWith();
        if(items.Contains(ActiveItem))
            items.Remove(ActiveItem);
        ActiveItem = null;
        SelectClosest();
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Item>() != null)
        {
            Item newItem = other.gameObject.GetComponent<Item>();
            if (!items.Contains(newItem)) 
                items.Add(newItem);
            SelectClosest();
        }
    }
    private void SelectClosest()
    {
        if (items.Count == 0)
        {
            ActiveItem = null;
            SetSelected(null);
            return;
        }
        // Solution checks distance between an item and its associated point thats placed the same distance along the forward direction

        Item closest = items[0];
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

    private void SetSelected(Item closest)
    {

        bool same = ActiveItem == closest;
        ActiveItem = closest;
        if (ActiveItem != null)
        {
            if(!same)
                ItemSelector.Instance.SetToPosition(closest.transform);
        }
        else
        {
            ItemSelector.Instance.Disable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Item>() != null)
        {
            Item item = other.gameObject.GetComponent<Item>();
            if (items.Contains(item))
                items.Remove(item);            
            StartCoroutine(player.ResetItemCollider());
            SelectClosest();
        }
    }
}
