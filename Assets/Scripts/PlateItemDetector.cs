using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateItemDetector : MonoBehaviour
{
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
        {
            Debug.Log("Checking collider for interactable: "+collider.gameObject.layer+" "+collider.gameObject.name);
            if(collider.gameObject.GetComponent<PickableItem>()!=null)
                return true;
        }
        return false;
    }
}
