using UnityEngine;

[CreateAssetMenu(menuName = "Items/ResourceDataAMount", fileName = "ResourceAmount")]
public class ResourceDataAmount : ResourceData
{
    public override ItemType itemType { get; } = ItemType.ResourceAmount;
    public int amount = 1;
}
