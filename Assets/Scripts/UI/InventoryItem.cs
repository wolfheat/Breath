using TMPro;    
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public Image image; 

    public void SetItem(Sprite sprite,string text="")
    {
        image.sprite = sprite;
        textField.text = text;
    }
}
