using System.Collections;
using UnityEngine;
public class BulletCreator : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Bullet enemyBulletPrefab;
    public static BulletCreator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void GenerateBullet(Transform fromObject) => Instantiate(bulletPrefab, fromObject.position,fromObject.rotation,transform);
    public void GenerateBulletStorm(Vector3 fromPos, Vector3 toPos, int amount, int damage = 1) => StartCoroutine(BulletStorm(fromPos, toPos, amount, damage));
    private IEnumerator BulletStorm(Vector3 fromPos, Vector3 toPos, int amount, int damage = 1)
    {
        Vector3 forward = (toPos - fromPos).normalized;

        int created = 0;
        while (created<amount) {
            Vector3 random = UnityEngine.Random.insideUnitSphere*0.35f;
            Bullet bullet = Instantiate(enemyBulletPrefab, fromPos, Quaternion.LookRotation(forward+random), transform);
            bullet.Damage = damage;
            created++;
            yield return new WaitForSeconds(0.03f);
        }
    }

}
