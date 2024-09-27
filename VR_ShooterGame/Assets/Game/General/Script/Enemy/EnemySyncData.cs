using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class EnemySyncData : RealtimeComponent<EnemySyncDataModel>
{
    public int _enemyHP;
    public string _enemyBehaviorState;
    public string _enemyTarget;
    private GameObject _damageDealer;
    private void Awake() 
    
    {
        if (gameObject.tag == "Boss")
        {
        _enemyHP = 500;
        }
        else
        {
        _enemyHP = 100;
        }
        _enemyBehaviorState = "Idle";
        _enemyTarget = "";
    }

    protected override void OnRealtimeModelReplaced(EnemySyncDataModel previousModel, EnemySyncDataModel currentModel) 
    {
        if (previousModel != null) {
            // Unregister from events
            previousModel.enemyHPDidChange -= EnemyHPDidChange;
            previousModel.enemyBehaviorStateDidChange -= EnemyBehaviorStateDidChange;
            previousModel.enemyTargetDidChange -= EnemyTargetDidChange;
        }
        
        if (currentModel != null) {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                currentModel.enemyHP = _enemyHP;
                currentModel.enemyBehaviorState = _enemyBehaviorState;
                currentModel.enemyTarget =_enemyTarget;
            }
            // Update the mesh render to match the new model
            UpdateEnemyHP();
            UpdateEnemyBehaviorState();
            UpdateEnemyTarget();
            // Register for events so we'll know if the color changes later
            currentModel.enemyHPDidChange += EnemyHPDidChange;
            currentModel.enemyBehaviorStateDidChange += EnemyBehaviorStateDidChange;
            currentModel.enemyTargetDidChange += EnemyTargetDidChange;
        }
    }

    private void EnemyHPDidChange(EnemySyncDataModel model, int value) 
    {
        UpdateEnemyHP();
    }
    
    private void EnemyBehaviorStateDidChange(EnemySyncDataModel model, string value) 
    {
        UpdateEnemyBehaviorState();
    }

    private void EnemyTargetDidChange(EnemySyncDataModel model, string value) 
    {
        UpdateEnemyTarget();
    }

    private void UpdateEnemyHP() 
    {
        _enemyHP = model.enemyHP;

        Debug.Log("EnemyHP = " + _enemyHP);

         if (_enemyHP <= 0)
        {
            EnemyBehaviorStateManager enemyBehaviorStateManager = gameObject.GetComponent<EnemyBehaviorStateManager>();
            EnemyTypeShieldBehaviorStateManager enemyTypeShieldBehaviorStateManager = gameObject.GetComponent<EnemyTypeShieldBehaviorStateManager>();
            EnemyTypeShootBehaviorStateManager enemyTypeShootBehaviorStateManager = gameObject.GetComponent<EnemyTypeShootBehaviorStateManager>();
            BossBehaviorStateManager bossBehaviorStateManager = gameObject.GetComponent<BossBehaviorStateManager>();
            ChangeBehaviorState("Die");

            if (_damageDealer != null)
            {
            PlayerSyncData playerSyncData = _damageDealer.GetComponent<PlayerSyncData>();
            playerSyncData.AddPlayerScore(10);
            playerSyncData.AddPlayerHP(30);
            
            }

            if (enemyBehaviorStateManager != null) 
            {
                enemyBehaviorStateManager.Die();
            }
            else if (enemyTypeShieldBehaviorStateManager != null)
            {
                enemyTypeShieldBehaviorStateManager.Die();
            }
            else if (enemyTypeShootBehaviorStateManager != null)
            {
                enemyTypeShootBehaviorStateManager.Die();
            }
            else if  (bossBehaviorStateManager != null)
            {
                bossBehaviorStateManager.Die();
            }
        } 
    }

    private void UpdateEnemyBehaviorState() 
    {
        _enemyBehaviorState = model.enemyBehaviorState;

        Debug.Log("EnemyBehaviorState = " + _enemyBehaviorState);
    }

    private void UpdateEnemyTarget()
    {
        _enemyTarget = model.enemyTarget;

        Debug.Log("EnemyTarget = " + _enemyTarget);
    }

    public void ChangeEnemyHP(int damage, GameObject damageDealer) 
    {
        _damageDealer = damageDealer;
        model.enemyHP -= damage;
    }

    public void ChangeBehaviorState(string state) 
    {
        model.enemyBehaviorState = state;
    }

    public void ChangeEnemyTarget(string target)
    {
        model.enemyTarget = target;
    }
}
