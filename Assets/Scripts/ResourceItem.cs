using UnityEngine;

public class ResourceItem : PickableItem
{
    public override int Type => (Data as ResourceData).Type;

    public override void InteractWith()
    {
        Debug.Log("Interact with this resourceItem: " + (Data as ResourceData).itemName);

        ItemDestructEffect.Instance.PlayTypeAt(ParticleType.PickUp,transform.position);

        Destroy(gameObject);
    }

}

