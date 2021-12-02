using TMPro;
using UnityEngine;

public class InGamePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI ammoText;

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public void SetAmmoText(int ammo)
    {
        ammoText.SetText(ammo.ToString());
    }

    public void SetHealthText(int healthPoint)
    {
        health.SetText(healthPoint.ToString());
    }
}