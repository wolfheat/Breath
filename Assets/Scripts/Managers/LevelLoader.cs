using System;
using System.Collections;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{

    public static LevelLoader Instance { get; private set; }

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    
        Debug.Log("LevelLoader Started");
        if (SavingUtility.playerGameData == null)
            SavingUtility.LoadingComplete += LevelDataLoaded;
        else
            LoadLevel();
    }

    private void LevelDataLoaded()
    {
        Debug.Log("Level Data is reported as loaded.");
        LoadLevel();
    }
    
    private void LoadLevel()
    {
        if (!SavingUtility.useLoadedData)
        {
            Debug.Log("Use Loaded Data set to false, dont load the level.");
            return;
        }
        else if (SavingUtility.playerGameData == null)
        {
            Debug.Log("Player Game Data is empty, cant load the level.");
            return;
        }

        Debug.Log("Loading Data from save.");
        LoadGameData();

    }

    private void LoadGameData()
    {

        PlayerStats.Instance.LoadFromFile();

        // Save Destructables Data
        ItemCreator.Instance.LoadFromFile();

    }

    public void SetGameData()
    {
        Debug.Log("Setting Game Data");
        PlayerStats.Instance.SetGameData();

        // Save Destructables Data
        ItemCreator.Instance.SetGameData();
    }
}
