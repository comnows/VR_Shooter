using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSway : MonoBehaviour
{
    private PlayerMovementInput playerMovementInput;

    [SerializeField] private float maxTurn = 3f;
    [SerializeField] private float rotateSpeed = 4f;

    private void Start() 
    {
        playerMovementInput = GetComponentInParent<PlayerMovementInput>();
    }

    void Update()
    {
        Vector2 mouseInput = playerMovementInput.lookAction.ReadValue<Vector2>();

        SetLocalRotation(GetTargetRotation(mouseInput));
    }

    private Quaternion GetTargetRotation(Vector2 mouse)
    {
        mouse = Vector2.ClampMagnitude(mouse, maxTurn);

        Quaternion rotationX = Quaternion.AngleAxis(-mouse.y, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouse.x, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;
        
        return targetRotation;
    }

    private void SetLocalRotation(Quaternion targetRotation)
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}
