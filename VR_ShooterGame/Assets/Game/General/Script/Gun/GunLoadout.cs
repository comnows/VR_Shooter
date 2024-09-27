using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLoadout : MonoBehaviour
{
    public GunData[] guns = new GunData[3];

    private void Awake()
    {
        InitAllGuns(); //remove in real game, do this on start the new game only
    }

    private void InitAllGuns()
    {
        foreach(GunData gun in guns)
        {
            if(gun != null)
            {
                gun.Initialize();
            }
        }
    }

    public void AddGun(GunData gun)
    {
        int slot = gun.type - 1;
        guns[slot] = gun;
    }
}
