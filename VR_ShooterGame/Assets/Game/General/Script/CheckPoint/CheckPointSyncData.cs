using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class CheckPointSyncData : RealtimeComponent<CheckPointDataModel>
{
    public Vector3 _respawnPointPos;
    //private GameObject[] players;
    private CheckPointManager checkPointManager;
    private RealtimeView _realtimeView;
    private void Awake() 
    {
        _realtimeView = GetComponent<RealtimeView>();  
        _respawnPointPos = GameObject.Find("SpawnPointLV1").transform.position;
        checkPointManager = GetComponent<CheckPointManager>();
    }

    protected override void OnRealtimeModelReplaced(CheckPointDataModel previousModel, CheckPointDataModel currentModel) 
    {
        if (previousModel != null) {
            previousModel.respawnPointPosDidChange -= RespawnPointDidChange;
        }
        
        if (currentModel != null) {
            if (currentModel.isFreshModel)
            {
                currentModel.respawnPointPos = _respawnPointPos;
            }
            UpdateRespawnPoint();
            currentModel.respawnPointPosDidChange += RespawnPointDidChange;
        }
    }

    private void RespawnPointDidChange(CheckPointDataModel model, Vector3 value) 
    {
        UpdateRespawnPoint();
    }
    
    private void UpdateRespawnPoint() 
    {
        _respawnPointPos = model.respawnPointPos;

        //players = GameObject.FindGameObjectsWithTag("Player");  
        Debug.Log(" SyncPos = " + _respawnPointPos);
        checkPointManager.CheckRespawnPlayers();

        // foreach(GameObject player in players)
        // {
        //     player.GetComponent<CheckPointManager>().CheckRespawnPlayers();
        // }
        
    }

    public void SetNewRespawnPoint(Vector3 newRespawnPointPos) 
    {
        model.respawnPointPos = newRespawnPointPos;
    }
}
