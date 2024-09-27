using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ShieldEnemy : Enemy
{
    public override void ReceiveAttack(int damage, GameObject damagesDealer)
    {
        EnemyTypeShieldBehaviorStateManager enemyTypeShieldBehaviorStateManager = gameObject.GetComponent<EnemyTypeShieldBehaviorStateManager>();

        if (enemyTypeShieldBehaviorStateManager.player == null)
        {
            enemyTypeShieldBehaviorStateManager.SetTarget(damagesDealer);
        }

        if (enemyTypeShieldBehaviorStateManager.currentState != "Defence")
        {
            if (gameObject.GetComponent<EnemySyncData>()._enemyHP > 0)
            { 
            TakeDamage(damage,damagesDealer);
            StartCoroutine(CheckCanDefence(enemyTypeShieldBehaviorStateManager));
            }
        }
    }

    private IEnumerator CheckCanDefence(EnemyTypeShieldBehaviorStateManager enemyTypeShieldBehaviorStateManager)
    {
        yield return new WaitForSeconds(0.5f);
        enemyTypeShieldBehaviorStateManager.CheckCanDefence();
    }
}
