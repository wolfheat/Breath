using System.Collections;
using UnityEngine;
using Wolfheat.StartMenu;

public class EnemyController : BaseObjectWithType, IObjectWithType
{
    public EnemyData Data;
    [SerializeField] Rigidbody rb;

    private int health = 25;
    private int damage = 5;

    [SerializeField] EnemyPichAttackController pichAttackController;
    [SerializeField] Animator animator;
    [SerializeField] GameObject agitator;
    [SerializeField] GameObject leftArm;
    [SerializeField] GameObject rightArm;
    [SerializeField] GameObject idleState;
    [SerializeField] GameObject attackState;
    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject pinchAttackPoint;
    Player player;
    private bool isDead = false;
    Coroutine deathProcess = null;
    public override int Type => Data.Type;

    public enum AttackType { Pinch,Cocoon,Web}


    // Enemy animation control

    private int attackcounter = 3;
    private const float AgitateTimeOutTime = 10f;
    private const float AttackDelayTime = 4f;
    private float agitateTimer = AgitateTimeOutTime;
    private float attackWaitTimer = AttackDelayTime;
    private bool agitated = false;
    private bool isAttacking = false;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void WebAttackAnimationComplete()
    {
        attackcounter--;
        Debug.Log(" Animation Completed, counter "+attackcounter);

        // Check if should change to pich attack
        if (pichAttackController.PlayerInRange)
        {
            Debug.Log("Player in pinch range while doing web attack, change to pinch attack");
            animator.SetBool("web", false); // Go back to Idle
            isAttacking = false;
            Debug.Log(" Attack set to false");
            idleState.SetActive(true);
            attackState.SetActive(false);
            attackWaitTimer = 1f;
        }
        else if (attackcounter == 0)
        {
            animator.SetBool("web", false); // Go back to Idle
            isAttacking = false;
            Debug.Log(" Attack set to false");
            idleState.SetActive(true);
            attackState.SetActive(false);
        }
    }
    
    public void PinchAttackAnimationComplete()
    {        
        animator.SetBool("pinch", false); // Go back to Idle
        isAttacking = false;
        idleState.SetActive(true);
        attackState.SetActive(false);
    }
    
    public void LeftPinchAttackPerformed()
    {
        leftArm.gameObject.SetActive(true);
        StartCoroutine(DisableArms());
    }

    private IEnumerator DisableArms()
    {
        SoundMaster.Instance.PlaySwoshSound();
        yield return null;
        yield return null;
        yield return null;
        leftArm.gameObject.SetActive(false);
        rightArm.gameObject.SetActive(false);
    }
    
    public void RightPinchAttackPerformed()
    {
        rightArm.gameObject.SetActive(true);
        StartCoroutine(DisableArms());
    }
    
    public void DoWebStormAttack()
    {
        Debug.Log(" Enemy Physically Attacking");
        
        Debug.Log("Pew");

        Quaternion direction = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);

        BulletCreator.Instance.GenerateBulletStorm(shootPoint.position, player.transform.position,20);
        //BulletCreator.Instance.GenerateBullet(shootPoint.transform.position,player.transform.position);
        SoundMaster.Instance.PlaySound(SoundName.EnemyShoot, true);
        
    }
    
    public void StartAttack(AttackType type = AttackType.Web)
    {
        Debug.Log(" Enemy Start Attack "+ type);
        idleState.SetActive(false);
        attackState.SetActive(true);
        isAttacking = true;
        switch (type)
        {
            case AttackType.Pinch:
            animator.SetBool("pinch", true);
                break;
            case AttackType.Cocoon:
            animator.SetBool("cocoon", true);
                break;
            case AttackType.Web:
            attackcounter = 3;
            animator.SetBool("web", true);
                break;
        }
    }

    private void Update()
    {
        if (isDead) return;

        if (agitated)
        {
            agitateTimer -= Time.deltaTime;

            if (agitateTimer <= 0)
            {
                agitated = false;
                Debug.Log(" Enemy no longer agitated");
                agitator.SetActive(false);
            }
        }

        if (agitated && !isAttacking)
        {
            // Count down timer if agitated but not currently attacking
            attackWaitTimer -= Time.deltaTime;            
            if (attackWaitTimer <= 0)
            {
                Debug.Log("attackWaitTimer run out, check for what attack to perform");
                // Check if player is within strike range
                if (pichAttackController.PlayerInRange)
                {
                    StartAttack(AttackType.Pinch);
                }else
                    StartAttack();


                attackWaitTimer += AttackDelayTime;
            }


        }


    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))    
        {
            agitateTimer = AgitateTimeOutTime;
            // Enemy is getting hit
            if (!agitated)
            {
                attackWaitTimer = 1f;
                Debug.Log("Enemy set to Agitated");
            }
            agitated = true;
            agitator.SetActive(true);

            Bullet bullet = other.GetComponentInParent<Bullet>();
            Debug.Log(" Enemy Hit by player bullet, damage"+bullet?.Damage);
            health -= bullet.Damage;
            if (health <= 0 && !isDead)
            {
                Die();
            }
        }
    }
    // Not currently used, the position is set the same way as items, directly to the transform
    public void SetLocation(Vector3 pos,Quaternion rot)
    {
        if(rb != null)
        {
            rb.position = pos;
            rb.rotation = rot;
        }
        else
        {
            transform.position = pos;
            transform.rotation = rot;
        }
    }
    public (Vector3 pos, Vector3 forward, Vector3 up) GetLocation()
    {
        return (rb.position,rb.transform.forward, rb.transform.up);
    }
    private void Die()
    {
        isDead = true;
        Debug.Log("Enemy has died, player wins");
        animator.CrossFade("Death",1f);
        if(deathProcess == null)
            deathProcess = StartCoroutine(DeathProcess());
    }
    
    private void Destroy()
    {
        Debug.Log("Destroy enemy");
        Destroy(gameObject);
    }

    private IEnumerator DeathProcess()
    {
        yield return new WaitForSeconds(7f);
        Debug.Log("Show PLayer win screen here");
        UIController.Instance.ShowWinScreen();
    }
}
