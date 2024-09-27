using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScoreBoard : MonoBehaviour
{
     [SerializeField] private GameObject scoreBoardPanel;
     private TMP_Text playerNameText;
     private TMP_Text playerScoreText;

     void Start()
     {
          GameObject.DontDestroyOnLoad(gameObject);
     }

     public void UpdateScoreBoard()
     {
          scoreBoardPanel.SetActive(true);
          var index = 1;
          GameObject [] allPlayerInGame = GameObject.FindGameObjectsWithTag("Player");
          foreach (GameObject player in allPlayerInGame)
          {
               PlayerSyncData playerSyncData = player.GetComponent<PlayerSyncData>();
               
               GameObject playerNameTextObj = GameObject.Find("PNameText" + index);
               playerNameText = playerNameTextObj.GetComponent<TMP_Text>();

               GameObject playerScoreTextObj = GameObject.Find("PScoreText" + index);
               playerScoreText = playerScoreTextObj.GetComponent<TMP_Text>();
               
               
               playerNameText.text = playerSyncData._playerName;
               playerScoreText.text = playerSyncData._playerScore.ToString();
               index ++;
          }
     }
}
