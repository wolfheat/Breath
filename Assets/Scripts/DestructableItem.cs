using UnityEngine;
public class DestructableItem : Item
{
    public DestructableData Data;

    public override void InteractWith()
    {
        Debug.Log("Interact with this destructable: " + Data.itemName);
        // For now destroy later use pooling
        Destroy(gameObject);
    }
}

