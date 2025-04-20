using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect",menuName = "Data/Heal Effect")]
public class Heal_Effect : ItemEffect
{
    [Range(0,1f)]
    [SerializeField] private float healPercent;
    
    public override void ExecuteEffect(Transform _respondPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);
        
        playerStats.IncreaseHealthBy(healAmount);
        
    }
}
