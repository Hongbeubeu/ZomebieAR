using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomebieController : MonoBehaviour
{
    private AudioSource attackSound;
    private bool isZombieClose;
    public int timeBetweenAttacks = 3;
    private float attackCooldownTimer;
    private bool isZombieAttacking;
    private void Start()
    {
        attackCooldownTimer = timeBetweenAttacks;
        InitializeSounds();
    }

    private void Update()
    {
        if (isZombieAttacking)
            return;
        Move();

        attackCooldownTimer -= Time.deltaTime;
        if (attackCooldownTimer <= 0 && isZombieClose)
        {
            Attack();
            attackCooldownTimer = timeBetweenAttacks;
        }
    }

    void InitializeSounds()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        attackSound = audioSources[0];
    }

    void Move()
    {
        transform.LookAt(Camera.main.transform.position);
        transform.Translate(Vector3.forward * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            isZombieClose = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            isZombieClose = false;
        }
    }

    void Attack()
    {
        isZombieAttacking = true;
        GetComponent<Animation>().Play("Zombie Attack");
        attackSound.Play();
        StartCoroutine(finishAttacking());
    }

    private IEnumerator finishAttacking()
    {
        yield return new WaitForSeconds(1.2f);
        isZombieAttacking = false;
    }
}
