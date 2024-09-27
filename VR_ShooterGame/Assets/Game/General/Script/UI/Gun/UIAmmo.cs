using UnityEngine;
using TMPro;

public class UIAmmo : MonoBehaviour
{
    private Gun gun;
    //private GunSwitching gunSwitching;
    [SerializeField] private TMP_Text magazineAmmoText;
    [SerializeField] private TMP_Text stashAmmoText;

    // private void Awake()
    // {
    //     gun = GameObject.FindObjectOfType<Gun>();
    //     gunSwitching = GameObject.FindObjectOfType<GunSwitching>();
    // }

    // private void Start()
    // {
    //     SetAmmoText(gun.gunData.currentMagazineAmmo, gun.gunData.currentStashAmmo);
    // }
    
    // private void OnEnable()
    // {
    //     gun.OnGunShoot += SetMagazineAmmoText;
    //     gun.OnGunReload += SetAmmoText;
    //     gunSwitching.OnGunSwitch += SetAllAmmoText;
    // }

    public void InitScript(GameObject player)
    {
        gun = player.GetComponent<Gun>();
        //gunSwitching = player.GetComponent<GunSwitching>();
        SetAmmoText(gun.gunData.currentMagazineAmmo, gun.gunData.currentStashAmmo);
        Subscribe();
    }

    private void Subscribe()
    {
        gun.OnGunShoot += SetMagazineAmmoText;
        gun.OnGunReload += SetAmmoText;
        //gunSwitching.OnGunSwitch += SetAllAmmoText;
    }

    private void OnDisable()
    {
        gun.OnGunShoot -= SetMagazineAmmoText;
        gun.OnGunReload -= SetAmmoText;
        //gunSwitching.OnGunSwitch -= SetAllAmmoText;
    }

    private void SetMagazineAmmoText()
    {
        magazineAmmoText.text = gun.gunData.currentMagazineAmmo.ToString();
    }

    private void SetAmmoText(int magazineAmmo, int stashAmmo)
    {
        magazineAmmoText.text = magazineAmmo.ToString();
        stashAmmoText.text = stashAmmo.ToString();
    }

    private void SetAllAmmoText()
    {
        magazineAmmoText.text = gun.gunData.currentMagazineAmmo.ToString();
        stashAmmoText.text = gun.gunData.currentStashAmmo.ToString();
    }
}
