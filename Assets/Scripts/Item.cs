using UnityEngine;

public abstract class Item : MonoBehaviour, IInteractable
{
    public abstract void InteractWith();
}

public interface IInteractable
{
    public void InteractWith();
}
