using UnityEngine;
public class PickableItem : Item
{
    public ItemData Data;

    public void SetAsSelected(bool set)
    {
        Debug.Log("Setting as selected item");
        if(set)
            ItemSelector.Instance.SetToPosition(this.transform);
        else
            ItemSelector.Instance.Disable();

    }
    public void PickUp()
    {
        Debug.Log("Picked up item is: " + Data.itemName);
        ItemSelector.Instance.Disable();
        // For now destroy later use pooling
        Destroy(gameObject);
    }
}
