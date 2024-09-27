using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Normal.Realtime;
public class PlayerStatus : MonoBehaviour
{
    Animator animator;
    Rig rigBody,rigHand;
    PlayerSyncData playerSyncData;
    private GameObject weaponCamera,rigBodyObj,rigHandObj;
    public int playerHP;
    [SerializeField] private GameObject helpMeText;
    [SerializeField] private GameObject weaponModel;
    RealtimeView _realTimeView; 
    private void Start()
    {
        playerSyncData = GetComponent<PlayerSyncData>();
        _realTimeView = GetComponent<RealtimeView>();

        GameObject cameraHolder = gameObject.transform.GetChild(0).gameObject;
        GameObject soldierModel = gameObject.transform.GetChild(2).gameObject;
        GameObject cameraRecoil = cameraHolder.transform.GetChild(0).gameObject;
        GameObject rigLayer = soldierModel.transform.GetChild(4).gameObject;
        weaponCamera = cameraRecoil.transform.GetChild(0).gameObject;
        rigBodyObj = rigLayer.transform.GetChild(1).gameObject;
        rigHandObj = rigLayer.transform.GetChild(3).gameObject;

        rigBody = rigBodyObj.GetComponent<Rig>(); 
        rigHand = rigHandObj.GetComponent<Rig>(); 

        animator = soldierModel.GetComponentInChildren<Animator>();

        cameraHolder.GetComponent<RealtimeView>().RequestOwnership();
        cameraHolder.GetComponent<RealtimeTransform>().RequestOwnership();
        
    }

    private void Update()
    {
        CheckPlayerHP();
        CheckPlayerStatus();
    }

    private void CheckPlayerHP()
    {
        playerHP = playerSyncData._playerHP;
    }

    private void CheckPlayerStatus()
    {
        if (playerHP <= 0)
        {
            ChangePlayerStatusToDead();
        }
        else
        {
            ChangePlayerStatusToAlive();    
        }
    }

    private void ChangePlayerStatusToDead()
    {
        weaponCamera.SetActive(false);
        weaponModel.SetActive(false);
        rigBody.weight = 0;
        rigHand.weight = 0;
        animator.SetBool("IsDead",true);
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerLookMovement>().enabled = false;
        GetComponent<Gun>().enabled = false;
        helpMeText.SetActive(true);
    }

    private void ChangePlayerStatusToAlive()
    {
        animator.SetBool("IsDead",false);

        if(_realTimeView.isOwnedLocallyInHierarchy)
        {
            weaponCamera.SetActive(true);
        }
        weaponModel.SetActive(true);
        rigBody.weight = 1;
        rigHand.weight = 1;
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerLookMovement>().enabled = true;
        GetComponent<Gun>().enabled = true;
        helpMeText.SetActive(false);
    }

    public void RevivingPlayer()
    {
        playerSyncData.AddPlayerHP(200);
    }
}
