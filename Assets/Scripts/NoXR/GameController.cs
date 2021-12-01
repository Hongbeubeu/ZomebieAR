using UnityEngine;

public class GameController : MonoBehaviour
{
    private SpawnController[] spawnControllers;
    [SerializeField] private CameraLook cameraLook;
    [SerializeField] private ShootEnemy shootEnemy;
    public void StartGame()
    {
        spawnControllers = FindObjectsOfType<SpawnController>();
        foreach (var con in spawnControllers)
        {
            con.InvokeSpawnZombie();
        }
        UIManager.instance.SetActiveInGamePanel(true);
        cameraLook.enabled = true;
        shootEnemy.enabled = true;
    }
}