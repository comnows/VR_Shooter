using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerNameTag : MonoBehaviour
{
    private GameObject player;
    PlayerSyncData playerSyncData;
    TMP_Text nameText;

    private Transform mainCameraTranform;

    private void Start()
    {
        mainCameraTranform = Camera.main.transform;
    }
    private void LateUpdate() 
    {
        transform.LookAt(transform.position + mainCameraTranform.rotation * Vector3.forward, mainCameraTranform.rotation * Vector3.up); 
    }

    public void UpdateNameTag()
    {
        var canvas = transform.parent.gameObject;
        player = canvas.transform.parent.gameObject;
        Debug.Log("PlayerParentTagName = " + player.name);
        playerSyncData = player.GetComponent<PlayerSyncData>();  
        nameText = gameObject.GetComponent<TMP_Text>();
        Debug.Log("This GameObj Name = " + this.gameObject.name);
        nameText.text = playerSyncData._playerName;
    }
}
