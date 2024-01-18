using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;



public class UIItem : MonoBehaviour
{
    public ItemData data;
    [SerializeField] Image image;
    [SerializeField] RectTransform rect;
    private const int TileSize = 86;
    private const int TileSpace = 4;
    private Vector2 homePosition = new Vector2();

    private void Start()
    {
        
    }

    public void UpdatePosition()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, Camera.main, out position);
        transform.position = rect.TransformPoint(position);
    }

    public void SetHomePosition(Vector2 pos)
    {
        homePosition = pos;
        transform.localPosition = homePosition;
    }
    public void SetData(ItemData dataIn)
    {
        data = dataIn;
        UpdateItem();
    }
    private void UpdateItem()
    {
        rect.sizeDelta = new Vector2(data.size.y * TileSize + (data.size.y - 1) * TileSpace, data.size.x * TileSize + (data.size.x - 1) * TileSpace);
        image.sprite = data.picture;
    }

    public void ResetPosition()
    {
        throw new NotImplementedException();
    }

    public void StartDrag(InputAction.CallbackContext context)
    {
        Debug.Log("StartDrag " + gameObject.GetInstanceID()+" "+gameObject.name);
    }
    public void EndDrag(InputAction.CallbackContext context)
    {
        Debug.Log("EndDrag "+gameObject.GetInstanceID());
    }
    public void Drag(InputAction.CallbackContext context)
    {

        if (context.started && context.action.name=="Drag")
        {
            Debug.Log("Button Pressed Down");
            
        }
        else if (context.performed)
        {
           
            //Debug.Log("Drag");
            
        }
        else if (context.canceled)
        {
            
            Debug.Log("Button Released");
           
        }
    }
}
