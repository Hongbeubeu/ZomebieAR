using UnityEngine;

public class Health : MonoBehaviour
{
    private int healthPoint;

    public int HealthPoint
    {
        get => healthPoint;
        set
        {
            healthPoint = value;
            if (healthPoint < 0)
            {
                FindObjectOfType<GameController>().Lose();
                healthPoint = 0;
            }
            SetHealthText();
        }
    }

    private void Awake()
    {
        HealthPoint = 100;
    }

    public void SetHealthText()
    {
        UIManager.instance.inGamePanel.SetHealthText(HealthPoint);
    }

    public void TakeDamage(int damage)
    {
        HealthPoint -= damage;
        UIManager.instance.BloodUIEffect();
    }
}
