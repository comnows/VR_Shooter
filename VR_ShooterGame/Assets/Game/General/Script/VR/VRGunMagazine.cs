using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Normal.Realtime;
public class VRGunMagazine : MonoBehaviour
{
    public int bulletCount = 30;

    public XRGrabInteractable gunMagazineGrabInteractable;
    public VRGun vrGun;

    private IEnumerator destroyMagazineCoroutine;

    private void Awake()
    {
        gunMagazineGrabInteractable = GetComponent<XRGrabInteractable>();
        destroyMagazineCoroutine = DestroyMagazineIn(2f);
    }

    private void Start()
    {
        gunMagazineGrabInteractable.hoverEntered.AddListener(CancelDestroyMagazineOnHoverEntered);
        gunMagazineGrabInteractable.hoverExited.AddListener(DestroyMagazineOnHoverExited);
        GameObject.DontDestroyOnLoad(gameObject);
    }

    public void CancelDestroyMagazineOnHoverEntered(HoverEnterEventArgs args)
    {
        StopCoroutine(destroyMagazineCoroutine);
    }

    public void DestroyMagazineOnHoverExited(HoverExitEventArgs args)
    {
        if(args.interactableObject.isHovered)
        {
            Debug.Log("is hovered by something");
            return;
        }

        Debug.Log("isnt hover by anything and start coroutine");

        StartCoroutine(destroyMagazineCoroutine);
    }

    IEnumerator DestroyMagazineIn(float timeInSecond)
    {
        yield return new WaitForSeconds(timeInSecond);

        AddBulletToGunData();
        
        //GameObject.Find("AR Magazine Inventory").GetComponent<VRMagazineGenerator>().CreateMagazine();

        Realtime.Destroy(this.gameObject);
    }

    public void AddBulletToGunData()
    {
        if(bulletCount <= 0){return;}
        if(vrGun.gunData.currentStashAmmo >= vrGun.gunData.maxStashAmmo){return;}

        int stashAmmo = vrGun.gunData.currentStashAmmo + bulletCount;
        vrGun.gunData.currentStashAmmo = Mathf.Min(stashAmmo, vrGun.gunData.maxStashAmmo);
        GameObject.Find("HUD Canvas").GetComponent<UIPlayerBullet>().RefreshPlayerAmmoText(bulletCount,0);
    }

    public void AssignVRGun(GameObject gun)
    {
        vrGun = gun.GetComponent<VRGun>();
    }
}
