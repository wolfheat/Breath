using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public DoorController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            controller.OpenDoors(true);            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            controller.OpenDoors(false);
        }
    }
}

