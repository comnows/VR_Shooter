using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    private PlayerInteractInput playerInteractInput;

    private Ray interactRay, reviveRay;
    private RaycastHit hitInfo, targetToRevive;

    private float reviveTime;
    private float holdTimer;
    private Color rayColor = Color.blue;
    
    [SerializeField] private float interactRange = 5f, reviveRange = 1.5f;

    private void Awake()
    {
        playerInteractInput = GetComponent<PlayerInteractInput>();
        reviveTime = 3f;
        holdTimer = reviveTime;
    }

    // Update is called once per frame
    void Update()
    {
        rayColor = Color.blue;
        
        interactRay = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if(Physics.Raycast(interactRay, out hitInfo, interactRange)) //can add masking for raycast
        {
            Interactable interactable = hitInfo.transform.GetComponent<Interactable>();
            if (interactable != null)
            {
                //don't forget if key is pressed
                if(playerInteractInput.playerInteractActions.Interact.triggered)
                {
                    interactable.Interact(this.gameObject);
                }
            }

            //rayColor = Color.yellow;
            //Debug.Log(hitInfo.transform.name);
        }

        //Debug.DrawRay(interactRay.origin, interactRay.direction * interactRange, rayColor);
        

        reviveRay = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if(Physics.Raycast(reviveRay, out targetToRevive, reviveRange))
        {
            PlayerStatus playerStatus = targetToRevive.transform.GetComponent<PlayerStatus>();
            if (playerStatus != null && playerStatus.playerHP <=0)
            {
                holdTimer -= Time.deltaTime;
                if (holdTimer < 0)
                {
                    playerStatus.RevivingPlayer();
                }
                rayColor = Color.green;
                Debug.Log(hitInfo.transform.name);
            }
            else
            {
                holdTimer = reviveTime;
            }
        }

        Debug.DrawRay(reviveRay.origin, reviveRay.direction * reviveRange, rayColor);
    }
}
