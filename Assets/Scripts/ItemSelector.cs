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
            transform.position = target.position;
    }

    public void SetToPosition(Transform newTarget)
    {
        // Place selector at target
        target = newTarget;
        particle.Play();
    }

    public void Disable()
    {
        // Disable selector if not used = no target
        target = null;
        particle.Clear();
        particle.Stop();
    }
}
