using UnityEngine;
using Wolfheat.StartMenu;

public class PlayerShootController : MonoBehaviour
{
    [SerializeField] GameObject gun;

    private const float ReloadTime = 1f;
    private float reloadTime = 1f;

    private void Update()
    {
        if (reloadTime > 0)
            reloadTime -= Time.deltaTime;
    }

    public void ResetTimer() => reloadTime = ReloadTime;
    public bool RequestShoot()
    {
        if (reloadTime <= 0)
        {
            DoShoot();
            reloadTime += ReloadTime;
            return true;
        }
        return false;
    }

    private void DoShoot()
    {
        Debug.Log("Pew");
        BulletCreator.Instance.GenerateBullet(gun.transform);
        SoundMaster.Instance.PlaySound(SoundName.Shoot, true);
    }
}
