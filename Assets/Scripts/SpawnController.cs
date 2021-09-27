using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject femaleZombiePrefab;

    private void Start()
    {
        InvokeRepeating("SpawnZombie", Random.Range(0f, 2f), Random.Range(10f, 15f));
    }

    private void SpawnZombie()
    {
        Vector3 position = gameObject.transform.position;

        GameObject zombieGO = Instantiate(femaleZombiePrefab, new Vector3(position.x, position.y, position.z), Quaternion.Euler(0, Random.Range(0f, 360f), 0));
    }
}