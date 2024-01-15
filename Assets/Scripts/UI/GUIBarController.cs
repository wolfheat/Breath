using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GUIBarController : MonoBehaviour
{
    [SerializeField] BarController oxygenBar;
    [SerializeField] BarController healthBar;
    [SerializeField] PlayerHealth playerHealth;

    private void OnEnable()
    {
        playerHealth.OxygenUpdated += SetOxygen;
        //playerHealth.HealthUpdated += healthBar.SetBar;
    }

    private void SetOxygen(float oxygen, int maxOxygen)
    {
        oxygenBar.SetBar(oxygen / maxOxygen, ((int)oxygen).ToString());
    }

}
