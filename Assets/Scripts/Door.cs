﻿using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform openPosition;
    [SerializeField] Transform closedPosition;

    private Coroutine coroutine;

    private const float DoorSpeed = 1.6f;
    private float DoorDistance = 0.1f;
    private float distance;

    private void Start()
    {
        distance = (openPosition.position - closedPosition.position).magnitude;
    }
    public void OpenDoor(bool open)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(open? 
            OpenCloseCorutine(closedPosition.position,openPosition.position): 
            OpenCloseCorutine(openPosition.position, closedPosition.position));
    }

    private IEnumerator OpenCloseCorutine(Vector3 originPositon, Vector3 targetPosition)
    {
        Vector3 startPos = transform.position;
        float distanceLeft = (startPos - targetPosition).magnitude;
        float totalTime = distance/DoorSpeed;
        float time = (1-distanceLeft/distance)*totalTime;

        while (true)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(originPositon,targetPosition,time/totalTime);
            yield return null;
            if (time > totalTime)
                break;
        }
        transform.position = targetPosition;
        yield return new WaitForEndOfFrame();
    }
}

