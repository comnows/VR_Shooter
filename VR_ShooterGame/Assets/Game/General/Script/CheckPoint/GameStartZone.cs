using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
        GameObject [] allPlayerInGame = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in allPlayerInGame)
        {
            PlayerSyncData playerSyncData = player.GetComponent<PlayerSyncData>();
            Debug.Log("SyncPlayerName = " + playerSyncData._playerName);
            Debug.Log("SyncPlayerScore = " + playerSyncData._playerScore);
            Debug.Log("SyncPlayerHP = " + playerSyncData._playerHP);
        }
        //string connectedPlayerName = "Player " + allPlayerInGame.Length;
        //other.GetComponent<PlayerSyncData>().ChangedPlayerName(connectedPlayerName);
        //connectedPlayer.name = "Player " + allPlayerInGame.Length;
        }
    }
}
