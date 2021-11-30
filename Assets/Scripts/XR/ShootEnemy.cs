using UnityEngine;
using UnityEngine.UI;

public class ShootEnemy : MonoBehaviour
{
    public Button shootButton;
    public Camera fpsCam;
    public int forceAdd = 300;
    public float damage;
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private GameObject shootingEffect;

    private AudioSource fireSound;
    private AudioSource reloadSound;

    private void Start()
    {
        shootButton.onClick.AddListener(OnShoot);
        AudioSource[] sounds = GetComponents<AudioSource>();
        fireSound = sounds[0];
        reloadSound = sounds[1];
    }

    private void OnShoot()
    {
        fireSound.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            var target = hit.transform.GetComponent<Enemy>();
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
}