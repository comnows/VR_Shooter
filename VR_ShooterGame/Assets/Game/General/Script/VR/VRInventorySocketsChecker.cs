using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class VRInventorySocketsChecker : MonoBehaviour
{
    private void Start() 
    {
        RealtimeView _realtimeView = gameObject.GetComponent<RealtimeView>();
        
        if (_realtimeView.isOwnedRemotelyInHierarchy)
        {
            for (int i=0; i < 2; i++)
            {
                GameObject child = this.gameObject.transform.GetChild(i).gameObject;
                child.GetComponent<XRSocketInteractorTag>().enabled = false;
            }
        }    
    }
}
