using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int health = 50;
    [SerializeField] protected float moveSpeed = 2f;

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // Default death behavior
        gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        // Default movement or AI logic
    }
}
