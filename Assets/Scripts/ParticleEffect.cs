using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem system;

    public void Play()
    {
        system.Play();
    }

}
