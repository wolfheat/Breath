using UnityEngine;

public class ToggleMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Animator animator;

    public bool IsActive { get { return panel.activeSelf; }}

    private void Start()
    {
        panel.SetActive(false);
    }

    public void Toggle()
    {
        // If item being dragged reset it
        if (panel.activeSelf)
            DragObject.Instance.UnSetDragedItem();

        // Toggle the menu
        MakeVisible(!panel.activeSelf);
    }

    public void HideMenu() => MakeVisible(false);

    public void ShowRecipe() => MakeVisible(true);

    private void MakeVisible(bool doMakeVisible)
    {
        if (doMakeVisible)
            panel.SetActive(true);

        animator.enabled = true;
        animator.Play(doMakeVisible ? "MakeVisible" : "MakeInVisible");
    }

    public void AnimationComplete()
    {
        // Check if the invisible animation completed, if so unload the panel
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("MakeInVisible"))
        {
            panel.SetActive(false);
            animator.enabled = false;
        }
    }    
}
