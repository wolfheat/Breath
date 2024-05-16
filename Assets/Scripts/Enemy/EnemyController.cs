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
        ResetBossValues();
    }

    private void ResetBossValues()
    {
        SetToIdle();
        StopAllCoroutines();

        animator.CrossFade("Idle", 0.1f);
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
            // Player in pinch range while doing web attack, change to pinch attack via idle
            SetToIdle();
            attackWaitTimer = 1f;
        }
        else if (attackcounter == 0)
        {
            // Enemy completed all its attacks
            SetToIdle();
        }
    }

    private void SetToIdle()
    {
        isAttacking = false;
        animator.SetBool("web", false); 
        animator.SetBool("pinch", false); 
        idleState.SetActive(true);
        attackState.SetActive(false);
    }

    public void PinchAttackAnimationComplete() => SetToIdle();

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
        // Web storm attack - Enemy sends a mass of web projectiles in the direction of the player
        Debug.Log("Pew");

        BulletCreator.Instance.GenerateBulletStorm(shootPoint.position, player.transform.position,35,webDamage);
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
            animator.SetBool("cocoon", true); // Not yet implemented attack
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
                // Agitator is a debugging feature showing red dot on top of enemy when he is in an agitated state
                agitator.SetActive(false);
            }
        }

        if (agitated && !isAttacking)
        {
            // Count down timer if agitated but not currently attacking
            attackWaitTimer -= Time.deltaTime;            
            if (attackWaitTimer <= 0)
            {
                // attackWaitTimer run out, check for what attack to perform
                // Check if player is within strike range
                if (pichAttackController.PlayerInRange)
                {
                    // Face player then pinch attack
                    StartCoroutine(FaceAndAttack(player.transform.position,AttackType.Pinch));
                    
                }else
                    // Face player then web attack
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
        
        // If decided enemy can do pinch then pinch towards that pos without limitations?

        targetDirection.y = 0; 
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.down,targetDirection);
        
        float angle = Vector3.Angle(transform.up, targetDirection);
        Debug.Log("Turn angle " + angle);   
        const float AngleVelocity = 80f;
        float turnTime = angle / AngleVelocity;
        float timer = 0;

        // Rotate to face player
        while (timer < turnTime)
        {
            rb.transform.rotation = Quaternion.Lerp(rotation, targetRotation, timer / turnTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // Turning complete. Now Tilt
        targetDirection = target - transform.position;
        angle = Vector3.SignedAngle(transform.up, targetDirection,Vector3.right);
        turnTime = angle / AngleVelocity;
        timer = 0;
        while (timer < turnTime)
        {
            rb.transform.rotation = Quaternion.Lerp(rotation, targetRotation, timer / turnTime);
            timer += Time.deltaTime;
            yield return null;
        }
        // Tilt complete, Start attack

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
            // Enemy Hit by player bullet
            health -= bullet.Damage;
            if (health <= 0 && !isDead)
                Die();
        }
        // Player inside enemy pinch area
        else if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
            Agitate();
    }

    // Enemy is getting hit
    private void Agitate()
    {
        agitateTimer = AgitateTimeOutTime;
        if (!agitated)
            attackWaitTimer = 1f;
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
    
    public (Vector3 pos, Vector3 forward, Vector3 up) GetLocation() => (rb.position, rb.transform.forward, rb.transform.up);

    private void Die()
    {
        // Enemy has died, player wins
        isDead = true;
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
        // Deathprocess started
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
        // Show player Win screen
        UIController.Instance.ShowWinScreen();
    }
}
