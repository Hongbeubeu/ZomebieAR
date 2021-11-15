using UnityEngine;
using UnityEngine.UI;

public class ShootEnemy : MonoBehaviour
{
    public Button shootButton;
    public Camera fpsCam;
    private float damage;
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private GameObject shootingEffect;

    private void Start()
    {
        shootButton.onClick.AddListener(OnShoot);
    }

    private void OnShoot()
    {
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
        }
    }
}