using System;
using System.Collections.Generic;

public class AchievementData
{
    public bool[] Data { get; set; } = new bool[0];
}

public class MissionsSaveData
{
    public Dictionary<int,MissionSaveData> Data { get; set; } = new Dictionary<int, MissionSaveData>();
}

public class MissionSaveData
{
    public bool everCompleted = false;
    public bool active = true;
    public int amount = 0;
    public DateTime lastCompletion = DateTime.MinValue;


    public static Action MissionUpdate;

    public void SetMissionCompletionInfo()
    {
        // Set new last completiontime
        lastCompletion = DateTime.UtcNow;
        everCompleted = true;
        amount = 0;

        MissionUpdate?.Invoke();
    }

    public bool CompleteStepForMission(int completeAmount)
    {
        amount++;

        // Invoke if not completed (if completed UpdateMissionCompletion will be called which invokes the save)
        MissionUpdate?.Invoke();

        return amount >= completeAmount;
    }

}

[Serializable]
public class LightSettings
{
    public float LightIntensity { get; set; } = 1;
}

[Serializable]
public class GameEffectsSettings
{
    public bool UseShake { get; set; } = true;
    public bool AnimatedWater { get; set; } = true;
}

[Serializable]
public class SoundSettings
{
    public bool UseMusic { get; set; } = true;
    public float MusicVolume { get; set; } = 0.4f;
    public bool UseSFX { get; set; } = true;
    public float SFXVolume { get; set; } = 0.4f;
}
[Serializable]
public class PlayerGameData
{
    // Players Inventory

    // Totals
    public int PlayTime { get; set; } = 0;
    
    // Action Events
    public static Action InventoryUpdate;
    public static Action MinuteWatched;

    public void AddPlayTimeMinutes(int amt)
    {
        PlayTime += amt;
        MinuteWatched?.Invoke();
    }
}

[Serializable]
public class GameSettingsData
{
    // General Game Settings
    public int ActiveTouchControl { get; set; } // Having these private set wont let the load method write these values
    public int CameraPos { get; set; } // Having these private set wont let the load method write these values

    public SoundSettings soundSettings = new SoundSettings();
    public LightSettings lightSettings = new LightSettings();
    public GameEffectsSettings gameEffectsSettings = new GameEffectsSettings(); // Use shake etc

    // Action Events
    public static Action GameSettingsUpdated;

    // General Settings - methods
    internal void ChangeActiveTouchControl(int id)
    {
        ActiveTouchControl = id;
        GameSettingsUpdated?.Invoke();
    }
    internal void ChangeCameraPos(int id)
    {
        CameraPos = id;
        GameSettingsUpdated?.Invoke();
    }
}
