using UnityEngine;

public class DoorThrower : MonoBehaviour
{
    public DoorController controller;
    public DoorThrower target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            Debug.Log("Player entered a thrower, disable both throwers");
            controller.SetThrowTriggersActivation(false);
            other.gameObject.GetComponent<PlayerMovement>().ThrowPlayer(this);
        }
    }
}
