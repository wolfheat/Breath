using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeItem : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image background;
    [SerializeField] Material grayscaleMaterial;
    private Color greenColor = new Color32(0x93, 0xE7, 0x86, 0xFF);
    private Color green = new Color32(0x93, 0xE7, 0x86, 0xFF);
    private Color red = new Color32(0xDB, 0x8B, 0x81, 0xFF);
    private Color myGrey = new Color32(0xB0, 0xB0, 0xB0, 0xFF);

    public void OccupyByData(ResourceData ingredience, int amount = 1, int playerGot = 1)
    {
        // Set sprite
        image.sprite = ingredience.picture;

        // Show the amount needed and the amount player has
        string newText = amount.ToString();
        if(playerGot>0)
            newText += " ("+playerGot.ToString()+ ")";
        amountText.text = newText;
        
        // Set ingredience name
        nameText.text = ingredience.itemName;

        // Set appropriate color
        background.color = playerGot<amount ? myGrey : green;
    }

    public void OccupyByData(ItemData result, bool canCreate)    
    {
        // Set Sprite
        image.sprite = result.picture;

        // Set result amount and name
        amountText.text = 1.ToString();
        nameText.text = result.itemName;

        // Set appropriate color
        background.color = canCreate ? green : myGrey;
    }

    public void SetAfford(bool afford)
    {
        image.material = afford ? null : grayscaleMaterial;
        background.color = afford ? greenColor : greenColor;
    }

    private void SetBackgroundHasItems() => background.color = Color.green;
}
