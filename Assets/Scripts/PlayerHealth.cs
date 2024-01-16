using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] UIController uiController;
    [SerializeField] Rigidbody playerRb;
    private int health = 100;
    private const int MaxHealth = 100;
    
    private float noOxygenSurvival = 8;
    private const float NoOxygenSurvivalMax = 8;
    private float oxygen = 11;
    private const int MaxOxygen = 100;
    
    private const int OxygenUsage = 1;
    private const int OxygenRefillSpeed = 10;
    private const float delay = 0.1f;
    WaitForSeconds secondDelay = new WaitForSeconds(delay);
    public bool IsDead { get; private set; }

    public Action<float,int> OxygenUpdated;
    public Action HealthUpdated;

    private void Start()
    {
        StartCoroutine(UseOxygen());
    }

    private IEnumerator UseOxygen()
    {
        while (true)
        {
            yield return secondDelay;
            float startOxygen = oxygen;

            if (!playerRb.useGravity)
            {
                if (oxygen > 0)
                {
                    oxygen -= OxygenUsage* delay;
                }
                else
                {
                    if(noOxygenSurvival > 0)
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
                noOxygenSurvival = NoOxygenSurvivalMax;
                if (oxygen < MaxOxygen)
                    oxygen += OxygenRefillSpeed* delay;
            }

            // Set distortioneffect and darkening from noOxygenSurvival value
            uiController.UpdateScreenDarkening(1-noOxygenSurvival/ NoOxygenSurvivalMax);
            uiController.SetOxygen(oxygen,MaxOxygen);

            if (oxygen != startOxygen)
                OxygenUpdated.Invoke(oxygen,MaxOxygen);

        }
    }

    public void Reset() 
    {
        noOxygenSurvival = NoOxygenSurvivalMax;
        oxygen = MaxOxygen;
        IsDead = false;
    }
}
