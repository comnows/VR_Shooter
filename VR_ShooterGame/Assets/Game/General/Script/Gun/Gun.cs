using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class Gun : MonoBehaviour
{
    public event Action OnGunShoot;
    public Action<int, int> OnGunReload;

    [SerializeField] private GunEffect gunEffect;
    [SerializeField] private GunInput gunInput;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera weaponCamera;
    [SerializeField] private Animator armsRigControllerAnimator;
    [SerializeField] private GameObject bulletHolePrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject player;

    public GameObject magazineGameObject;

    private RealtimeView _realtimeView;
    public GameObject fpsCurrentGun;
    public GameObject currentGun;
    public GunData gunData;
    public GameObject fpsWeaponHolder;
    public GameObject weaponHolder;
    private PlayerSyncData playerSyncData;

    private int damage = 10;
    private float fireRate = 10f;
    private float shotRange = 100;
    private float defaultFieldOfView = 60f;

    private float nextTimeToFire = 0f;
    public bool isAimingDownSight = false;
    private bool isReload = false;

    private void OnEnable()
    {
        OnGunShoot += gunEffect.CastFireEffect;
    }

    private void OnDisable()
    {
        OnGunShoot -= gunEffect.CastFireEffect;
    }

    void Start()
    {
        //gunData.Initialize();
        //weaponHolder = transform.Find("NonVRController/CameraHolder/CameraRecoil/WeaponCamera/WeaponHolder").gameObject;
        playerSyncData = player.GetComponent<PlayerSyncData>();
    }

    private void Awake() 
    {
        _realtimeView = GetComponent<RealtimeView>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // if (_realtimeView.isOwnedLocallyInHierarchy)
        // {
                if(gunData.isAutoFire)
                {
                    if(gunInput.ShootInput && CanShoot())
                    {
                        nextTimeToFire = 1f / gunData.fireRatePerSecond;

                        Shoot();

                        //ApplyKickback();
                    }
                }
                else
                {
                    if(gunInput.shootAction.triggered)
                    {
                        //if(CanShoot())
                        //{
                            //nextTimeToFire = 1f / gunData.fireRatePerSecond;
                            Shoot();
                        //}
                    }
                }

                nextTimeToFire -= Time.deltaTime;

                    //MoveGunToDefaultPosition();

                if(gunInput.aimAction.triggered)
                {
                    if(!gunData.canAimDownSight) return;
                    
                    isAimingDownSight = !isAimingDownSight;
                    armsRigControllerAnimator.SetBool("IsAimDownSight", isAimingDownSight);
                    // StartCoroutine(AimingDownSight());
                }
                
                if(isAimingDownSight)
                {
                    SetFieldOfView(Mathf.Lerp(playerCamera.fieldOfView, gunData.aimFieldOfView, Time.deltaTime * gunData.aimSpeed));
                }
                else
                {
                    SetFieldOfView(Mathf.Lerp(playerCamera.fieldOfView, defaultFieldOfView, Time.deltaTime * gunData.aimSpeed));
                }
                
                // if(gunInput.reloadAction.triggered && CanReload())
                // {
                //     if(gunData.isAmmoLimited && gunData.currentStashAmmo <= 0) return;
                    
                //     Debug.Log("Gun Reload");
                //     StartCoroutine(Reload());
                // }
        // }
    }

    bool CanShoot()
    {
        bool canShoot = nextTimeToFire <= 0 && gunData.currentMagazineAmmo > 0 && !isReload;

        return canShoot;
    }

    public bool CanReload()
    {
        bool canReload = !isReload && gunData.currentMagazineAmmo < gunData.magazineSize && gunData.currentStashAmmo > 0 && gunData.isAmmoLimited;

        return canReload;
    }

    void Shoot()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitInfo, shotRange))
        {
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * shotRange, Color.red, 3);
            AttackTarget target = hitInfo.transform.GetComponent<AttackTarget>();
            Debug.Log(target);

            if (target != null)
            {
                Debug.Log("ReceiveAttack");
                target.ReceiveAttack(gunData.bulletDamage, gameObject);
            }
        }

        PlaySound(gunData.shootClip);
        gunData.RemoveCurrentMagazineAmmo();
        OnGunShoot?.Invoke();
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void ApplyKickback()
    {
        weaponHolder.transform.position += weaponHolder.transform.up * gunData.kickback;
    }

    private void MoveGunToDefaultPosition()
    {
        weaponHolder.transform.localPosition = Vector3.Lerp(weaponHolder.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
    }

    private IEnumerator AimingDownSight()
    {
        Transform anchor = currentGun.transform.Find("Anchor");
        Transform hip = currentGun.transform.Find("AimStates/Hip");
        Transform ads = currentGun.transform.Find("AimStates/ADS");

        float timeInSecond = 0;

        if(isAimingDownSight)
        {
            while(timeInSecond < 1)
            {
                anchor.position = Vector3.Lerp(anchor.position, ads.position, timeInSecond);
                SetFieldOfView(Mathf.Lerp(playerCamera.fieldOfView, gunData.aimFieldOfView, timeInSecond));

                yield return null;

                timeInSecond += gunData.aimSpeed * Time.deltaTime;
            }
        }
        else
        {
            while(timeInSecond < 1)
            {
                anchor.position = Vector3.Lerp(anchor.position, hip.position, gunData.aimSpeed * Time.deltaTime);
                SetFieldOfView(Mathf.Lerp(playerCamera.fieldOfView, defaultFieldOfView, timeInSecond));
                
                yield return null;

                timeInSecond += gunData.aimSpeed * Time.deltaTime;
            }
        }
    }

    private void SetFieldOfView(float fov)
    {
        playerCamera.fieldOfView = fov;
        weaponCamera.fieldOfView = fov;
    }

    public IEnumerator Reload()
    {
        isReload = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        // gunData.Reload();
        // OnGunReload?.Invoke(gunData.currentMagazineAmmo, gunData.currentStashAmmo);

        isReload = false;
    }

    public void CancelReload()
    {
        StopCoroutine(Reload());

        isReload = false;
    }

}
