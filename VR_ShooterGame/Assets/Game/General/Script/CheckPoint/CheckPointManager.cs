using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CheckPointManager : MonoBehaviour
{
    private Vector3 currentRespawnPointPos;
    private CheckPointSyncData checkPointSyncData;
    private GameObject[] players;
    
    void Awake()
    {
        checkPointSyncData = GetComponent<CheckPointSyncData>();
        currentRespawnPointPos = checkPointSyncData._respawnPointPos;
    }

    public void CheckUpdateCheckPoint(Vector3 enteredCheckPointPos)
    {
        currentRespawnPointPos = checkPointSyncData._respawnPointPos;
        Debug.Log("BeforePos = " + currentRespawnPointPos);
        if (currentRespawnPointPos != enteredCheckPointPos)
        {
            checkPointSyncData.SetNewRespawnPoint(enteredCheckPointPos);
            currentRespawnPointPos = checkPointSyncData._respawnPointPos; 
            Debug.Log("After Pos = " + currentRespawnPointPos);
        }
    }

    public void CheckRespawnPlayers()
    {
        StartCoroutine(RespawnPlayers());
    }

    private IEnumerator RespawnPlayers()
    {
        yield return new WaitForSeconds (3);

        players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            PlayerSyncData playerSyncData = player.GetComponent<PlayerSyncData>();
            int playerHP = playerSyncData._playerHP;

            if (playerHP <= 0)
            {
                player.GetComponent<PlayerMovement>().enabled = true;
                player.GetComponent<Gun>().enabled = true;
                playerSyncData.AddPlayerHP(100);
                player.transform.position = currentRespawnPointPos;
            }
            //player.GetComponent<CheckPointManager>().CheckRespawnPlayers();
        }
    }
}
