using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] Door leftdoor;
    [SerializeField] Door rightdoor;
    [SerializeField] bool useScript;
    [SerializeField] DoorThrower[] doorThrowers;


    public void SetThrowTriggersActivation(bool active)
    {
        foreach (var thrower in doorThrowers)
            thrower.gameObject.SetActive(active);
    }

    public void OpenDoors(bool open)
    {
        SetThrowTriggersActivation(open);

        if (!useScript) return;
        leftdoor.OpenDoor(open);   
        rightdoor.OpenDoor(open);   
    }
}

