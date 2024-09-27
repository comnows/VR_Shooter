using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunInput : MonoBehaviour
{
    GunInputActions gunInputActions;

    public InputAction shootAction;
    public InputAction aimAction;
    public InputAction reloadAction;

    public bool ShootInput { get; private set; } = false;

    private void Awake() 
    {
        gunInputActions = new GunInputActions();

        InitGunInputAction();
    }

    private void InitGunInputAction()
    {
        shootAction = gunInputActions.GunControls.Shoot;
        aimAction = gunInputActions.GunControls.Aim;
        reloadAction = gunInputActions.GunControls.Reload;
    }

    private void OnEnable() 
    {
        gunInputActions.GunControls.Enable();

        AddGunActionsListener();
    }

    private void OnDisable() 
    {
        gunInputActions.GunControls.Disable();

        RemoveGunActionsListener();
    }

    void AddGunActionsListener()
    {
        AddShootActionListener();
    }

    void RemoveGunActionsListener()
    {
        RemoveShootActionListener();
    }

    void AddShootActionListener()
    {
        shootAction.performed += SetShootInput;
        shootAction.canceled += SetShootInput;
    }

    void RemoveShootActionListener()
    {
        shootAction.performed -= SetShootInput;
        shootAction.canceled -= SetShootInput;
    }

    void SetShootInput(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() > 0)
        {
            ShootInput = true;
            Debug.Log("Shoot pressed");
        }
        else
        {
            ShootInput = false;
            Debug.Log("Shoot released");
        }
    }

}
