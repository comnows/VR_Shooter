using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeWeaponManager : MonoBehaviour
{
    public GameObject objWeapon; 
    private BoxCollider weaponColider;
    void Start()
    {
        weaponColider = objWeapon.GetComponent<BoxCollider>();
        weaponColider.enabled = false;
    }

    public void ActivateColider()
    {
        weaponColider.enabled = true;
    }

    public void DeactivateColider()
    {
        weaponColider.enabled = false;
    }
}
