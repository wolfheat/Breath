using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int health = 10;
    private int damage = 5;

    [SerializeField] Animator animator;

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
