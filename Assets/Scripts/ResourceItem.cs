using UnityEngine;

public class ResourceItem : PickableItem
{
    public override int Type => (Data as ResourceData).Type;

    public const float ResourceSize = 0.1f;
    public override void InteractWith()
    {
        Debug.Log("Interact with this resourceItem: " + (Data as ResourceData).itemName);

        ParticleEffects.Instance.PlayTypeAt(ParticleType.PickUp,transform.position);

        Destroy(gameObject);
    }

}

