using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSuperAtkObj : MonoBehaviour
{
    private EnemySyncData enemySyncData;
    private void Awake() 
    {
        enemySyncData = GetComponentInParent<EnemySyncData>();
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player" && enemySyncData._enemyHP > 0)
        {
            other.GetComponent<PlayerSyncData>().DecreasePlayerHP(30);
        }
    }
}
