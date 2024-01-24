using UnityEngine;
public class PickableItem : Item
{
    public ItemData Data;

    public override void InteractWith()
    {
        Debug.Log("interacting with Pickable: " + Data.itemName);
        // For now destroy later use pooling
        Destroy(gameObject);
    }
}
