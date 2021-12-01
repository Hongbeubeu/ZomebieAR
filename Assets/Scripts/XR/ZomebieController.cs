using System.Collections;
using UnityEngine;
using DG.Tweening;

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
    private bool isDie;
    [SerializeReference] private Animator animator;
    [SerializeField] private CapsuleCollider zombieCollider;
    [SerializeField] private Rigidbody zombieRigidbody;
    [SerializeField] private Transform player;
    Sequence sequence;

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
        attackCooldownTimer = timeBetweenAttacks;
        InitializeSounds();
        AwakeAnim();
        StartCoroutine(WaitToStand());
    }


    private void Update()
    {
        if (!isRunning || isZombieAttacking || isDie)
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
        var lookAt = player.position;
        lookAt.y = transform.position.y;
        // if (sequence == null)
        // {
        //     sequence = DOTween.Sequence();
        //     sequence.Append(transform.DOLookAt(lookAt, 0.5f).SetEase(Ease.Linear)).OnComplete(() => sequence.Kill());
        // }
        transform.LookAt(lookAt);
        transform.Translate(transform.forward * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    public void TakeDamage(float damage)
    {
        bloodHit.Play();
        health -= damage;
        if (health < 0f)
        {
            if (!isDie)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        zombieCollider.enabled = false;
        zombieRigidbody.useGravity = false;
        isRunning = false;
        isDie = true;
        animator.SetTrigger(Constants.CharatorAnimation.Die);
        Destroy(gameObject, 3f);
    }

    public void AwakeAnim()
    {
        animator.SetTrigger(Constants.CharatorAnimation.Awake);
    }

    private IEnumerator WaitToStand()
    {
        yield return new WaitForSeconds(4.43f);
        isRunning = true;
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
        animator.SetTrigger(Constants.CharatorAnimation.Attack);
        attackSound.Play();
        StartCoroutine(FinishAttacking());
    }

    private IEnumerator FinishAttacking()
    {
        yield return new WaitForSeconds(1.2f);
        isZombieAttacking = false;
    }
}