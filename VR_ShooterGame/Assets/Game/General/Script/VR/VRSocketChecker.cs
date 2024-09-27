using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRSocketChecker : MonoBehaviour
{
    
    private bool isMagazineInSocket;
    private void OnTriggerStay(Collider other) {
        Debug.Log("Other.tag =" + other.tag);
        if (other.gameObject.tag == "ARMagazine")
        {
            isMagazineInSocket = true;
            Debug.Log("Magazine in Socket");
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "ARMagazine")
        {
            isMagazineInSocket = false;
            Debug.Log("Magazine out Socket");
            
            if (!isMagazineInSocket)
            {
                Debug.Log("CreateMagazine");
                gameObject.GetComponent<VRMagazineGenerator>().CreateNewMagazine();
            }
        }
    }
}
