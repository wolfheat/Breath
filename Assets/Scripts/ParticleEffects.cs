using UnityEngine;

public enum ParticleType{Small,Plasma,PickUp,Creation}
public class ParticleEffects : MonoBehaviour
{
    public static ParticleEffects Instance;
    [SerializeField] ParticleEffect[] particleSystems;

    private void Awake()
    {
        Debug.Log("ItemDestructEffect initialized");
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void PlayTypeAt(ParticleType type, Vector3 pos)
    {
        // Create instance
        ParticleEffect particleEffect = Instantiate(particleSystems[(int)type],pos,Quaternion.identity,transform);
        particleEffect.Play();
    }
}
