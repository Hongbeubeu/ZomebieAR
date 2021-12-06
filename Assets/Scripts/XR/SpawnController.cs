using UnityEngine;


public class SpawnController : MonoBehaviour
{
    public GameObject[] zombiePrefabs;

    public void InvokeSpawnZombie()
    {
        InvokeRepeating(nameof(SpawnZombie), Random.Range(4f, 5f), Random.Range(10f, 15f));
    }

    private void SpawnZombie()
    {
        if (GameManager.instance.currentPopulation < GameManager.instance.data.maxPopulation)
        {
            GameManager.instance.currentPopulation++;
            Vector3 position = gameObject.transform.position;
            position.y += 0.1f;
            var index = Random.Range(0, zombiePrefabs.Length);
            GameObject zombieGO = Instantiate(zombiePrefabs[index], new Vector3(position.x, position.y, position.z), Quaternion.Euler(0, Random.Range(0f, 360f), 0));
        }
    }
}

