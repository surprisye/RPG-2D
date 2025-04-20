using System;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    protected PlayerStats playerStats;
    
    
    void Start()
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();
            
            playerStats.DoMagicDamage(enemyTarget);
        }
        
        
    }
}
