using UnityEngine;

[CreateAssetMenu(menuName = "Items/ResourceData", fileName = "Resource")]
public class ResourceData : ItemData
{
    public override ItemType itemType { get; } = ItemType.Resource;

    public Resource resource;
}
