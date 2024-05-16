using UnityEngine;

public class GravityObject : MonoBehaviour
{
    private Rigidbody rb;

    void Start() => rb = GetComponent<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        //Entering Gravity area
        if (other.gameObject.GetComponent<GravityArea>() != null)
        {
            if(!rb)
                rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //Exiting Gravity area
        if (other.gameObject.GetComponent<GravityArea>() != null)
            rb.useGravity = false;
    }



}
