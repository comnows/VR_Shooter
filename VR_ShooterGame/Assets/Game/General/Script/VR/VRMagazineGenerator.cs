using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Normal.Realtime;
public class VRMagazineGenerator : MonoBehaviour
{
    public VRGunMagazine magazinePrefab;
    public XRSocketInteractorTag magazineInventory;

    public VRGun vrGun;

    public Transform createdTransform;

    private VRGunMagazine newMagazine;
    private bool isMagazineInSocket;

    void Start()
    {
        if (gameObject.GetComponent<RealtimeTransform>().isOwnedLocallySelf)
        {
        isMagazineInSocket = false;

        Invoke(nameof(CreateNewMagazine),1.2f);
        //CreateNewMagazine();

        magazineInventory.hoverExited.AddListener(CreateNewMagazineOnHoverExited);
        }
    }

    public void CreateNewMagazine()
    {
        CreateMagazine();
        SetupMagazine();
    }

    public void CreateMagazine()
    {
        Realtime realtime = GameObject.FindGameObjectWithTag("Realtime").GetComponent<Realtime>();
        var options = new Realtime.InstantiateOptions {
        ownedByClient            = true,    // Make sure the RealtimeView on this prefab is owned by this client.
        preventOwnershipTakeover = false,    // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
        useInstance              = realtime // Use the instance of Realtime that fired the didConnectToRoom event.
        };
        newMagazine = Realtime.Instantiate(magazinePrefab.name, createdTransform.position, Quaternion.identity,options).GetComponent<VRGunMagazine>();
        newMagazine.GetComponent<RealtimeTransform>().RequestOwnership();
        //GameObject.DontDestroyOnLoad(newMagazine);
    }

    public void SetupMagazine()
    {
        if(!CanReload()){return;}

        newMagazine.vrGun = vrGun;

        GunData gunData = vrGun.gunData;
        if(IsAmmoLimit())
        {
            newMagazine.bulletCount = Mathf.Min(gunData.magazineSize, gunData.currentStashAmmo);
            RemoveBulletFromStash(newMagazine.bulletCount);
        }
        else
        {
            newMagazine.bulletCount = gunData.magazineSize;
        }
    }

    public bool CanReload()
    {
        if(IsAmmoLimit())
        {
            return vrGun.gunData.currentStashAmmo > 0;
        }

        return true;
    }

    public bool IsAmmoLimit()
    {
        return vrGun.gunData.isAmmoLimited;
    }

    public void RemoveBulletFromStash(int amount)
    {
        vrGun.gunData.currentStashAmmo -= amount;
    }

    // public void CreateNewMagazineOn

    public void CreateNewMagazineOnHoverExited(HoverExitEventArgs args)
    {
        if(args.interactorObject.hasHover){return;}

        // if(args.interactableObject.isHovered)
        // {
        newMagazine = null;
        
        CreateNewMagazine();
        // }

        magazineInventory.hoverExited.RemoveListener(CreateNewMagazineOnHoverExited);
        magazineInventory.selectExited.AddListener(CreateNewMagazineOnSelectExited);
    }

    public void CreateNewMagazineOnStart(SelectExitEventArgs args)
    {
        if(args.interactorObject.hasSelection){return;}

        CreateNewMagazine();

        magazineInventory.selectExited.RemoveListener(CreateNewMagazineOnStart);
        magazineInventory.selectExited.AddListener(CreateNewMagazineOnSelectExited);
    }

    public void CreateNewMagazineOnSelectExited(SelectExitEventArgs args)
    {
        //if(args.interactorObject.hasSelection){return;}
        //Debug.Log("isSelected " + args.interactableObject.isSelected);
        //if(!args.interactableObject.isSelected){return;}

        // if(args.interactableObject.isHovered)
        // {
        // newMagazine = null;
        
        //CreateNewMagazine();
        // }
    }

    public void AssignVRGun(GameObject gun)
    {
        vrGun = gun.GetComponent<VRGun>();
    }

    // private void OnTriggerStay(Collider other) {
    //     if (other.gameObject.tag == "ARMagazine")
    //     {
    //         isMagazineInSocket = true;
    //         Debug.Log("Magazine in Socket");
    //     }
    // }

    // private void OnTriggerExit(Collider other) {
    //     if (other.gameObject.tag == "ARMagazine")
    //     {
    //         isMagazineInSocket = false;
    //         Debug.Log("Magazine out Socket");
            
    //         if (!isMagazineInSocket)
    //         {
    //             Debug.Log("CreateMagazine");
    //             CreateNewMagazine();
    //         }
    //     }
    // }
}


