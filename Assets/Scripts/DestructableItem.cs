using System;
using UnityEngine;

public class DestructableItem : Interactable
{
    public DestructableData Data;
    public override int Type => Data.Type;

    public override void InteractWith()
    {
        // Play Particle effect when destroying item
        ParticleEffects.Instance.PlayTypeAt(ParticleType.Small,transform.position);

        // Spawn resources contained inside the item
        foreach (var resource in Data.resources)
            ItemCreator.Instance.InstantiateTypeAtRandomSpherePos(resource, transform.position);

        // No need to pool, items are never created again
        Destroy(gameObject);
    }
}

