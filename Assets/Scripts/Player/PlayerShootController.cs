using UnityEngine;

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
        SoundMaster.Instance.PlaySFX(SoundMaster.SFX.Shoot);
    }
}