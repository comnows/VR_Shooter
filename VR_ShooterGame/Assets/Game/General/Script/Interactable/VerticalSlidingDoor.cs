using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Normal.Realtime;

public class VerticalSlidingDoor : Interactable
{
    public bool isOpen = false;
    [SerializeField] private float slideAmount = 3.0f;
    [SerializeField] private float slideSpeed = 1.0f;

    private Vector3 startPosition;
    private Vector3 slideDirection = Vector3.up;
    private RealtimeView _realtimeView;
    private RealtimeTransform _realtimeTransform;

    private void Start()
    {
        //startPosition = transform.position;
        _realtimeView = GetComponent<RealtimeView>();
        _realtimeTransform = GetComponent<RealtimeTransform>();
    }

    public override void Interact(GameObject player)
    {
        startPosition = transform.position;
        if (_realtimeTransform.isOwnedRemotelySelf || _realtimeTransform.isUnownedSelf)
        {
            _realtimeView.RequestOwnership();
            _realtimeTransform.RequestOwnership();
        }

        Debug.Log("Door opening");

        if (isOpen) return;

        StartCoroutine(DoorOpen());
    }

    public override void VRInteract(SelectEnterEventArgs args)
    {

        startPosition = transform.position;
        if (_realtimeTransform.isOwnedRemotelySelf || _realtimeTransform.isUnownedSelf)
        {
            _realtimeView.RequestOwnership();
            _realtimeTransform.RequestOwnership();
        }
        
        Debug.Log("Door opening");

        if (isOpen) return;

        StartCoroutine(DoorOpen());
    }

    private IEnumerator DoorOpen()
    {
        Vector3 endPosition = startPosition + slideAmount * slideDirection;

        float timeInSecond = 0;

        isOpen = true;
        
        while (timeInSecond < 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, timeInSecond);

            yield return null;

            timeInSecond += Time.deltaTime * slideSpeed;
        }
    }
}
