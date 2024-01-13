using UnityEngine;
public class DestructableItem : Item
{
    public DestructableData Data;

    public override void InteractWith()
    {
        Debug.Log("Interact with this destructable: " + Data.itemName);

        // Create the object contained
        foreach (Resource r in Data.resources)
            ItemCreator.Instance.InstantiateTypeAt(r,transform.position);

        // For now destroy later use pooling
        Destroy(gameObject);
    }
}

