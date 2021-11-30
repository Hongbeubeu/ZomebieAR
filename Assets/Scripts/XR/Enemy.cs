using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float health = 100f;
    [SerializeField] private AudioSource bloodHit;
    [SerializeField] private Animator animator;

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
        animator.SetTrigger(Constants.Die);
        Destroy(gameObject, 1f);
    }

    public void AwakeAnim()
    {
        animator.SetTrigger(Constants.Awake);
    }
}