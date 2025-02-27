using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Normal.Realtime;
public class UIScore : MonoBehaviour
{
    PlayerSyncData playerSyncData;

    [SerializeField] private TMP_Text playerScoreText;
    
    private void Start() 
    {
        GameObject.DontDestroyOnLoad(gameObject);    
    }
    public void InitScript(GameObject player)
    {
        playerSyncData = player.GetComponent<PlayerSyncData>();
    }
    public void UpdateScoreText(GameObject player)
    {
        if (player.GetComponent<RealtimeView>().isOwnedLocallyInHierarchy)
        {
        playerSyncData = player.GetComponent<PlayerSyncData>();
        playerScoreText.text = playerSyncData._playerScore.ToString();
        }
    }
}
