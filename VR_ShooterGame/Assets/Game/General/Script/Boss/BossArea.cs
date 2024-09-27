using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Normal.Realtime;
public class BossArea : MonoBehaviour
{
    private Realtime _realtime;
    private RealtimeView _realtimeView;
    private RealtimeTransform _realtimeTransform;
    public GameObject bossHPUI;
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private GameObject [] spawnPoints;
    [SerializeField] private GameObject [] allPlayerInGame;
    private bool canCheckSpawnEnemy, isSpawning;
    private bool isGameEnd;

    private void Start()
    {
        _realtime = GetComponent<Realtime>();
        _realtimeView = GetComponent<RealtimeView>();
        _realtimeView.SetOwnership(0);
        isGameEnd = false;
    }
    private void Update() 
    {
        if (_realtimeView.isOwnedLocallyInHierarchy)
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            if (canCheckSpawnEnemy && !isSpawning && boss != null)
            {
                CheckSpawnEnemy();
            }
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            //bossHPUI.SetActive(true);
            canCheckSpawnEnemy = true;
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<RealtimeTransform>().isOwnedLocallyInHierarchy)
            {
                GameObject [] boss = GameObject.FindGameObjectsWithTag("Boss");
                GameObject [] bossGuard = GameObject.FindGameObjectsWithTag("BossGuard");
                
                if (boss.Length == 0 && bossGuard.Length == 0 && isGameEnd == false)
                {
                    isGameEnd = true;
                    Debug.Log("MillEndMyGame");
                    GameObject canvas = GameObject.Find("Canvas");
                    canvas.GetComponent<UIPlayerWinGame>().DeleyPlayerWinGame();
                }
            }
        }
    }

    private void CheckSpawnEnemy()
    {
        GameObject enemyInBossArea = GameObject.FindGameObjectWithTag("BossGuard");
        
        if (enemyInBossArea == null)
        {
            isSpawning = true;
            Invoke(nameof(SpawnEnemy),3);
        }
    }

    private void SpawnEnemy()
    {
        allPlayerInGame = GameObject.FindGameObjectsWithTag("Player");
        spawnPoints = GameObject.FindGameObjectsWithTag("BossGuardPos");
        for(int i = 1; i <= allPlayerInGame.Length * 2; i++)
        {
            var options = new Realtime.InstantiateOptions {
            ownedByClient            = true,   
            preventOwnershipTakeover = true,    
            useInstance              = _realtime 
            };

            GameObject bossGuard = Realtime.Instantiate(enemyToSpawn.name, options);
            bossGuard.tag = "BossGuard";
            _realtimeTransform = bossGuard.GetComponent<RealtimeTransform>();
            _realtimeTransform.SetOwnership(0);
            bossGuard.GetComponent<RealtimeView>().ClearOwnership();
            bossGuard.transform.position = spawnPoints[i].transform.position;
            bossGuard.transform.rotation = spawnPoints[i].transform.rotation;
            //SetTarget(bossGuard);
        }

        isSpawning = false;
    }

    // private void SetTarget(GameObject bossGuard)
    // {
    //     allPlayerInGame = GameObject.FindGameObjectsWithTag("Player");
    //     int randNum = Random.Range(0,allPlayerInGame.Length - 1);
    //     EnemyBehaviorStateManager enemyBehaviorStateManager = bossGuard.GetComponent<EnemyBehaviorStateManager>();
    //     if (enemyBehaviorStateManager != null) 
    //     {
    //         if (enemyBehaviorStateManager.player == null)
    //         {
    //             Debug.Log("MillLove " + allPlayerInGame[randNum].name);
    //             enemyBehaviorStateManager.SetTarget(allPlayerInGame[randNum]);
    //         }
    //     }
    // }
}
