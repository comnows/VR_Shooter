using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BossHPUI : MonoBehaviour
{
    private EnemySyncData enemySyncData;
    public TMP_Text bossHPText;
    void Start()
    {
        enemySyncData = GetComponent<EnemySyncData>();  
    }

    public void UpdateBossHPText()
    {
        bossHPText.text = enemySyncData._enemyHP.ToString();
    }
    
}
