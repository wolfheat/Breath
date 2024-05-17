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
    [SerializeField] WinScreen winScreen;
    [SerializeField] GameObject debugHelpText;
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
#if !UNITY_EDITOR
        debugHelpText.SetActive(false);
#endif
    }

    public void OnEnable()
    {        
        Inputs.Instance.Controls.Player.Tab.started += Toggle;
        Inputs.Instance.Controls.Player.Esc.started += Pause;

        // Start by setting game to unpaused
        Pause(false);
    }
    
    public void OnDisable()
    {        
        Inputs.Instance.Controls.Player.Tab.started -= Toggle;
        Inputs.Instance.Controls.Player.Esc.started -= Pause;
    }


    public void Pause(InputAction.CallbackContext context)
    {
        // Player can not toggle pause when dead
        if (playerStats.IsDead) return;

        // Toggle pause
        bool doPause = GameState.state == GameStates.Running;
        Pause(doPause);
        pauseScreen.SetActive(doPause);
    }

    public void Pause(bool pause = true)
    {
        // Setting state and timescale
        GameState.state = pause?GameStates.Paused:GameStates.Running;
        Time.timeScale = pause?0f:1f;
    }

    public void Toggle(InputAction.CallbackContext context)
    {
        // Disable toggle when game is paused
        if (GameState.IsPaused)
            return;

        // Request To toggle inventory
        if (craftingToggle.IsActive)
        {
            craftingUI.Reset();
            craftingToggle.HideMenu();
        }
        else
            inventoryToggle.Toggle();
    }

    public void SetSpeed(Vector3 s) => speed.text = "Speed: (" + s.x.ToString("F") + "," + s.y.ToString("F") + "," + s.z.ToString("F") + ")(" + s.magnitude.ToString("F") + ")";

    public void ShowHUDIconAt(HUDIconType type, Interactable follow)
    {
        bool canInteractWith = true;
        if (follow is DestructableItem)
        {
            // Destructable and player can interact if he have the correct tool
            DestructableItem destructableItem = (DestructableItem)follow; 
            canInteractWith = Inventory.Instance.PlayerHasEquipped(destructableItem.Data.destructType);
        }
        // Show the icon
        hudIcons.Set(type, follow,canInteractWith);
    }
    public void HideHUDIcon() => hudIcons.Disable();

    public void HideTempHair() => tempHair.SetActive(false);

    public void InventoryChanged() => inventoryUI.UpdateInventory();

    public void SetTilt(float x) => tilt.text = "Tilt: (" + x + ")";

    public void SetPlayerTilt(Vector3 a) => playerTilt.text = "PlayerTilt: (" + a.x + "," + a.y + "," + a.z + ")";

    public void SetOxygen(float oxygen, int maxOxygen) => oxygenText.text = "Oxygen: (" + oxygen.ToString("F") + "/" + maxOxygen + ")";

    public void UpdateScreenDarkening(float percent)
    {
        // Setting darkening effect to specific percent
        image.color = new Color() { a = percent };
        volume.weight = percent;
    }

    public void ShowDeathScreen()
    {
        Pause();
        playerStats.SetToDead();
        deathScreen.ShowScreen();
    }

    public void ShowWinScreen()
    {
        GameState.state = GameStates.Paused;
        winScreen.ShowScreen();
    }

    public void ResetPlayer(bool keepStats = false)
    {
        player.ResetPlayer(keepStats);
        Pause(false);
    }

}
