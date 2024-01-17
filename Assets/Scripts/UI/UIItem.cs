using System;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    public ItemData data;
    [SerializeField] Image image;
    [SerializeField] RectTransform rect;
    private const int TileSize = 86;
    private const int TileSpace = 4;

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

}
