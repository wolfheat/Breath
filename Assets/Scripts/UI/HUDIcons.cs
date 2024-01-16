using System;
using UnityEngine;

public enum HUDIconType {Generic,PickUp, LeftClick, Interact };
public class HUDIcons : MonoBehaviour
{
    [SerializeField] private GameObject[] iconList;
    Interactable objectToFollow;
    public void Disable()
    {
        HideAll();
        objectToFollow = null;
    }

    private void HideAll()
    {
        foreach (var t in iconList)
            t.SetActive(false);
    }

    private void Update()
    {
        if(objectToFollow != null)
            transform.position = Camera.main.WorldToScreenPoint(objectToFollow.transform.position);
    }
    public void Set(HUDIconType type, Interactable follow)
    {
        HideAll();
        iconList[(int)type].SetActive(true);
        objectToFollow = follow;
    }
}
