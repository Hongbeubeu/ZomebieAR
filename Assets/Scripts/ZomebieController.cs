using System.Collections;
using UnityEngine;
using GoogleARCore.Examples.HelloAR;

public class ZomebieController : MonoBehaviour
{
    private AudioSource attackSound;
    private bool isZombieClose;
    public int timeBetweenAttacks = 3;
    private float attackCooldownTimer;
    private bool isZombieAttacking;
    [SerializeReference] private Animator anim;

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
        anim.SetTrigger(Constants.Attack);
        attackSound.Play();
        StartCoroutine(finishAttacking());
    }

    private IEnumerator finishAttacking()
    {
        yield return new WaitForSeconds(1.2f);
        isZombieAttacking = false;
    }
}