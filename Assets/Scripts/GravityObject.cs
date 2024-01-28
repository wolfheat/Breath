using UnityEngine;

public class GravityObject : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Entering Gravity area
        if (other.gameObject.GetComponent<GravityArea>() != null)
        {
            rb.useGravity = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //Entering Gravity area
        if (other.gameObject.GetComponent<GravityArea>() != null)
        {
            rb.useGravity = false;
        }
    }



}
