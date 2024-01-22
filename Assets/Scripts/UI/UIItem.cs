using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;


public class UIItem : MonoBehaviour,IDragHandler,IEndDragHandler, IBeginDragHandler, IPointerClickHandler
{
    public ItemData data;
    [SerializeField] Image image;
    [SerializeField] RectTransform rect;
    private const int TileSize = 86;
    private const int TileSpace = 4;
    public Vector2 homePosition = new Vector2();

    public Vector2Int Spot { get; private set; } = new Vector2Int(-1, -1);

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
        Debug.Log("Setting spot to "+spotIn);
        Spot = spotIn;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            inventoryGrid.RequestEquip(this);
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
            Debug.Log("Middle clicking "+data.itemName);
        else
            Debug.Log("Clicking " + data.itemName);

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

    public bool IsInInventory()
    {
        return Spot != new Vector2Int(-1,-1);
    }

}
