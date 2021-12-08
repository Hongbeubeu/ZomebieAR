using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private SpawnController[] spawnControllers;
    // [SerializeField] private CameraLook cameraLook;
    [SerializeField] private ShootEnemy shootEnemy;
    [SerializeField] private FirstPersonController firstPersonController;
    // [SerializeField] private 
    public void StartGame()
    {
        spawnControllers = FindObjectsOfType<SpawnController>();
        foreach (var con in spawnControllers)
        {
            con.InvokeSpawnZombie();
        }
        UIManager.instance.SetActiveInGamePanel(true);
        // cameraLook.enabled = true;
        shootEnemy.enabled = true;
        firstPersonController.enabled = true;
        UpdateCurrentGold(GetCurrentGold());
    }

    public int GetCurrentGold()
    {
        return PlayerPrefs.GetInt("Gold", 0);
    }

    public void GainGold(int amount)
    {
        int currentGold = GetCurrentGold();
        currentGold += amount;
        UpdateCurrentGold(currentGold);
    }

    public void UpdateCurrentGold(int currentGold)
    {
        PlayerPrefs.SetInt("Gold", currentGold);
        UIManager.instance.inGamePanel.SetGoldText(currentGold);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void Lose()
    {
        PauseGame();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager.instance.losePanel.SetActive(true);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(1);
        ResumeGame();
    }
}