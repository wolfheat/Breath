using System.Collections;
using UnityEngine;

public class EnemyController : BaseObjectWithType, IObjectWithType
{
    public EnemyData Data;
    [SerializeField] Rigidbody rb;

    private int health = 25;
    private int damage = 5;

    [SerializeField] Animator animator;

    public override int Type => Data.Type;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            Bullet bullet = other.GetComponentInParent<Bullet>();
            Debug.Log("Enemy Hit by player bullet, damage"+bullet?.Damage);
            health -= bullet.Damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void SetLocation(Vector3 pos,Vector3 forward,Vector3 up)
    {
        rb.position = pos;
        rb.rotation = Quaternion.LookRotation(forward,up);
    }
    public (Vector3 pos, Vector3 forward, Vector3 up) GetLocation()
    {
        return (rb.position,rb.transform.forward, rb.transform.up);
    }
    private void Die()
    {
        Debug.Log("Enemy has died, player wins");
        animator.CrossFade("Death",1f);
        StartCoroutine(DeathProcess());
    }

    private IEnumerator DeathProcess()
    {
        yield return new WaitForSeconds(7f);
        Debug.Log("Show PLayer win screen here");
        UIController.Instance.ShowWinScreen();
    }
}
