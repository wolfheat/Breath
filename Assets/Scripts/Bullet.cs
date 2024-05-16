using UnityEngine;
using Wolfheat.StartMenu;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public int Damage { get; set; } = 1;
    private const float Speed = 24f;    
    private const float StartLife = 2f;
    private float life;

    private void Start()
    {
        // Set the initial life and speed for the bullet
        life = StartLife;
        rb.AddForce(Speed*transform.forward,ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {        
        // Hitting anything defined in collission matrix
        ParticleEffects.Instance.PlayTypeAt(ParticleType.Plasma,transform.position);
        SoundMaster.Instance.PlaySound(SoundName.BulletImpact,true);
        // Pool item later?
        Destroy(gameObject);
    }

    private void Update()
    {
        life -= Time.deltaTime;
        if(life <= 0)
            Destroy(gameObject);
    }

}
