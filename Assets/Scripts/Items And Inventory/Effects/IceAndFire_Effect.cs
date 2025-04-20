using UnityEngine;


[CreateAssetMenu(fileName = "Ice And Fire Effect",menuName = "Data/Item Effect/Ice And Fire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private float newXVelocity;

    public override void ExecuteEffect(Transform _respondPosition)
    {
        Player player = PlayerManager.instance.player;

        bool thirdAttack = player.primaryAttackState.comboCounter == 2;

        if (thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respondPosition.position,player.transform.rotation);
            
            newIceAndFire.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(newXVelocity * player.facingDirection,0);
            Destroy(newIceAndFire,1f); 
        }
        
        
    }
}
