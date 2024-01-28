using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] UIController uiController;
    [SerializeField] Rigidbody playerRb;
    [SerializeField] EquipedGrid equiped;
    [SerializeField] InfoHeader infoHeader;
    private int health = 100;
    private float oxygen = 11;
    private int speed = 2;
    
    private float noOxygenSurvival = 8;
    private const float NoOxygenSurvivalMax = 8;

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
        Debug.Log("Update equipment");

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
            yield return coroutineDelay;
            float startOxygen = oxygen;

            if (!playerRb.useGravity)
            {
                if (oxygen > 0)
                {
                    oxygen -= OxygenUsage* delay;
                }
                else
                {
                    if (noOxygenSurvival == NoOxygenSurvivalMax)
                    {
                        // Starting to "drown"
                        SoundMaster.Instance.PlaySFX(SoundMaster.SFX.Drowning);
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
                            uiController.ShowDeathScreen();
                        }
                    }
                }
            }
            else
            {
                if (noOxygenSurvival < NoOxygenSurvivalMax)
                {
                    // Stops drowning clip from playing
                    if (oxygen == 0)
                        SoundMaster.Instance.StopSFX();

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
        IsDead = false;
        SoundMaster.Instance.ResumeMusic();
    }

    public void Consume(ConsumableData data)
    {
        health = Math.Min(maxHealth, health + data.healthRegain);
        HealthUpdated.Invoke(health, maxHealth);
    }
}
