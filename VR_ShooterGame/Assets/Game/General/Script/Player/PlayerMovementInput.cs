using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Normal.Realtime;

public class PlayerMovementInput : MonoBehaviour
{
    PlayerInputActions playerInputActions;
    PlayerMovement playerMovement;
    [SerializeField] PlayerLookMovement playerLookMovement;

    InputAction moveAction;
    public InputAction lookAction;
    InputAction sprintAction;
    public InputAction jumpAction;

    public Vector2 MoveInput { get; private set; }
    Vector2 lookDirection;

    bool isMouse;
    public bool IsSprint { get; private set; }

    private float lookSensitivity = 100f;
    private PlayerSyncData playerSyncData;
    private RealtimeView _realtimeView;

    void Awake()
    {
        _realtimeView = GetComponent<RealtimeView>(); 
        playerInputActions = new PlayerInputActions();
        playerMovement = GetComponent<PlayerMovement>();
        playerSyncData = GetComponent<PlayerSyncData>();  
        // playerLookMovement = GetComponentInChildren<PlayerLookMovement>();

        InitMovementActions();
    }

    private void InitMovementActions()
    {
        moveAction = playerInputActions.PlayerControls.Move;
        lookAction = playerInputActions.PlayerControls.Look;
        jumpAction = playerInputActions.PlayerControls.Jump;
        sprintAction = playerInputActions.PlayerControls.Sprint;
    }

    void OnEnable()
    {
        playerInputActions.PlayerControls.Enable();

        AddMovementActionsListener();
    }

    void OnDisable() 
    {
        playerInputActions.PlayerControls.Disable();

        RemoveMovementActionsListener();
    }

    void AddMovementActionsListener()
    {
        AddMoveActionListener();
        AddLookActionListener();
        AddSprintActionListener();
    }

    void AddMoveActionListener()
    {
        moveAction.performed += SetMoveInput;
        moveAction.canceled += SetMoveInput;
    }

    void AddLookActionListener()
    {
        lookAction.performed += GetLookInput;
        lookAction.canceled += SetLookInput;
    }

    void AddSprintActionListener()
    {
        sprintAction.performed += GetSprintInput;
        sprintAction.canceled += GetSprintInput;
    }

    void RemoveMovementActionsListener()
    {
        RemoveMoveActionListener();
        RemoveLookActionListener();
        RemoveSprintActionListener();
    }

    void RemoveMoveActionListener()
    {
        moveAction.performed -= SetMoveInput;
        moveAction.canceled -= SetMoveInput;
    }

    void RemoveLookActionListener()
    {
        lookAction.performed -= GetLookInput;
        lookAction.canceled -= SetLookInput;
    }

    void RemoveSprintActionListener()
    {
        sprintAction.performed -= GetSprintInput;
        sprintAction.canceled -= GetSprintInput;
    }

    void SetMoveInput(InputAction.CallbackContext context)
    {
        if (_realtimeView.isOwnedLocallyInHierarchy)
        {
            MoveInput = context.ReadValue<Vector2>();
            playerSyncData.ChangedPlayerMoveInput(MoveInput);
        }
    }
    
    void GetLookInput(InputAction.CallbackContext context)
    {
        lookDirection = context.ReadValue<Vector2>() * lookSensitivity * Time.deltaTime;
        // Debug.Log(lookDirection.x);

        isMouse = context.control.device.name == "Mouse";
        Debug.Log(isMouse);

        playerLookMovement.SetInput(lookDirection);
    }

    void SetLookInput(InputAction.CallbackContext context)
    {
        lookDirection = Vector2.zero;

        playerLookMovement.SetInput(lookDirection);
    }

    void GetSprintInput(InputAction.CallbackContext context)
    {
        IsSprint = context.ReadValueAsButton();
    }
}
