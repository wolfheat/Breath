using UnityEngine;

public class ParticleEffectInititator : MonoBehaviour
{
    [SerializeField] ParticleSystem system;
    [SerializeField] GameObject panel;
    private bool playing;
    // Update is called once per frame
    void Update()
    {
        if(system.isPlaying)
            panel.SetActive(true);
        else
            panel.SetActive(false);
        playing = system.isPlaying;
    }
}
