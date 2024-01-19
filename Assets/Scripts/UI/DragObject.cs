using UnityEngine;

public class DragObject : MonoBehaviour
{
    [SerializeField] Transform draggedParent;
    [SerializeField] Transform normalParent;
    private UIItem draggedItem;

    public static DragObject Instance;

    public void UnSetDragedItem()
    {
        draggedItem?.SetParent(normalParent);
        draggedItem?.ResetPosition();
        draggedItem = null;
    }
    
    public void SetDragedItem(UIItem item)
    {
        draggedItem = item;
        draggedItem?.SetParent(draggedParent);
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
}
