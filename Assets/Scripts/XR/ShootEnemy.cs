using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ShootEnemy : MonoBehaviour
{
    private int currentAmmo;
    public Button shootButton;
    public Camera fpsCam;
    public int forceAdd = 300;
    public float damage;
    private float coolDown;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private GameObject shootingEffect;

    [SerializeField] private AudioSource fireSound;
    [SerializeField] AudioSource reloadSound;

    public int CurrentAmmo
    {
        get => currentAmmo;
        set
        {
            currentAmmo = value;
            ChangeAmmoUI();
        }
    }

    private void Awake()
    {
        coolDown = 0.01f;
    }
    private void Start()
    {
        //        shootButton.onClick.AddListener(OnShoot);
        //        AudioSource[] sounds = GetComponents<AudioSource>();
        //        fireSound = sounds[0];
        //        reloadSound = sounds[1];
        CurrentAmmo = 30;
        ChangeAmmoUI();
    }

    private void Update()
    {
        if (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (CurrentAmmo > 0)
            {
                OnShoot();
                coolDown = 0.01f;
            }
        }

        // if (Input.GetKeyDown(KeyCode.Mouse1))
        // {
        //     animator.SetTrigger(Constants.CharatorAnimation.Reload);
        // }

        //        Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward);
    }

    private void OnShoot()
    {
        CurrentAmmo--;
        Debug.Log(CurrentAmmo);
        fireSound.Play();
        animator.SetTrigger(Constants.CharatorAnimation.Fire);

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            var target = hit.transform.GetComponent<ZomebieController>();
            if (target != null)
            {
                target.TakeDamage(damage);
                var bloodEffectGO = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodEffectGO, 0.2f);
            }
            else
            {
                var shootEffectGO = Instantiate(shootingEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(shootEffectGO, 0.2f);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(hit.normal * forceAdd);
            }
        }
    }

    private void ChangeAmmoUI()
    {
        UIManager.instance.SetAmmoText(CurrentAmmo);
        if (CurrentAmmo <= 0)
        {
            animator.SetTrigger(Constants.CharatorAnimation.Reload);
            StartCoroutine(WaitReloadAmmo());
        }
    }

    private IEnumerator WaitReloadAmmo()
    {
        yield return new WaitForSeconds(2f);
        CurrentAmmo = 30;
    }
}