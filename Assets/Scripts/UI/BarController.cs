﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    [SerializeField] RectTransform bar;
    [SerializeField] TextMeshProUGUI text;
    private Rect rect;
    private Vector2 origSize;

    private void Start()
    {
        origSize = bar.rect.size;
        Debug.Log("Original size: "+origSize);
        rect = bar.rect;
    }
    public void SetBar(float value, string textValue)
    {
        text.text = textValue;
        //text.text = (int)value+"/"+maxValue;
        float size = bar.rect.size.x;
        bar.sizeDelta = new Vector2(value*origSize.x,origSize.y);
    }
}