using UnityEngine;

public enum HUDIconType {Generic,PickUp, LeftClick, Interact };
public class HUDIcons : MonoBehaviour
{
    [SerializeField] private HUDIcon[] iconList;
    Interactable objectToFollow;
    public void Disable()
    {
        HideAll();
        objectToFollow = null;
    }

    private void HideAll()
    {
        foreach (var t in iconList)
            t.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(objectToFollow != null)
            transform.position = Camera.main.WorldToScreenPoint(objectToFollow.transform.position);
    }
    public void Set(HUDIconType type, Interactable follow,bool canInteract)
    {
        HideAll();
        iconList[(int)type].gameObject.SetActive(true);
        iconList[(int)type].SetAvailable(canInteract);
        objectToFollow = follow;
    }
}
