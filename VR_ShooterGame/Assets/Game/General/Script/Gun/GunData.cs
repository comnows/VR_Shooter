using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class GunData : ScriptableObject
{
    [Header("General")]
    public new string name;
    public int type; //1-primary 2-secondary 3-special
    public GameObject prefab;
    public AnimationClip gunHoldAnimation;
    public AudioClip shootClip;

    [Header("Basic Properties")]
    public int bulletDamage;
    public float reloadTime;

    [Header("Shooting")]
    public bool isAutoFire;
    public float fireRatePerSecond;
    public float kickback;

    [Header("Aiming")]
    public bool canAimDownSight;
    public float aimSpeed;
    public float aimFieldOfView;
    
    [Header("Ammo")]
    public bool isAmmoLimited;
    public int maxStashAmmo;
    public int magazineSize;
    public int currentStashAmmo;
    public int currentMagazineAmmo;

    // public bool isReload;

    public void Initialize()
    {
        currentStashAmmo = maxStashAmmo;
        currentMagazineAmmo = magazineSize;
    }

    public void RemoveCurrentMagazineAmmo()
    {
        currentMagazineAmmo--;
    }

    public void Reload()
    {
        if(isAmmoLimited)
        {
            currentStashAmmo += currentMagazineAmmo;
            currentMagazineAmmo = Mathf.Min(currentStashAmmo, magazineSize);
            currentStashAmmo -= currentMagazineAmmo;
        }
        else
        {
            currentMagazineAmmo = magazineSize;
        }
    }

    public void AddAmmo(int ammoAmount)
    {
        currentStashAmmo += ammoAmount;
        Debug.Log("Ammo added");
    }

    public bool IsAmmoFull()
    {
        return currentStashAmmo + currentMagazineAmmo >= maxStashAmmo + magazineSize;
    }

}
