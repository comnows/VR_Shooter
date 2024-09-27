using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
public class RealtimeChangeRoom : MonoBehaviour
{
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject bossSpawnPoint;
    private GameObject boss;
    private void OnTriggerEnter(Collider other) {
        Debug.Log("MillTriggerSpawnLoveLove");
        if (other.tag == "Player" && GameObject.FindGameObjectsWithTag("Boss").Length == 0)
        {
            Debug.Log("MillSpawnLoveLove");
            Realtime realtime = GameObject.FindGameObjectWithTag("Realtime").GetComponent<Realtime>();
            var options = new Realtime.InstantiateOptions {
            ownedByClient            = true,    // Make sure the RealtimeView on this prefab is owned by this client.
            preventOwnershipTakeover = true,    // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
            useInstance              = realtime // Use the instance of Realtime that fired the didConnectToRoom event.
            };
            boss = Realtime.Instantiate(bossPrefab.name,options);
            boss.GetComponent<RealtimeTransform>().RequestOwnership();

            Invoke(nameof(ChangeBossPos),1);
            //boss.transform.position = bossSpawnPoint.transform.position;
        }
    }

    private void ChangeBossPos()
    {
        boss.transform.position = bossSpawnPoint.transform.position;
    }
}
