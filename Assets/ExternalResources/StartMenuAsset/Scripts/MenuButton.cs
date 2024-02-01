using UnityEngine;
using UnityEngine.EventSystems;

namespace Wolfheat.StartMenu
{
    public class MenuButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public void AnimationComplete()
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //SoundMaster.Instance.PlaySound(SoundName.MenuClick);
            Debug.Log("Click in Button: "+Time.realtimeSinceStartup);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SoundMaster.Instance.PlaySound(SoundName.MenuOver);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //
        }
}
}
