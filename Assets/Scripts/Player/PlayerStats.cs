﻿using System;
using System.Collections;
using UnityEngine;
using Wolfheat.StartMenu;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] UIController uiController;
    [SerializeField] Rigidbody rb;
    [SerializeField] EquipedGrid equiped;
    [SerializeField] InfoHeader infoHeader;
    private int health = 100;
    private float oxygen = 11;
    private int speed = 2;
    
    private float noOxygenSurvival = 8f;
    private const float NoOxygenSurvivalMax = 8f;

    private int maxHealth = 100;
    private int maxOxygen = 70;
    private int maxSpeed = 2;
    public int MaxSpeed { get { return maxSpeed; }}
    public bool AtMaxHealth => health == maxHealth;
    
    private const int StartHealth = 10;
    private const int StartOxygen = 10;
    private const int StartSpeed = 2;
    
    private const int OxygenUsage = 1;
    private const int OxygenWarningLevel = 10;
    private const int SecondOxygenWarningLevel = 2;
    private const int OxygenRefillSpeed = 10;
    private const float delay = 0.1f;
    WaitForSeconds coroutineDelay = new WaitForSeconds(delay);
    public bool IsDead { get; private set; }

    public Action<float, int> OxygenUpdated;
    public Action<float, int> HealthUpdated;

    public static PlayerStats Instance;

    public Action PlayerDied;

    public void SetToDead()
    {
        IsDead = true;
    }

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
        // Start the Use oxygen routine, this runs without stopping
        StartCoroutine(UseOxygen());

        // Update Equipments every time the event of equipment change occure
        equiped.EquipmentChanged += UpdateEquipment;

        // Initial values set
        maxOxygen = StartOxygen;
        maxHealth= StartHealth;
        maxSpeed= StartSpeed;

        oxygen = maxOxygen;
        health = maxHealth;
        speed = maxSpeed;


        UpdateEquipment();
    }

    private void UpdateEquipment()
    {
        // Updating Stats from Equipment "Benefits"

        // Change max values
        maxHealth = StartHealth + equiped.Health;
        maxOxygen = StartOxygen + equiped.Oxygen;
        maxSpeed  = StartSpeed  + equiped.Speed;

        // Limit oxygen if removing tank
        oxygen = Math.Min(oxygen, maxOxygen);
        health = Math.Min(health, maxHealth);

        // Update visuals
        infoHeader.SetInfo(maxHealth,maxOxygen,maxSpeed);

        // Update bars
        OxygenUpdated.Invoke(oxygen, maxOxygen);
        HealthUpdated.Invoke(health, maxHealth);
    }

    public void TakeDamage(int amt)
    {
        health -= amt;
        HealthUpdated.Invoke(health,maxHealth);
        if (health <= 0)
        {
            health = 0;
            IsDead = true;
            PlayerDied?.Invoke();
            uiController.ShowDeathScreen();
        }
    }
    private IEnumerator UseOxygen()
    {
        // This routine always runs
        while (true)
        {
            // This routines effect is "paused" when dead
            while (IsDead) 
                yield return coroutineDelay; 
            yield return coroutineDelay;
            float startOxygen = oxygen;

            if (!rb.useGravity)
            {
                // Only use oxygen when in vaccuum = outside
                if (oxygen > 0)
                {
                    bool aboveWarningBeforeDecrease = oxygen >= OxygenWarningLevel;
                    bool aboveSecondWarningBeforeDecrease = oxygen >= SecondOxygenWarningLevel;
                    oxygen -= OxygenUsage* delay;

                    // Give warning of low oxygen
                    if(aboveWarningBeforeDecrease && oxygen < OxygenWarningLevel)
                        HUDMessage.Instance.ShowMessage("Oxygen!",sound: SoundName.LowOxygen);
                    else if (aboveSecondWarningBeforeDecrease && oxygen < SecondOxygenWarningLevel)
                        HUDMessage.Instance.ShowMessage("Quickly Now!",sound: SoundName.LowOxygen);
                }
                else
                {
                    // All oxygen is used ,player starts to drown
                    // No oxygen Survival stat is monitoring how long the player survives without oxygen, separate timer
                    if (noOxygenSurvival == NoOxygenSurvivalMax)
                    {
                        // Starting to drown
                        SoundMaster.Instance.PlaySound(SoundName.Drowning);
                        SoundMaster.Instance.FadeMusic();
                    }                    
                    if (noOxygenSurvival > 0)
                        noOxygenSurvival -= delay;
                    else
                    {
                        // Kill player if not allready dead
                        if (!IsDead)
                        {
                            Debug.Log("PlayerDIED");
                            IsDead = true;
                            PlayerDied?.Invoke();
                            uiController.ShowDeathScreen();
                        }
                    }
                }
            }
            else
            {
                // Only regain oxygen when inside
                if (noOxygenSurvival < NoOxygenSurvivalMax)
                {
                    // Stops drowning clip from playing
                    if (oxygen <= 0)
                    {
                        Debug.Log("Stop drowning sound");
                        SoundMaster.Instance.StopSound(SoundName.Drowning);
                    }
                    // Refills the drowning timer at same rate as the oxygen (not visible to player)
                    noOxygenSurvival = Math.Min(NoOxygenSurvivalMax, noOxygenSurvival + OxygenRefillSpeed * delay);
                }
                // Refills oxygen tank 
                if (oxygen < maxOxygen)
                    oxygen += OxygenRefillSpeed * delay;
                if (oxygen > maxOxygen)
                    oxygen = maxOxygen;

            }

            // Set distortioneffect and darkening from noOxygenSurvival value
            uiController.UpdateScreenDarkening(1-noOxygenSurvival/ NoOxygenSurvivalMax);
            uiController.SetOxygen(oxygen,maxOxygen);

            // Check if the current oxygen is the same as we started with i.e no uptdates event dispatch needed
            if (oxygen != startOxygen)
                OxygenUpdated.Invoke(oxygen,maxOxygen);
        }
    }

    public void ResetPlayer(bool keepstats = false) 
    {
        if (IsDead)
        {
            noOxygenSurvival = NoOxygenSurvivalMax;
            oxygen = maxOxygen;
            health = StartHealth;
        }
        IsDead = false;
        SoundMaster.Instance.PlayMusic(MusicName.IndoorMusic);
        OxygenUpdated.Invoke(oxygen, maxOxygen);
        HealthUpdated.Invoke(health, maxHealth);
    }

    public void Consume(ConsumableData data)
    {
        health = Math.Min(maxHealth, health + data.healthRegain);
        HealthUpdated.Invoke(health, maxHealth);
    }

    public void LoadFromFile()
    {
        PlayerGameData data = SavingUtility.playerGameData;
        if (data == null) return;

        // Loading all data from file
        rb.position = SavingUtility.V3AsVector3(data.PlayerPosition);
        rb.rotation = Quaternion.LookRotation(SavingUtility.V3AsVector3(data.PlayerRotation),Vector3.up);

        health = SavingUtility.playerGameData.PlayerHealth;
        oxygen = SavingUtility.playerGameData.PlayerOxygen;

        OxygenUpdated.Invoke(oxygen,maxOxygen);
        Debug.Log("  * Updating Player *");
        Debug.Log("    Player loaded: position: "+rb.position+ " oxygen: "+oxygen+" health: "+health);
        HealthUpdated.Invoke(health,maxHealth);
    }

    public void DefineGameDataForSave()
    {
        // Player position and looking direction (Tilt is disregarder, looking direction is good enough)
        SavingUtility.playerGameData.PlayerPosition = SavingUtility.Vector3AsV3(rb.transform.position);
        SavingUtility.playerGameData.PlayerRotation = SavingUtility.Vector3AsV3(rb.transform.forward);

        // Health, Oxygen
        SavingUtility.playerGameData.PlayerHealth = health;
        SavingUtility.playerGameData.PlayerOxygen = oxygen;

    }
}
