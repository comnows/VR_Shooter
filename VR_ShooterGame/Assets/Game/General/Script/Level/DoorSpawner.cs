using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class DoorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject door1Prefab;
    [SerializeField] private GameObject door2Prefab;
    Realtime.InstantiateOptions options;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player" && GameObject.FindGameObjectsWithTag("Door").Length == 0)
        {
            if (other.GetComponent<RealtimeTransform>().isOwnedLocallySelf)
            {
                if (other.GetComponent<RealtimeTransform>().ownerIDInHierarchy == 0)
                {
                    Realtime realtime = GameObject.FindGameObjectWithTag("Realtime").GetComponent<Realtime>();
                    options = new Realtime.InstantiateOptions {
                    ownedByClient            = true,    
                    preventOwnershipTakeover = false,    
                    useInstance              = realtime 
                    };
                    SpawnDoor1();
                    SpawnDoor2();
                }
            }
        }
    }

    private void SpawnDoor1()
    {
        GameObject [] door1Pos = GameObject.FindGameObjectsWithTag("Door1Pos");
        foreach (GameObject doorPos in door1Pos)
        {
            GameObject door = Realtime.Instantiate(door1Prefab.name,options);
            door.GetComponent<RealtimeTransform>().RequestOwnership();
            StartCoroutine(ChangeDoorPos(door,doorPos));
        }
    }

    private void SpawnDoor2()
    {
        GameObject [] door2Pos = GameObject.FindGameObjectsWithTag("Door2Pos");
        if (door2Pos.Length != 0)
        {
            foreach (GameObject doorPos in door2Pos)
            {
                GameObject door = Realtime.Instantiate(door2Prefab.name,options);
                door.GetComponent<RealtimeTransform>().RequestOwnership();
                StartCoroutine(ChangeDoorPos(door,doorPos));
            }
        }
    }


    IEnumerator ChangeDoorPos(GameObject doorToChange,GameObject doorPos)
    {
        yield return new WaitForSeconds(0);
        doorToChange.transform.localScale = doorPos.transform.localScale;
        doorToChange.transform.position = doorPos.transform.position;
        doorToChange.transform.rotation = doorPos.transform.rotation;
    }
}
