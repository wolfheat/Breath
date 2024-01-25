using TMPro;
using UnityEngine;

public class HUDMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject textObject;
    [SerializeField] private Animator animator;


    public static HUDMessage Instance;
    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }


    public void ShowMessage(string message)
    {
        text.text = message;
        animator.Play("ShowMessage");
        SoundMaster.Instance.PlaySFX(SoundMaster.SFX.HUDError);
    }
}
