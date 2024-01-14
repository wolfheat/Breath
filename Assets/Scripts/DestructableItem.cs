using UnityEngine;
public class DestructableItem : Item
{
    public DestructableData Data;

    public override void InteractWith()
    {
        Debug.Log("Interact with this destructable: " + Data.itemName);

        ItemDestructEffect.Instance.PlayTypeAt(ParticleType.Small,transform.position);

        foreach (Resource r in Data.resources)
            ItemCreator.Instance.InstantiateTypeAt(r, transform.position);

        Destroy(gameObject);
    }

}

