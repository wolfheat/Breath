using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    public static ItemSelector Instance;
    [SerializeField] ParticleSystem particleSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void SetToPosition(Transform target)
    {
        Debug.Log("Setting selector to target"); 
        particleSystem.Play();
        transform.position = target.position;
    }

    public void Disable()
    {
        Debug.Log("Disable selector");
        // Disable selector if not used
        particleSystem.Clear();
        particleSystem.Stop();
    }
}
