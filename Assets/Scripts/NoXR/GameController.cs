using UnityEngine;

public class GameController : MonoBehaviour
{
//    private static GameController _instance;
//
//    public static GameController instance
//    {
//        get
//        {
//            if (_instance == null)
//            {
//                _instance = FindObjectOfType<GameController>();
//                return _instance;
//            }
//        }
//    }

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