using TMPro;
using UnityEngine;

public class BarController : MonoBehaviour
{
    [SerializeField] RectTransform bar;
    [SerializeField] TextMeshProUGUI text;

    public void SetBar(float value, string textValue)
    {
        text.text = textValue;
        bar.sizeDelta = new Vector2(-GUIBarController.Barwidth*(1-value),0);
    }
}
