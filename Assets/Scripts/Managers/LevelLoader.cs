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
            Debug.Log(" ** LOADING DEFAULT LEVEL **");
            return;
        }
        else if (SavingUtility.playerGameData == null)
        {
            Debug.Log("Player Game Data is empty, cant load the level.");
            return;
        }

        LoadGameData();

    }

    private void LoadGameData()
    {
        Debug.Log(" ** LOADING LEVEL FROM SAVE **");

        PlayerStats.Instance.LoadFromFile();

        Inventory.Instance.LoadFromFile();

        // Save Destructables Data
        ItemCreator.Instance.LoadFromFile();

        Debug.Log("**** LOADING COMPLETE ****");
    }

    public void DefineGameDataForSave()
    {
        Debug.Log("  Setting Game Data");
        PlayerStats.Instance.DefineGameDataForSave();
        
        Inventory.Instance.DefineGameDataBeforSave();

        // Save Destructables Data
        ItemCreator.Instance.DefineGameDataForSave();
    }
}
