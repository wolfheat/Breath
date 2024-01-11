using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform openPosition;
    [SerializeField] Transform closedPosition;

    private Coroutine coroutine;

    private const float DoorSpeed = 1.6f;
    private float DoorDistance = 0.1f;
    
    public void OpenDoor(bool open)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(OpenCloseCorutine(open?openPosition.position: closedPosition.position));
    }

    private IEnumerator OpenCloseCorutine(Vector3 targetPosition)
    {
        Vector3 dir = (targetPosition - transform.position);
        dir = dir.normalized;

        transform.position += dir*DoorSpeed * Time.deltaTime;
        while ((transform.position -targetPosition).magnitude > DoorDistance)
        {
            transform.position += dir*DoorSpeed*Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        yield return new WaitForEndOfFrame();
    }
}

