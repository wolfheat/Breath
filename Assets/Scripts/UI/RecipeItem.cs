using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeItem : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI amount;
    [SerializeField] Image background;

    public void OccupyByData(ResourceData ingredience)
    {
        image.sprite = ingredience.picture;
        amount.text = ""+1;
    }

    public void OccupyByData(ItemData result)
    {
        image.sprite = result.picture;
        amount.text = 1.ToString();
    }

    private void SetBackgroundHasItems()
    {
        background.color = Color.green;
    }
}
