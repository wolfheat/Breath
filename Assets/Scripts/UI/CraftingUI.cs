using UnityEngine;
public class CraftingUI : MonoBehaviour
{
    [SerializeField] CraftButton[] baseCraftingButtons;


    public static CraftingUI Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < baseCraftingButtons.Length; i++)
        {
            baseCraftingButtons[i].ButtonID = i;
        }
        EnableBaseCrafting(-1);
    }


    public void EnableBaseCrafting(int id)
    {
        Debug.Log("Enabling crafting id: "+id);
        for (int i = 0;i < baseCraftingButtons.Length;i++)
        {
            baseCraftingButtons[i].ActivateSubMenu(i==id);
        }
    }


}
