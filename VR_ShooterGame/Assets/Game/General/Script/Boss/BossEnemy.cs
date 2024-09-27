using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    public override void ReceiveAttack(int damage, GameObject damagesDealer)
    {
        TakeDamage(damage,damagesDealer);
        BossBehaviorStateManager bossBehaviorStateManager = gameObject.GetComponent<BossBehaviorStateManager>();
        if (bossBehaviorStateManager != null) 
        {
            if (bossBehaviorStateManager.player == null)
            {
                bossBehaviorStateManager.SetTarget(damagesDealer);
            }
        }
    }
    public override void TakeDamage(int damage,GameObject damagesDealer)
    {
        if (enemySyncData._enemyHP > 0)
        {
            enemySyncData.ChangeEnemyHP(damage,damagesDealer);
            // BossHPUI bossHPUI = GetComponent<BossHPUI>();
            // bossHPUI.UpdateBossHPText();
        }
    }
}
