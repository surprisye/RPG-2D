using UnityEngine;
using UnityEngine.Serialization;

public class CharacterStatus : MonoBehaviour
{
    public int damage;
    public int maxHealth;
    
    [SerializeField]private int currentHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        
    }
}
