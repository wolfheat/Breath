using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;        
    }

    private void Start()
    {

#if UNITY_EDITOR
            CheckedForScenes();
#else      
            ChangeScene("StartMenu");            
#endif


    }

    // This method only runs in editor mode, used to be able to start game from Start menu or In game correctly
    private void CheckedForScenes()
    {
        // Checking Scenes to Set active
        if (SceneManager.GetSceneByName("StartMenu").IsValid())
        {
            if (SceneManager.GetSceneByName("MainScene").IsValid())
            {
                SceneManager.UnloadSceneAsync("StartMenu");
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));
                return;
            }
            // StartMenu is set as active
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("StartMenu"));
        }else if (SceneManager.GetSceneByName("MainScene").IsValid())
        {
            // MainScene is set as active
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));
        }
    }

    public void ChangeScene(string name, bool additive = true, bool loadFromSaveFile = true)
    {
        if(additive)
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
        else
            SceneManager.LoadScene(name, LoadSceneMode.Single);
        StartCoroutine(ChangeToActive(name));// Changing scene to name
    }
    private IEnumerator ChangeToActive(string name)
    {
        yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
    }
}
