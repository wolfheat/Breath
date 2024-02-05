using UnityEngine;

public enum FacilityType{Workbench,Storage}
public class Facility : Interactable
{
    public override int Type { get; }
    public override void InteractWith()
    {
        Debug.Log("Interact with facility");
    }
}
