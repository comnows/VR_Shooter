using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
public class MovePlayerToPoint : MonoBehaviour
{
    [SerializeField] private GameObject movePoint;
    private void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<RealtimeTransform>().ClearOwnership();
            other.transform.position = movePoint.transform.position;
            other.GetComponent<RealtimeTransform>().RequestOwnership();

            if (other.GetComponent<PlayerVROwnership>() != null)
            {
                StartCoroutine(DeleyChangePosition(other.gameObject));
            }
        }
    }

    IEnumerator DeleyChangePosition(GameObject player)
    {
        yield return new WaitForSeconds(1);
        GameObject inventorySockets = player.transform.GetChild(2).gameObject;
            GameObject arInventory = inventorySockets.transform.GetChild(0).gameObject;
            GameObject arMagazineInventory = inventorySockets.transform.GetChild(1).gameObject;

            GameObject arGun = GameObject.FindGameObjectWithTag("ARGun");
            GameObject arMagazine = GameObject.FindGameObjectWithTag("ARMagazine");

            arGun.GetComponent<RealtimeTransform>().ClearOwnership();
            arMagazine.GetComponent<RealtimeTransform>().ClearOwnership();

            arGun.transform.position = arInventory.transform.GetChild(0).gameObject.transform.position;
            arMagazine.transform.position = arMagazineInventory.transform.position;

            arGun.GetComponent<RealtimeTransform>().RequestOwnership();
            arMagazine.GetComponent<RealtimeTransform>().RequestOwnership();
    }
}
