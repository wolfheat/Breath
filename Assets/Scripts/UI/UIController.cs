using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI speed;
    [SerializeField] GameObject tempHair;
    

    public void SetSpeed(Vector3 s)
    {
        speed.text = "Speed: ("+s.x+","+s.y+","+s.z+")";
    }
    public void ShowTempHairAt(Vector2 s)
    {
        tempHair.transform.position = s;
        tempHair.SetActive(true);
    }
    public void HideTempHair()
    {
        tempHair.SetActive(false);
    }
}
