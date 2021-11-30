using System.Collections;
using UnityEngine;
using GoogleARCore.Examples.HelloAR;

public class ZomebieController : MonoBehaviour
{
    private float health = 100f;
    [SerializeField] private AudioSource bloodHit;
    private AudioSource attackSound;
    private bool isZombieClose;
    public int timeBetweenAttacks = 3;
    private float attackCooldownTimer;
    private bool isZombieAttacking;
    private bool isRunning = false;
    [SerializeReference] private Animator animator;
    [SerializeField] private CapsuleCollider zombieCollider;
    [SerializeField] private Rigidbody zombieRigidbody;

    private void Start()
    {
        attackCooldownTimer = timeBetweenAttacks;
        InitializeSounds();
        AwakeAnim();
        StartCoroutine(WaitToStand());
    }


    private void Update()
    {
        if (!isRunning)
        {
            return;
        }

        if (isZombieAttacking)
        {
            return;
        }

        Move();

        attackCooldownTimer -= Time.deltaTime;
        if (attackCooldownTimer <= 0 && isZombieClose)
        {
            Attack();
            attackCooldownTimer = timeBetweenAttacks;
        }
    }

    private void InitializeSounds()
    {
        var audioSources = GetComponents<AudioSource>();
        attackSound = audioSources[0];
    }

    private void Move()
    {
        transform.LookAt(Vector3.zero);
        transform.Translate(Vector3.forward * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

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
        isRunning = false;
        Destroy(gameObject, 5f);
    }

    public void AwakeAnim()
    {
        animator.SetTrigger(Constants.Awake);
    }

    private IEnumerator WaitToStand()
    {
        yield return new WaitForSeconds(4.43f);
        isRunning = false;
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
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

    private void Attack()
    {
        isZombieAttacking = true;
        animator.SetTrigger(Constants.Attack);
        attackSound.Play();
        StartCoroutine(FinishAttacking());
    }

    private IEnumerator FinishAttacking()
    {
        yield return new WaitForSeconds(1.2f);
        isZombieAttacking = false;
    }
}