using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRInventoryEvent : MonoBehaviour
{
    public XRSocketInteractorTag inventorySocketInteractor;

    public MeshRenderer meshRenderer;
    public bool isSelect = false;
    
    // Start is called before the first frame update
    void Start()
    {
        inventorySocketInteractor.hoverEntered.AddListener(EnableMeshRenderer);
        inventorySocketInteractor.hoverExited.AddListener(DisableMeshRenderer);

        inventorySocketInteractor.selectEntered.AddListener(DisableMeshRendererSelectEntered);
    }

    private void EnableMeshRenderer(HoverEnterEventArgs args)
    {
        if(inventorySocketInteractor.hasSelection){return;}

        Debug.Log("Interactable hover entered");
        string targetTag = inventorySocketInteractor.targetTag;
        if(args.interactableObject.transform.CompareTag(targetTag))
        {
            meshRenderer.enabled = true;
        }
    }

    private void DisableMeshRenderer(HoverExitEventArgs args)
    {
        Debug.Log("Interactable hover exit");
        meshRenderer.enabled = false;
    }

    private void DisableMeshRendererSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("Interactable select entered");
        meshRenderer.enabled = false;
    }

    private void OnSocketSelectEntered(SelectEnterEventArgs args)
    {
        if(args.interactorObject.hasSelection)
        {
            isSelect = true;
        }
    }

    private void OnSocketSelectExited(SelectExitEventArgs args)
    {
        if(args.interactorObject.hasSelection)
        {
            isSelect = false;
        }
    }
}
