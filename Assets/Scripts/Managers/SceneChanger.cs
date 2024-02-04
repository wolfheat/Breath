using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        CheckedForScenes();
    }

    private void CheckedForScenes()
    {
        Debug.Log("** Checking Scenes to Set active. **");
        if (SceneManager.GetSceneByName("StartMenu").IsValid())
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("StartMenu"));
            Debug.Log("  StartMenu is set as active.");
        }
        if (SceneManager.GetSceneByName("MainScene").IsValid())
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));
            Debug.Log("  MainScene is set as active.");
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
