using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    private int numPlayerInArea;
    private int LV;
    private bool isEnterOtherMap;
    [SerializeField] private GameObject realtimeObj;
    private void Start() 
    {
        numPlayerInArea = 0;
        isEnterOtherMap = false;
    }

    
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player")
        {
            numPlayerInArea -= 1;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {   
            numPlayerInArea += 1;
            Debug.Log("VRPlayerTrigger");
            var playersInGame = GameObject.FindGameObjectsWithTag("Player");
            if(playersInGame.Length == numPlayerInArea)
            {
                foreach(GameObject player in playersInGame)
                {

                    player.GetComponent<PlayerSyncData>().ChangedIsCanEnterNextLV(true);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if (other.tag == "Player" && other.GetComponent<PlayerSyncData>()._playerIsCanEnterNextLV && !isEnterOtherMap)
        {
            Debug.Log("VRPlayerNextLevel");
            isEnterOtherMap = true;
            ActivateBlackBG();
            var playersInGame = GameObject.FindGameObjectsWithTag("Player");
            other.GetComponent<PlayerSyncData>().ChangedIsCanEnterNextLV(false);


            GameObject xrManager = GameObject.Find("XR Interaction Manager");
            GameObject xrDeviceSim = GameObject.Find("XR Device Simulator");
            //GameObject.DontDestroyOnLoad(xrDeviceSim);
            GameObject.DontDestroyOnLoad(xrManager);
            EnterNextLevel(other.gameObject);
        }
    }

    private void EnterNextLevel(GameObject player)
    {
        isEnterOtherMap = false;
        numPlayerInArea = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void ActivateBlackBG()
    {
        GameObject.Find("Canvas").GetComponent<LevelChangingEffect>().ActiveBlackBG();
    }
}
