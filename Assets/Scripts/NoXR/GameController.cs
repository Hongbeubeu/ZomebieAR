using UnityEngine;

public class GameController : MonoBehaviour
{
    private SpawnController[] spawnControllers;
    [SerializeField] private CameraLook cameraLook;
    public void StartGame()
    {
        spawnControllers = FindObjectsOfType<SpawnController>();
        foreach (var con in spawnControllers)
        {
            con.InvokeSpawnZombie();
        }

        cameraLook.enabled = true;
    }
}