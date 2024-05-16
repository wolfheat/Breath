using UnityEngine;

public class PlateItemDetector : MonoBehaviour
{
    // Detects if an item is on top of the creation plate prohibiting initiation af a new item creation
    public bool HasItem { get {return CheckCollider(); }}
    [SerializeField] private GameObject colliderDefinition;
    private float radius;

    private void Start()
    {
        radius = transform.localScale.x;
    }

    private bool CheckCollider()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
            if(collider.gameObject.GetComponent<PickableItem>()!=null)
                return true;
        return false;
    }
}
