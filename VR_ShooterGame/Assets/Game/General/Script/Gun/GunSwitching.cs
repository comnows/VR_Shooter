using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class GunSwitching : MonoBehaviour
{
    public event Action OnGunSwitch;

    private RealtimeView realtimeView;
    private GunSwitchingInput gunSwitchingInput;
    private GunLoadout gunLoadout;
    private Gun gun;
    [SerializeField] private GunEffect gunEffect;

    [SerializeField] private Animator armsRigControllerAnimator;
    [SerializeField] private Animator rigControllerAnimator;

    [SerializeField] private Transform fpsGunHolder;
    [SerializeField] private Transform gunHolder;

    private int selectedGun = 0;

    private void Awake()
    {
        realtimeView = GetComponent<RealtimeView>();
        gunSwitchingInput = GetComponent<GunSwitchingInput>();
        gunLoadout = GetComponent<GunLoadout>();
        gun = GetComponent<Gun>();
    }

    void Update()
    {
        int previousSelectedGun = selectedGun;
        if(gunSwitchingInput.gunSwitchingControlsActions.PrimaryWeapon.triggered)
        {
            selectedGun = 0;
        }

        if(gunSwitchingInput.gunSwitchingControlsActions.SecondaryWeapon.triggered)
        {
            selectedGun = 1;
        }

        if(gunSwitchingInput.ScrollSwitchInput > 0f)
        {
            selectedGun++;
            selectedGun %= gunLoadout.guns.Length;
            // selectedGun %= 2;

            Debug.Log("Selected gun = " + selectedGun);
        }

        if(gunSwitchingInput.ScrollSwitchInput < 0f)
        {
            selectedGun--;

            if(selectedGun < 0)
            {
                selectedGun = gunLoadout.guns.Length - 1;
                // selectedGun = 1;
            }

            Debug.Log("Selected gun = " + selectedGun);
        }

        if(previousSelectedGun != selectedGun)
        {
            SelectGun();
        }
    }

    private void SelectGun()
    {
        int gunLoadoutIndex = 0;

        foreach(GunData gunData in gunLoadout.guns)
        {
            if(gunLoadoutIndex == selectedGun)
            {
                //destroy current gun
                // GameObject destroyedGun = gunHolder.GetChild(0).gameObject;
                // Destroy(destroyedGun);

                //create selected gun
                //GameObject fpsCreatedGun = Instantiate (gunData.fpsPrefab);
                //EquipFpsWeapon(fpsCreatedGun);

                GameObject createdGun = Instantiate(gunData.prefab);
                //EquipWeapon(createdGun);

                SetupNewGunData(gunData);
                SetupMagazine(createdGun);

                if(realtimeView.isOwnedLocallyInHierarchy)
                {
                    EquipFpsWeapon(createdGun);
                }
                else
                {
                    EquipWeapon(createdGun);
                }
                // gun.currentGun = createdGun;
                // gun.gunData = gunData;

                OnGunSwitch?.Invoke();
            }

            gunLoadoutIndex++;
        }
    }

    private void EquipFpsWeapon(GameObject newFpsGun)
    {
        DestroyGun(gun.fpsCurrentGun);

        SetupNewFpsGun(newFpsGun);

        SetFpsGunTransform();

        SetupOriginGunEffects(newFpsGun);

        PlayFpsEquipAnimation();
    }

    private void EquipWeapon(GameObject newGun)
    {
        DestroyGun(gun.currentGun);

        SetupNewGun(newGun);

        SetGunTransform();

        SetupOriginGunEffects(newGun);

        PlayEquipAnimation();
    }

    private void DestroyGun(GameObject gunGameObject)
    {
        if(gunGameObject)
        {
            Destroy(gunGameObject);
        }
    }

    private void SetupNewFpsGun(GameObject newFpsGun)
    {
        gun.fpsCurrentGun = newFpsGun;
    }

    private void SetupNewGun(GameObject newGun)
    {
        gun.currentGun = newGun;
    }

    private void SetupNewGunData(GunData newGunData)
    {
        gun.gunData = newGunData;
    }

    private void SetupMagazine(GameObject newGun)
    {
        if(gun.gunData.name == "AssaultRifle")
        {
            gun.magazineGameObject = newGun.transform.Find("AssaultRifleModel/MagazineAR").gameObject;
        }
    }

    private void SetFpsGunTransform()
    {
        gun.fpsCurrentGun.transform.parent = gun.fpsWeaponHolder.transform;
        gun.fpsCurrentGun.transform.localPosition = Vector3.zero;
        gun.fpsCurrentGun.transform.localRotation = Quaternion.identity;
    }

    private void SetGunTransform()
    {
        gun.currentGun.transform.parent = gun.weaponHolder.transform;
        gun.currentGun.transform.localPosition = Vector3.zero;
        gun.currentGun.transform.localRotation = Quaternion.identity;
        gun.currentGun.transform.localScale = Vector3.one;
    }

    private void SetupOriginFpsGunEffects(GameObject newFpsGun)
    {
        Transform raycastOrigin = newFpsGun.transform.Find("RaycastOrigin");

    }

    private void SetupOriginGunEffects(GameObject newGun)
    {
        Transform raycastOrigin = newGun.transform.Find("RaycastOrigin");
        Debug.Log("RaycastOrigin is " + raycastOrigin);
        gunEffect.SetRaycastOrigin(raycastOrigin);

        // Transform effectsTransform = newGun.transform.Find("Effects");
        // ParticleSystem muzzleFlash = effectsTransform.Find("MuzzleFlash").GetComponent<ParticleSystem>();
        // ParticleSystem hitEffect = effectsTransform.Find("HitEffect").GetComponent<ParticleSystem>();
        // gunEffect.SetGunEffect(muzzleFlash, hitEffect);
    }

    private void PlayFpsEquipAnimation()
    {
        armsRigControllerAnimator.Play(gun.gunData.name + "_Arms_Equip");
    }

    private void PlayEquipAnimation()
    {
        rigControllerAnimator.Play(gun.gunData.name + "_Equip");
    }
}
