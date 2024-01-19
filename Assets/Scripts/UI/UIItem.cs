using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIItem : MonoBehaviour,IDragHandler,IEndDragHandler, IBeginDragHandler
{
    public ItemData data;
    [SerializeField] Image image;
    [SerializeField] RectTransform rect;
    private const int TileSize = 86;
    private const int TileSpace = 4;
    public Vector2 homePosition = new Vector2();
    public Vector2Int spot = new Vector2Int();
    private InventoryGrid inventoryGrid;

    private void Start()
    {
        inventoryGrid = FindObjectOfType<InventoryGrid>();
    }

    public void UpdatePosition()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, Camera.main, out position);
        transform.position = rect.TransformPoint(position);
    }

    public void SetHomePositionAndSpot(Vector2 pos,Vector2Int spotIn)
    {
        spot = spotIn;
        SetHomePosition(pos);
    }
    
    public void SetHomePosition(Vector2 pos)
    {
        Debug.Log("Item local home position set "+pos);
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
        Debug.Log("Item position reset");
        transform.localPosition = homePosition; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position+ offset;
    }

    public void SetParent(Transform p)
    {
        transform.SetParent(p);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        // Check if dropped position is a valid spot
        Vector2 drop = eventData.position + offset;
        Debug.Log("Dropping item at "+ drop);


        inventoryGrid.RequestMove(this,drop);
        DragObject.Instance.UnSetDragedItem();

    }

    Vector2 offset = new Vector2();
    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = (Vector2)transform.position-eventData.position;
        Debug.Log("Started Dragging object");
        DragObject.Instance.SetDragedItem(this);
    }

    internal bool IsInInventory()
    {
        return spot!=new Vector2Int(-1,-1);
    }
}
