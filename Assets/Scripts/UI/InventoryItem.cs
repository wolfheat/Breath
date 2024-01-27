using TMPro;    
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public TextMeshProUGUI nameField;
    public Image image; 

    public void SetItem(Sprite sprite,string text="",string nameIn="")
    {
        image.sprite = sprite;
        textField.text = text;
        nameField.text = nameIn;
    }
}
