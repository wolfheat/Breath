using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Wolfheat.StartMenu;

public class UIItem : MonoBehaviour,IDragHandler,IEndDragHandler, IBeginDragHandler, IPointerClickHandler
{
    public ItemData data;
    [SerializeField] Image image;
    [SerializeField] RectTransform rect;
    private const int TileSize = 86;
    private const int TileSpace = 4;
    private Vector2 baseRectSize;
    private Vector2 equippedRectSize;
    private Vector2 currentRectSize;
    public Vector2 homePosition = new Vector2();

    public Vector2 EquippedRectOffset { get; private set; }
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
        //Debug.Log("Setting spot to "+spotIn);
        Spot = spotIn;
        if (Spot == new Vector2Int(-1, -1))
            homePosition = pos+EquippedRectOffset;
            //homePosition = pos+EquippedRectOffset;
        else
            homePosition = pos;
        transform.localPosition = homePosition;

        ResetPosition();
    }

    public void SetData(ItemData dataIn)
    {
        data = dataIn;
        baseRectSize = new Vector2(data.size.y * TileSize + (data.size.y - 1) * TileSpace, data.size.x * TileSize + (data.size.x - 1) * TileSpace);
        SetToBaseRectSize();
    }
    
    public void SetData(ItemData dataIn, Vector2 equippedSize)
    {
        equippedRectSize = new Vector2(equippedSize.x, equippedSize.y);
        EquippedRectOffset = new Vector2(-equippedSize.x / 2, equippedSize.y / 2);
        if(dataIn.itemName == "Space Helmet")
        {
            Debug.Log("Set data for "+dataIn.itemName+" offset is "+EquippedRectOffset);
        }
        SetData(dataIn);
    }

    private void SetToBaseRectSize()
    {
        currentRectSize = baseRectSize;
        UpdateItem();
    }

    private void DetermineRectSize()
    {
        //Check if equipped
        if(Spot == new Vector2Int(-1,-1))
            currentRectSize = equippedRectSize;
        else
            currentRectSize = baseRectSize;
    }
    private void UpdateItem()
    {
        rect.sizeDelta = currentRectSize;
        image.sprite = data.picture;
    }

    public void ResetPosition()
    {
        //Debug.Log("Item position reset");
        transform.localPosition = homePosition;
        DetermineRectSize();
        UpdateItem();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left && Inputs.Instance.Controls.Player.Shift.IsPressed())
        {
            Debug.Log("Request Drop this item");
            inventoryGrid.DropItem(this);
            return;
        }
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            // Limit player from clicking on the switched item in same click unsetting a swap
            
            if(data.itemType == ItemType.Equipable)
            {

                if (inventoryGrid.ClickTimerLimited)
                    return;
                StartCoroutine(inventoryGrid.ClickTimerLimiter());
                inventoryGrid.RequestEquip(this);
            }
            else if(data.itemType == ItemType.Consumable)
            {
                if (PlayerStats.Instance.AtMaxHealth)
                {
                    HUDMessage.Instance.ShowMessage("Already at max health!");
                    return;
                }
                ConsumableData consumeData = (ConsumableData)data;
                Debug.Log("Consumed "+data.itemType);
                PlayerStats.Instance.Consume(consumeData);
                // Try consume
                inventoryGrid.RemoveFromInventory(this);
                HUDMessage.Instance.ShowMessage("You regained some health!",false,SoundName.HUDPositive);
            }

        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
            Debug.Log("Middle clicking "+data.itemName);

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        transform.position = eventData.position+ offset;
        SetToBaseRectSize();
    }

    public void SetParent(Transform p)
    {
        Debug.Log("Setting parent for "+name);
        transform.SetParent(p);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        // Check if dropped position is a valid spot
        Vector2 drop = eventData.position + offset;
        //Debug.Log("Dropping item at "+ drop);


        inventoryGrid.RequestMove(this,drop);
        DragObject.Instance.UnSetDragedItem();

    }

    Vector2 offset = new Vector2();
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        offset = (Vector2)transform.position-eventData.position;
        //Debug.Log("Started Dragging object");
        DragObject.Instance.SetDragedItem(this);
    }

    public bool IsInInventory()
    {
        return Spot != new Vector2Int(-1,-1);
    }

    public void SetNormalRectScale()
    {
        SetToBaseRectSize();
    }
}
