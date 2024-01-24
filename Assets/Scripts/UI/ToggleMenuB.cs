using UnityEngine;

public class ToggleMenuB : MonoBehaviour
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
        if (panel.activeSelf)
            DragObject.Instance.UnSetDragedItem();
        //panel.SetActive(!panel.activeSelf);

        MakeVisible(!panel.activeSelf);
        //Debug.Log("UIActive set to "+UIController.UIActive);
    }

    // Animation
    public void HideRecipe()
    {
        MakeVisible(false);
    }
    public void ShowRecipe()
    {
        MakeVisible(true);

    }
    private void MakeVisible(bool doMakeVisible)
    {
        Debug.Log("Make visible = "+doMakeVisible);

        if (doMakeVisible)
            panel.SetActive(true);

        //animator.Play(doMakeVisible ? "MakeVisible" : "MakeInVisible");

    }
    public void AnimationComplete()
    {
        Debug.Log("Animation trigger speed:"+ animator.GetCurrentAnimatorStateInfo(0).speed);
        if (animator.GetCurrentAnimatorStateInfo(0).speed < 0)
        {
            Debug.Log("Animation closes");
            panel.SetActive(false);
        }

    }    

}
