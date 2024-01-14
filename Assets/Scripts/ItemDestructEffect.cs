using UnityEngine;

public enum ParticleType{Small,Plasma}

public class ItemDestructEffect : MonoBehaviour
{
    public static ItemDestructEffect Instance;
    [SerializeField] ParticleSystem[] particleSystems;

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
        particleSystems[(int)type].Play();
        transform.position = pos;
    }
}
