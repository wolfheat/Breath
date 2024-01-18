using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DragObject : MonoBehaviour
{
    public UIItem draggedItem;

    List<RaycastResult> rayCastResults;
    PointerEventData rayCastData;
    [SerializeField] GraphicRaycaster raycaster;


    private void Start()
    {
        Inputs.Instance.Controls.Player.Drag.performed += Drag;
        Inputs.Instance.Controls.Player.Click.started += StartDrag;
        Inputs.Instance.Controls.Player.Click.canceled += StopDrag;

        rayCastResults = new();
        rayCastData = new PointerEventData(EventSystem.current);
    }

    void Update()
    {
        
    }

    public void StartDrag(InputAction.CallbackContext context)
    {
        Debug.Log("StartDrag:");
        //Determine if an item is clicked on or not
        var rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));
        rayCastData.position = Mouse.current.position.ReadValue();
        rayCastResults.Clear();
        raycaster.Raycast(rayCastData, rayCastResults);
        
        foreach (var ray in rayCastResults)
        {
            Debug.Log("StartDrag Hit "+ray.gameObject.name);
        }

    }
    public void Drag(InputAction.CallbackContext context)
    {
        if (draggedItem != null)
        {
            // Update dragget items position
            draggedItem.UpdatePosition();
        }
    }
    
    public void StopDrag(InputAction.CallbackContext context)
    {
        if (draggedItem == null) return;
        draggedItem.ResetPosition();
        draggedItem = null;
    }


}
