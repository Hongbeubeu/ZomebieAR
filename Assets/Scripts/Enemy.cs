using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float health = 100f;
    [SerializeField] private AudioSource bloodHit;

    public void TakeDamage(float damage)
    {
        bloodHit.Play();
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