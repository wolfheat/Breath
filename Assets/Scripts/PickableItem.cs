using UnityEngine;
public class PickableItem : Interactable
{
    public ItemData Data;
    
    public override int Type => Data.Type;

    public override void InteractWith()
    {
        Debug.Log("interacting with Pickable: " + Data.itemName);
        // For now destroy later use pooling
        Destroy(gameObject);
    }
}
