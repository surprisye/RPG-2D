using UnityEngine;

public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect(Transform _respondPosition)
    {
        Debug.Log("Effect executed");
    }
}
