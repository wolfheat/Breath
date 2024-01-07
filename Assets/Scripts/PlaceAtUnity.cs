using UnityEngine;

public class PlaceAtUnity : MonoBehaviour
{
    [SerializeField] private GameObject unitToFollow;
    
    void Update()
    {
        transform.SetLocalPositionAndRotation(unitToFollow.transform.position,unitToFollow.transform.rotation);
    }
}
