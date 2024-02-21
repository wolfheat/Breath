using System.Collections;
using UnityEngine;
using Wolfheat.StartMenu;

public class EnemyController : BaseObjectWithType, IObjectWithType
{
    public EnemyData Data;
    [SerializeField] Rigidbody rb;

    private int health = 100;
    private int webDamage = 12;
    private int pinchDamage = 5;
    public int PinchDamage { get { return pinchDamage; }}

    [SerializeField] EnemyPichAttackController pichAttackController;
    [SerializeField] Animator animator;
    [SerializeField] GameObject agitator;
    [SerializeField] GameObject leftArm;
    [SerializeField] GameObject rightArm;
    [SerializeField] GameObject idleState;
    [SerializeField] GameObject attackState;
    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject pinchAttackPoint;
    [SerializeField] GameObject[] explosionPoints;
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
        PlayerStats.Instance.PlayerDied += PlayerDied;
    }

    private void OnDisable()
    {
        PlayerStats.Instance.PlayerDied -= PlayerDied;
    }


    public void PlayerDied()
    {
        Debug.Log(" Boss recieved player dies info, set to Idle");
        isAttacking = false;
        idleState.SetActive(true);
        attackState.SetActive(false);
        StopAllCoroutines();
        animator.CrossFade("Idle",0.1f);
        animator.SetBool("web", false); // Go back to Idle
        animator.SetBool("pinch", false); // Go back to Idle
        agitateTimer = 0.1f;
        agitated = false;
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

        BulletCreator.Instance.GenerateBulletStorm(shootPoint.position, player.transform.position,35,webDamage);
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
                    // Face player then pinch attack
                    StartCoroutine(FaceAndAttack(player.transform.position,AttackType.Pinch));
                    
                }else
                    StartCoroutine(FaceAndAttack(player.transform.position, AttackType.Web));

                attackWaitTimer += AttackDelayTime;
            }


        }


    }

    private IEnumerator FaceAndAttack(Vector3 target,AttackType attackType)
    {
        Quaternion rotation = transform.rotation;
        // Rotate
        Vector3 targetDirection = target-transform.position;

        // Get max angle or angle towards player in z direction
        //float ZAngle = Vector3.Angle(Mathf.Cos)
        // If decided enemy can do pinch then pinch towards that posint without limitations?


        targetDirection.y = 0; 
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.down,targetDirection);
        //float angle = Quaternion.Angle(rotation, targetRotation);
        float angle = Vector3.Angle(transform.up, targetDirection);
        Debug.Log("Turn angle " + angle);   
        const float AngleVelocity = 80f;
        float turnTime = angle / AngleVelocity;
        float timer = 0;
        while (timer < turnTime)
        {
            rb.transform.rotation = Quaternion.Lerp(rotation, targetRotation, timer / turnTime);
            //Debug.Log("Turning towards player "+(Vector3.Angle(transform.forward,targetDirection)));
            timer += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Turning complete. Now Tilt");
        targetDirection = target - transform.position;
        angle = Vector3.SignedAngle(transform.up, targetDirection,Vector3.right);
        Debug.Log("Tilt angle "+angle);
        turnTime = angle / AngleVelocity;
        timer = 0;
        while (timer < turnTime)
        {
            rb.transform.rotation = Quaternion.Lerp(rotation, targetRotation, timer / turnTime);
            //Debug.Log("Turning towards player "+(Vector3.Angle(transform.forward,targetDirection)));
            timer += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Tilt complete.");

        rb.transform.rotation = targetRotation;
        StartAttack(attackType);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerStats.Instance.IsDead) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))    
        {
            Agitate();
            
            Bullet bullet = other.GetComponentInParent<Bullet>();
            Debug.Log(" Enemy Hit by player bullet, damage"+bullet?.Damage);
            health -= bullet.Damage;
            if (health <= 0 && !isDead)
            {
                Die();
            }
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("PLayer inside enemy pinch area");
            Agitate();
        }
    }

    private void Agitate()
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
        Debug.Log("Deathprocess started");
        float explodeTimer = 4f;
        const float ExplosionInterval = 0.3f;
        while (explodeTimer >= 0)
        {
            Vector3 randomPosition = explosionPoints[UnityEngine.Random.Range(0,explosionPoints.Length)].transform.position;
            ParticleEffects.Instance.PlayTypeAt(ParticleType.Small,randomPosition);
            randomPosition = explosionPoints[UnityEngine.Random.Range(0, explosionPoints.Length)].transform.position;
            ParticleEffects.Instance.PlayTypeAt(ParticleType.Small,randomPosition);
            SoundMaster.Instance.PlaySound(SoundName.BulletImpact, true);
            yield return new WaitForSeconds(ExplosionInterval);
            explodeTimer -= ExplosionInterval;
        }
        yield return new WaitForSeconds(3f);
        Debug.Log("Show PLayer win screen here");
        UIController.Instance.ShowWinScreen();
    }
}
