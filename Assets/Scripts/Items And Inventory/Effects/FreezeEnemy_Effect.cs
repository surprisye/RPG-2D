using UnityEngine;


[CreateAssetMenu(fileName = "Freeze Enemy Effect",menuName = "Data/Item Effect/Freeze Enemy")]
public class FreezeEnemy_Effect : ItemEffect
{
    [SerializeField] private float duration;


    public override void ExecuteEffect(Transform _respondPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats.currentHealth  < playerStats.GetMaxHealthValue() * .1f)
        {
            if (Inventory.instance.CanUseArmor())
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(_respondPosition.position, 2);

                foreach (var hit in colliders)
                {
                    hit.GetComponent<Enemy>().FreezeTimeFor(duration);
                }
            }
        }
        
    }
}
