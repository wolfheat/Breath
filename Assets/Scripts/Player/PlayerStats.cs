using System;
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
    public bool AtMaxHealth { get { return health==maxHealth; }}
    
    private const int StartHealth = 10;
    private const int StartOxygen = 10;
    private const int StartSpeed = 2;
    
    private const int OxygenUsage = 1;
    private const int OxygenRefillSpeed = 10;
    private const float delay = 0.1f;
    WaitForSeconds coroutineDelay = new WaitForSeconds(delay);
    public bool IsDead { get; private set; }

    public Action<float, int> OxygenUpdated;
    public Action<float, int> HealthUpdated;

    public static PlayerStats Instance;

    public void SetToDead()
    {
        IsDead = true;
    }

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        StartCoroutine(UseOxygen());

        equiped.EquipmentChanged += UpdateEquipment;

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
        Debug.Log("PlayerStats Updated");

        // Change max values
        maxHealth = StartHealth + equiped.GetHealthAddition();
        maxOxygen = StartOxygen + equiped.GetOxygenAddition();
        maxSpeed  = StartSpeed  + equiped.GetSpeedAddition();

        // Limit oxygen if removing tank
        oxygen = Math.Min(oxygen, maxOxygen);
        health = Math.Min(health, maxHealth);

        // Update visuals
        infoHeader.SetInfo(maxHealth,maxOxygen,maxSpeed);

        // Update bars
        OxygenUpdated.Invoke(oxygen, maxOxygen);
        HealthUpdated.Invoke(health, maxHealth);
    }

    private IEnumerator UseOxygen()
    {
        while (true)
        {
            while (IsDead) 
                yield return coroutineDelay; 
            yield return coroutineDelay;
            float startOxygen = oxygen;

            if (!rb.useGravity)
            {
                
                if (oxygen > 0)
                {
                    oxygen -= OxygenUsage* delay;
                }
                else
                {
                    if (noOxygenSurvival == NoOxygenSurvivalMax)
                    {
                        Debug.Log("Starting to drown");
                        // Starting to "drown"
                        SoundMaster.Instance.PlaySound(SoundName.Drowning);
                        SoundMaster.Instance.FadeMusic();

                    }

                    if (noOxygenSurvival > 0)
                        noOxygenSurvival -= delay;
                    else
                    {
                        if (!IsDead)
                        {
                            Debug.Log("PlayerDIED");
                            IsDead = true;
                            uiController.ShowDeathScreen(); //TODO
                        }
                    }
                }
            }
            else
            {
                //Debug.Log("In gravity: "+ noOxygenSurvival+"="+NoOxygenSurvivalMax+" oxygen: "+oxygen );
                if (noOxygenSurvival < NoOxygenSurvivalMax)
                {
                    // Stops drowning clip from playing
                    if (oxygen <= 0)
                    {
                        Debug.Log("Stop drowning sound");
                        SoundMaster.Instance.StopSound(SoundName.Drowning);
                    }

                    noOxygenSurvival = Math.Min(NoOxygenSurvivalMax, noOxygenSurvival + OxygenRefillSpeed * delay);
                }

                if (oxygen < maxOxygen)
                    oxygen += OxygenRefillSpeed * delay;
                if (oxygen > maxOxygen)
                    oxygen = maxOxygen;

            }

            // Set distortioneffect and darkening from noOxygenSurvival value
            uiController.UpdateScreenDarkening(1-noOxygenSurvival/ NoOxygenSurvivalMax);
            uiController.SetOxygen(oxygen,maxOxygen);

            if (oxygen != startOxygen)
                OxygenUpdated.Invoke(oxygen,maxOxygen);
            

        }
    }

    public void Reset() 
    {
        noOxygenSurvival = NoOxygenSurvivalMax;
        oxygen = maxOxygen;
        health = StartHealth;
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
        Debug.Log("LOADING FROM FILE?");
        PlayerGameData data = SavingUtility.playerGameData;
        if (data == null) return;

        //Loading all data from file
        rb.position = SavingUtility.V3AsVector3(data.PlayerPosition);
        rb.rotation = Quaternion.LookRotation(SavingUtility.V3AsVector3(data.PlayerRotation),Vector3.up);
        Debug.Log("LOADING: Player position "+rb.position);

        health = SavingUtility.playerGameData.PlayerHealth;
        oxygen = SavingUtility.playerGameData.PlayerOxygen;

        OxygenUpdated.Invoke(oxygen,maxOxygen);
    }

    public void SetGameData()
    {
        // Player position and looking direction (Tilt is disregarder, looking direction is good enough)
        SavingUtility.playerGameData.PlayerPosition = SavingUtility.Vector3AsV3(rb.transform.position);
        SavingUtility.playerGameData.PlayerRotation = SavingUtility.Vector3AsV3(rb.transform.forward);

        // Inventory

        // Health, Oxygen
        SavingUtility.playerGameData.PlayerHealth = health;
        SavingUtility.playerGameData.PlayerOxygen = oxygen;

    }
}
