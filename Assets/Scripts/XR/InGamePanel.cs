using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
}