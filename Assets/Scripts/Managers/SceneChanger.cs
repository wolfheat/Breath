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
    }


    public void ChangeScene(string name, bool additive = true, bool loadFromSaveFile = true)
    {
        if(additive)
            SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        else
            SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        StartCoroutine(ChangeToActive(name));// Changing scene to name
    }
    private IEnumerator ChangeToActive(string name)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
    }
}
