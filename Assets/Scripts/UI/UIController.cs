using System;
using TMPro;
using UnityEngine;
public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI speed;
    [SerializeField] TextMeshProUGUI tilt;
    [SerializeField] TextMeshProUGUI playerTilt;
    [SerializeField] GameObject tempHair;
    [SerializeField] InventoryUI inventoryUI;


    public static UIController Instance;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

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

    public void InventoryChanged()
    {
        inventoryUI.UpdateInventory();
        
    }

    public void SetTilt(float x)
    {
        tilt.text = "Tilt: (" + x + ")";
    }

    public void SetPlayerTilt(Vector3 a)
    {
        playerTilt.text = "PLayerTilt: ("+a.x+","+a.y+","+a.z+")";
    }
}
