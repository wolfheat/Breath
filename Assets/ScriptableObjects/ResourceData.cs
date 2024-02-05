using UnityEngine;

[CreateAssetMenu(menuName = "Items/ResourceData", fileName = "Resource")]
public class ResourceData : ItemData
{
    public override ItemType itemType { get; } = ItemType.Resource;
    public override int Type => (int)resource;

    public Resource resource;
}
