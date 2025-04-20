using UnityEngine;


[CreateAssetMenu(fileName = "Thunder Strike Effect",menuName = "Data/Item Effect/Thunder Strike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;
    
    public override void ExecuteEffect(Transform _respondPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab,_respondPosition.position,Quaternion.identity);
        
        Destroy(newThunderStrike,1f);
    }
}
