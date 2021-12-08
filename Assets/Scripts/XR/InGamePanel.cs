using TMPro;
using UnityEngine;

public class InGamePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI goldText;

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

    public void SetGoldText(int gold)
    {
        goldText.SetText("$" + gold.ToString());
    }
}