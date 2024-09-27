using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
public class PlayerVROwnership : MonoBehaviour
{
    [SerializeField] private GameObject leftArmIK_target;
    [SerializeField] private GameObject rightArmIK_target;
    [SerializeField] private GameObject head_Constraint;
    [SerializeField] private GameObject arInventory;
    [SerializeField] private GameObject arInventoryMagazine;
    RealtimeTransform _realtimeTransform;
    RealtimeView _realtimeView;
    void Start()
    {
        _realtimeView = GetComponent<RealtimeView>();
        if (_realtimeView.isOwnedLocallyInHierarchy)
        {
            SetOwnerShip();
        }
    }

    void SetOwnerShip()
    {
        _realtimeTransform = GetComponent<RealtimeTransform>();
        _realtimeTransform.RequestOwnership();
        GameObject cameraOffset = transform.GetChild(0).gameObject;
        GameObject playerModel = transform.GetChild(1).gameObject;
        GameObject invetorySocket = transform.GetChild(2).gameObject;

        playerModel.GetComponent<RealtimeTransform>().RequestOwnership();
        playerModel.GetComponent<RealtimeView>().RequestOwnership();

        cameraOffset.GetComponent<RealtimeTransform>().RequestOwnership();
        cameraOffset.GetComponent<RealtimeView>().RequestOwnership();
        
        cameraOffset.transform.position += new Vector3(0,0.8f,0);

        invetorySocket.GetComponent<RealtimeTransform>().RequestOwnership();
        invetorySocket.GetComponent<RealtimeView>().RequestOwnership();

        for (int i = 0; i < 5; i++)
        {
            GameObject cameraOffsetChild = cameraOffset.transform.GetChild(i).gameObject;
            if (i == 0)
            {
                cameraOffsetChild.SetActive(true);
            }
            cameraOffsetChild.GetComponent<RealtimeTransform>().RequestOwnership();
            cameraOffsetChild.GetComponent<RealtimeView>().RequestOwnership();
        } 

        leftArmIK_target.GetComponent<RealtimeTransform>().RequestOwnership();
        leftArmIK_target.GetComponent<RealtimeView>().RequestOwnership();
        rightArmIK_target.GetComponent<RealtimeTransform>().RequestOwnership();
        rightArmIK_target.GetComponent<RealtimeView>().RequestOwnership();
        head_Constraint.GetComponent<RealtimeTransform>().RequestOwnership();
        head_Constraint.GetComponent<RealtimeView>().RequestOwnership();

        arInventory.GetComponent<RealtimeTransform>().RequestOwnership();
        arInventory.GetComponent<RealtimeView>().RequestOwnership();

        arInventoryMagazine.GetComponent<RealtimeTransform>().RequestOwnership();
        arInventoryMagazine.GetComponent<RealtimeView>().RequestOwnership();
        
        GameObject attachTransformMagazine = arInventoryMagazine.transform.GetChild(0).gameObject;
        attachTransformMagazine.GetComponent<RealtimeTransform>().RequestOwnership();
        attachTransformMagazine.GetComponent<RealtimeView>().RequestOwnership();

        GameObject attachTransform = arInventory.transform.GetChild(0).gameObject;
        attachTransform.GetComponent<RealtimeTransform>().RequestOwnership();
        attachTransform.GetComponent<RealtimeView>().RequestOwnership();
    }
}
