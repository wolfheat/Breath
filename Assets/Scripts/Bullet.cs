using UnityEngine;
using Wolfheat.StartMenu;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private const float Speed = 24f;
    
    private const float StartLife = 2f;
    private float life;
    private int damage = 1;
    public int Damage { get { return damage; }}

    private void Start()
    {
        life = StartLife;
        rb.AddForce(Speed*transform.forward,ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {        
        Debug.Log("Bullet collided with "+other.gameObject.name);
        ItemDestructEffect.Instance.PlayTypeAt(ParticleType.Plasma,transform.position);
        SoundMaster.Instance.PlaySound(SoundName.BulletImpact);
        Destroy(gameObject);
    }

    private void Update()
    {
        life -= Time.deltaTime;
        if(life <= 0){
            Debug.Log("Destroy bullet");
            Destroy(gameObject);
        }
    }

}
