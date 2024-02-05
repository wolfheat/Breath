using UnityEngine;

public class DestructableItem : Interactable
{
    public DestructableData Data;
    public override int Type => Data.Type;

    public override void InteractWith()
    {
        Debug.Log("Interact with this destructable: " + Data.itemName);

        ItemDestructEffect.Instance.PlayTypeAt(ParticleType.Small,transform.position);

        foreach (Resource r in Data.resources)
            ItemCreator.Instance.InstantiateTypeAtRandomSpherePos(r, transform.position);

        Destroy(gameObject);
    }

}

