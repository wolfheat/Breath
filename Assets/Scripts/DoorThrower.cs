using UnityEngine;

public class DoorThrower : MonoBehaviour
{
    public DoorController controller;
    public DoorThrower target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            controller.SetThrowTriggersActivation(false);
            other.gameObject.GetComponent<Player>().ThrowPlayer(this);
        }
    }
}
