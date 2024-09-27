using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class GunReload : MonoBehaviour
{
    [SerializeField] private Animator rigControllerAnimator;
    [SerializeField] private Animator armsRigControllerAnimator;
    [SerializeField] private GunAnimationEvents gunAnimationEvents;
    [SerializeField] private GunInput gunInput;
    [SerializeField] private Gun gun;

    [SerializeField] private Transform leftHandTransform;

    private RealtimeView realtimeView;

    private GameObject magazineHand;

    private void Awake()
    {
        realtimeView = GetComponent<RealtimeView>();

        // if(realtimeView.isOwnedLocallyInHierarchy)
        // {
        //     gunAnimationEvents = transform.Find("CameraHolder/CameraRecoil/WeaponCamera/RigLayers").GetComponent<GunAnimationEvents>();
        // }
        // else
        // {
        //     gunAnimationEvents = transform.Find("Soldier/RigLayers").GetComponent<GunAnimationEvents>();
        // }
    }

    void Start()
    {
        if(gunAnimationEvents)
        {
            gunAnimationEvents.GunAnimationEvent.AddListener(OnAnimationEvent);
        }
    }

    void Update()
    {
        // if(realtimeView.isOwnedLocallyInHierarchy)
        // {
            if(gunInput.reloadAction.triggered && gun.CanReload())
            {
                if(gun.gunData.isAmmoLimited && gun.gunData.currentStashAmmo <= 0) return;

                Debug.Log("Reloading");
                rigControllerAnimator.SetTrigger("Reload");
                armsRigControllerAnimator.SetTrigger("Reload");
                StartCoroutine(gun.Reload());
            }
        // }
    }

    private void OnAnimationEvent(string eventName)
    {
        switch(eventName)
        {
            case "Detach_magazine":
                DetachMagazine();
                break;
            case "Attach_magazine":
                AttachMagazine();
                break;
        }
    }

    private void DetachMagazine()
    {
        magazineHand = Instantiate(gun.magazineGameObject, leftHandTransform, true);

        gun.magazineGameObject.SetActive(false);
    }

    private void AttachMagazine()
    {
        gun.magazineGameObject.SetActive(true);
        Destroy(magazineHand);

        gun.gunData.Reload();
        gun.OnGunReload?.Invoke(gun.gunData.currentMagazineAmmo, gun.gunData.currentStashAmmo);

        armsRigControllerAnimator.ResetTrigger("Reload");
    }
}
