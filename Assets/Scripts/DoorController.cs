using UnityEngine;
using UnityEngine.InputSystem;

public class DoorController : MonoBehaviour
{
    [SerializeField] Door leftdoor;
    [SerializeField] Door rightdoor;
    [SerializeField] bool useScript;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Player>() != null)
        {
            OpenDoors(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            OpenDoors(false);
        }
    }
    public void OpenDoors(bool open)
    {
        if (!useScript) return;
        leftdoor.OpenDoor(open);   
        rightdoor.OpenDoor(open);   
    }
}

