using UnityEngine;

public class Workbench : Facility
{
    public override void InteractWith()
    {
        Debug.Log("Interact with the workbench");
    }
}
