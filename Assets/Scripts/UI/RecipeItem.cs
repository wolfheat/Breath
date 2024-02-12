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
        image.sprite = ingredience.picture;
        //string newText = (amount == 1 ? "" : amount.ToString());
        string newText = amount.ToString();
        if(playerGot>0)
            newText += " ("+playerGot.ToString()+ ")";
        amountText.text = newText;
        nameText.text = ingredience.itemName;
        // Set appropriate color
        background.color = playerGot<amount ? myGrey : green;
    }

    public void OccupyByData(ItemData result, bool canCreate)
    {
        image.sprite = result.picture;
        amountText.text = 1.ToString();
        nameText.text = result.itemName;
        // Set appropriate color
        background.color = canCreate ? green : myGrey;
        //SetAfford(canCreate);

    }
    public void SetAfford(bool afford)
    {
        image.material = afford ? null : grayscaleMaterial;
        background.color = afford ? greenColor : greenColor;
    }

    private void SetBackgroundHasItems()
    {
        background.color = Color.green;
    }
}
