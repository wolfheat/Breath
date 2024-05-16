using UnityEngine;

public class DoorThrower : MonoBehaviour
{
    public DoorController controller;
    public DoorThrower target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            controller.SetThrowTriggersActivation(false);
            playerMovement.ThrowPlayer(this);
        }
    }
}
