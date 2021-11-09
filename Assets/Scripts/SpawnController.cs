using UnityEngine;


public class SpawnController : MonoBehaviour
{
    public GameObject femaleZombiePrefab;

    public void InvokeSpawnZombie()
    {
        InvokeRepeating(nameof(SpawnZombie), Random.Range(4f, 5f), Random.Range(10f, 15f));
    }

    private void SpawnZombie()
    {
        Vector3 position = gameObject.transform.position;

        GameObject zombieGO = Instantiate(femaleZombiePrefab, new Vector3(position.x, position.y, position.z), Quaternion.Euler(0, Random.Range(0f, 360f), 0));
    }
}
