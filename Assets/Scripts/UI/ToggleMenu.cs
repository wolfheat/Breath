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
        Debug.Log("Toggle Request, panel is active: "+ panel.activeSelf+" ID:"+gameObject.GetInstanceID());

        if (panel.activeSelf)
            DragObject.Instance.UnSetDragedItem();
        //panel.SetActive(!panel.activeSelf);

        MakeVisible(!panel.activeSelf);
        //Debug.Log("UIActive set to "+UIController.UIActive);
    }

    // Animation
    public void HideMenu()
    {
        MakeVisible(false);
    }
    public void ShowRecipe()
    {
        MakeVisible(true);

    }
    private void MakeVisible(bool doMakeVisible)
    {
        Debug.Log("Make visible = "+doMakeVisible + " ID:" + gameObject.GetInstanceID());

        if (doMakeVisible)
            panel.SetActive(true);

        animator.enabled = true;
        animator.Play(doMakeVisible ? "MakeVisible" : "MakeInVisible");

    }
    public void AnimationComplete()
    {
        Debug.Log("Animation trigger speed:"+ animator.GetCurrentAnimatorStateInfo(0).speed + " ID:" + gameObject.GetInstanceID());
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("MakeInVisible"))
        {
            Debug.Log("Animation closes" + " ID:" + gameObject.GetInstanceID());
            panel.SetActive(false);
            animator.enabled = false;
        }

    }    

}
