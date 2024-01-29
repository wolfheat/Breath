using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    public static ItemSelector Instance;
    [SerializeField] private ParticleSystem particle;
    private Transform target;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }

    public void SetToPosition(Transform newTarget)
    {
        target = newTarget;
        Debug.Log("Setting selector to target "+target.gameObject.name); 
        particle.Play();
    }

    public void Disable()
    {
        target = null;
        Debug.Log("Selector Lost Target");
        // Disable selector if not used
        particle.Clear();
        particle.Stop();
    }
}
