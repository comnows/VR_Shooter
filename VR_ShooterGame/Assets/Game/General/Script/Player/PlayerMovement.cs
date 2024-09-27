using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    private PlayerMovementInput playerMovementInput;

    public Animator animator;

    [SerializeField] private float moveSpeed = 12f;
    private Vector3 moveDirection;

    [SerializeField] private float jumpHeight = 1.2f;

    [SerializeField] private float gravityForce = -9.81f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.04f;
    [SerializeField] private LayerMask groundMask;

    float moveSpeedMultiplier;
    private Vector3 gravityVelocity;
    private bool isGrounded;
    private PlayerSyncData playerSyncData;
    private RealtimeView _realtimeView;
    private GameObject weaponModel;

    private void Awake()
    {
        _realtimeView = GetComponent<RealtimeView>();  
        playerSyncData = GetComponent<PlayerSyncData>();  
        playerMovementInput = GetComponent<PlayerMovementInput>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start() {
        if (_realtimeView.isOwnedLocallyInHierarchy)
            LocalStart();

        if (!_realtimeView.isOwnedLocallyInHierarchy)
        {
            weaponModel = this.transform.Find("Soldier/RigLayers/WeaponHolder/WeaponPivot/AssualtRifleModel/AssaultRifle(1)/AR").gameObject;
            int LayerIgnoreRaycast = LayerMask.NameToLayer("Default");
            weaponModel.layer = LayerIgnoreRaycast;

            weaponModel = this.transform.Find("Soldier/RigLayers/WeaponHolder/WeaponPivot/AssualtRifleModel/AssaultRifle(1)/MagazineAR").gameObject;
            weaponModel.layer = LayerIgnoreRaycast;
        }
    }

    private void LocalStart() {
        GetComponent<RealtimeTransform>().RequestOwnership();
        characterController.GetComponent<RealtimeTransform>().RequestOwnership();
    }

    private void Update() 
    {
        if (_realtimeView.isOwnedLocallyInHierarchy)
        {
            SetMoveDirection(playerMovementInput.MoveInput);
            ResetGravity();
            Move();
            Jump();
            SetGravity();
        }
        SetMoveAnimation();
    }

    public void SetMoveDirection(Vector2 moveInput)
    {
        moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
    }

    private void ResetGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = -2f;
        }
    }

    private void Move()
    {
        moveSpeedMultiplier = playerMovementInput.IsSprint && playerMovementInput.MoveInput.y > 0f ? 1.5f : 1f;
        playerSyncData.ChangedPlayerMoveSpeedMultiplier(moveSpeedMultiplier);
        characterController.Move(moveSpeed * moveSpeedMultiplier * Time.deltaTime * moveDirection);
    }

    private void SetMoveAnimation()
    {
        animator.SetFloat("HorizontalMove", playerSyncData._playerMoveInput.x * playerSyncData._playerMoveSpeedMultiplier, 0.05f, Time.deltaTime);
        animator.SetFloat("VerticalMove", playerSyncData._playerMoveInput.y * playerSyncData._playerMoveSpeedMultiplier, 0.05f, Time.deltaTime);
    }

    private void Jump()
    {
        if(playerMovementInput.jumpAction.triggered && isGrounded)
        {
            gravityVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityForce);
        }
    }

    private void SetGravity()
    {
        gravityVelocity.y += gravityForce * Time.deltaTime;

        characterController.Move(gravityVelocity * Time.deltaTime);
    }
}
