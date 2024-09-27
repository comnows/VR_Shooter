using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class PlayerLookMovement : MonoBehaviour
{
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Transform playerBody;

    private float xInput;
    private float yInput;

    [SerializeField] private float mouseSensitivity = 1200f;
    private float xRotation = 0f;

    private RealtimeView _realtimeView;

    private void Awake() 
    {
        _realtimeView = GetComponent<RealtimeView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (_realtimeView.isOwnedLocallyInHierarchy)
            LocalUpdate();
    }

    void LocalUpdate()
    {
        SetVerticalRotation();
        VerticalRotate();
        RotateBody();
    }

    public void SetInput(Vector2 lookInput)
    {
        xInput = lookInput.x;
        yInput = lookInput.y;
    }

    public void SetVerticalRotation()
    {
        xRotation -= yInput;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

    private void VerticalRotate()
    {
        // transform.localEulerAngles = Vector3.right * xRotation;
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void RotateBody()
    {
        playerBody.Rotate(Vector3.up * xInput);
    }
}
