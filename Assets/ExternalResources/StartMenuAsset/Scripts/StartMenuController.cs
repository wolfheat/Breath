using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wolfheat.StartMenu
{
    public struct AnimationRequest
{
    public Animator animator;
    public string animationName;
    public bool disable;
}
public enum MenuOption {MainMenu, Settings, Credits, StartGame, Exit}

public class StartMenuController : MonoBehaviour
{
    public static StartMenuController Instance { get; private set; }
    public MenuState menuState = MenuState.Idle;
    [SerializeField] StartMenuPanel credits;
    [SerializeField] StartMenuPanel settings;
    [SerializeField] StartMenuPanel startMenu;
    [SerializeField] private MenuOption nextMenu;
    public static MenuButton lastButton;

    private StartMenuPanel currentOption;

        public void SetNextMenu(int nextMenuindex)
    {
        Debug.Log("Set Next: " + Time.realtimeSinceStartup);
        if (menuState == MenuState.Transitioning) return;
        nextMenu = (MenuOption)nextMenuindex;
        SoundMaster.Instance.PlaySound(SoundName.MenuClick);
        CloseCurrent();
    }

    private void CloseCurrent()
    {
        currentOption.animator.CrossFade("Close", 0.1f);
        menuState = MenuState.Transitioning;
    }

    private void OnEnable()
    {
        Debug.Log("StartMenu On Enable");
        if (Instance != null) Destroy(gameObject);
        Instance = this;

        settings.gameObject.SetActive(false);
        credits.gameObject.SetActive(false);

        SoundMaster.Instance.PlayMusic(MusicName.MenuMusic);

        InitiateStartMenu();
    }

    public void ShowMenu(MenuOption menu)
    {
        switch (menu)
        {
            case MenuOption.MainMenu:
                InitiateStartMenu();
                break;
            case MenuOption.Settings:
                ShowSettings();
                break;
            case MenuOption.Credits:
                ShowCredits();
                break;
            case MenuOption.StartGame:
                StartGame();
                break;
            case MenuOption.Exit:
                ExitGame();
                break;
        }
    }

    public void AnimationComplete()
    {
        currentOption.gameObject.SetActive(false);        
        ShowMenu(nextMenu);

        // Maybe to early to enable this
        menuState = MenuState.Idle;

    }
    
    private void InitiateStartMenu()
    {
        
        startMenu.gameObject.SetActive(true);
        startMenu.animator.CrossFade("Initiate",0.1f);
        //startMenu.animator.Play("Initiate");
        currentOption = startMenu;
    }

    private void StartGame()
    {
        Debug.Log("Start Game Pressed");
        SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("StartMenu");

        }

    private void ShowSettings()
    {
        Debug.Log("Settings Pressed");
        menuState = MenuState.Transitioning;
        settings.gameObject.SetActive(true);
        currentOption = settings;
    }

    private void ShowCredits()
    {
        Debug.Log("Credits Pressed");
        menuState = MenuState.Transitioning;
        credits.gameObject.SetActive(true);
        currentOption = credits;
    }

    private void ExitGame()
    {
        SavingUtility.Instance.SavePlayerDataToFile();
        Debug.Log("Exit Pressed");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
public enum MenuNames {StartGame,Settings,Credits,Exit,
    CloseMenuOption
}
public enum MenuState {Idle,Transitioning}
}   
