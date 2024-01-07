using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform openPosition;
    [SerializeField] Transform closedPosition;

    private Coroutine coroutine;

    private const float DoorSpeed = 1.0f;
    private float doorDistance = 0;
    
    public void OpenDoor(bool open)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        StartCoroutine(OpenCloseCorutine(open?openPosition.position: closedPosition.position));
    }

    private IEnumerator OpenCloseCorutine(Vector3 targetPosition)
    {
        Vector3 dir = (targetPosition - transform.position);
        doorDistance = dir.magnitude;
        dir = dir.normalized;

        transform.position += dir*DoorSpeed * Time.deltaTime;
        while ((transform.position -targetPosition).magnitude<doorDistance)
        {
            doorDistance = (transform.position - targetPosition).magnitude;
            transform.position += dir*DoorSpeed*Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        yield return new WaitForEndOfFrame();
    }
}

