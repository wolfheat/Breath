using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI speed;
    [SerializeField] TextMeshProUGUI tilt;
    [SerializeField] TextMeshProUGUI playerTilt;
    [SerializeField] TextMeshProUGUI oxygenText;
    [SerializeField] Volume volume;
    [SerializeField] DeathScreen deathScreen;
    [SerializeField] DeathScreen winScreen;
    [SerializeField] PauseController pauseScreen;
    [SerializeField] GameObject tempHair;

    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] ToggleMenu inventoryToggle;
    [SerializeField] CraftingUI craftingUI;
    [SerializeField] ToggleMenu craftingToggle;

    [SerializeField] HUDIcons hudIcons;
    [SerializeField] Image image;
    [SerializeField] Player player;
    [SerializeField] PlayerStats playerStats;


    public static UIController Instance;
    public static bool InventoryActive { get { return Instance.inventoryToggle.IsActive; } }
    public static bool CraftingActive { get { return Instance.craftingToggle.IsActive; }}

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

    }

    public void OnEnable()
    {        
        Inputs.Instance.Controls.Player.Tab.started += Toggle;
        Inputs.Instance.Controls.Player.Esc.started += Pause;
    }
    
    public void OnDisable()
    {
        
        Inputs.Instance.Controls.Player.Tab.started -= Toggle;
        Inputs.Instance.Controls.Player.Esc.started -= Pause;
    }


    public void Pause(InputAction.CallbackContext context)
    {
        if (GameState.Instance.state == GameStates.Running)
        {
            GameState.Instance.state = GameStates.Paused;
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
        }
        else
        {
            UnPause();
        }
    }

    public void UnPause()
    {
        GameState.Instance.state = GameStates.Running;
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
    }

    public void Toggle(InputAction.CallbackContext context)
    {
        // Request To toggle inventory
        if (craftingToggle.IsActive)
        {
            Debug.Log("Trying to Toggle inventory but crafting is active");
            craftingUI.Reset();
            craftingToggle.HideMenu();
        }
        else
        {
            inventoryToggle.Toggle();
        }
    }

    public void SetSpeed(Vector3 s)
    {
        speed.text = "Speed: ("+s.x.ToString("F")+","+s.y.ToString("F") + ","+s.z.ToString("F") + ")("+s.magnitude.ToString("F") + ")";
    }
    public void ShowHUDIconAt(HUDIconType type, Interactable follow)
    {
        bool canInteractWith = true;
        if (follow is DestructableItem)
        {
            DestructableItem destructableItem = (DestructableItem)follow; 
            canInteractWith = Inventory.Instance.PlayerHasEquipped(destructableItem.Data.destructType);
        }
        hudIcons.Set(type, follow,canInteractWith);
    }
    public void HideHUDIcon()
    {
        hudIcons.Disable();
    }
    public void ShowTempHairAt(Vector2 s)
    {
        //Debug.Log("Tempt hair: "+tempHair+" UI ID: "+gameObject.GetInstanceID());
        if (tempHair.activeSelf) return;
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
        playerTilt.text = "PlayerTilt: ("+a.x+","+a.y+","+a.z+")";
    }

    public void SetOxygen(float oxygen, int maxOxygen)
    {
        oxygenText.text = "Oxygen: (" + oxygen.ToString("F") + "/" + maxOxygen + ")";
    }
    public void UpdateScreenDarkening(float percent)
    {
        image.color = new Color() { a = percent };
        volume.weight = percent;
    }
    public void ShowDeathScreen()
    {
        deathScreen.ShowScreen();
    }
    public void ShowWinScreen()
    {
        Debug.Log("PlayerDIED");
        playerStats.SetToDead();
        winScreen.ShowScreen();
    }
    public void ResetPlayer()
    {
        player.Reset();        
    }

}
