using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI bulletNumber;

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}