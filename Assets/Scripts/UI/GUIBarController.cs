using UnityEngine;

public class GUIBarController : MonoBehaviour
{
    [SerializeField] BarController oxygenBar;
    [SerializeField] BarController healthBar;
    [SerializeField] PlayerStats playerHealth;

    private void OnEnable()
    {
        playerHealth.OxygenUpdated += SetOxygen;
        playerHealth.HealthUpdated += SetHealth;
    }

    private void SetHealth(float health, int maxHealth)
    {
        healthBar.SetBar(health / maxHealth, ((int)health).ToString());
    }
    
    private void SetOxygen(float oxygen, int maxOxygen)
    {
        oxygenBar.SetBar(oxygen / maxOxygen, ((int)oxygen).ToString());
    }

}
