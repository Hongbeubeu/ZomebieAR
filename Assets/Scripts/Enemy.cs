using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject, 1f);
    }
}