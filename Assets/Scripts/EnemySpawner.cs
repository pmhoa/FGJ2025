using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float timeBetweenSpawns;
    private float timer = 0;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private GameObject randomizer;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint.position = transform.position + new Vector3(0, 0, distance);
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTimer();
    }

    void SpawnTimer()
    {
        timer += Time.deltaTime;

        if (timer > timeBetweenSpawns)
        {
            timer = 0;
            SpawnEnemy(FindSpawnPosition());
        }
    }

    Vector3 FindSpawnPosition()
    {
        randomizer.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 365f), 0);
        return spawnPoint.position;
    }



    void SpawnEnemy(Vector3 position)
    {
        Debug.Log(position);
        Instantiate(enemy, position, Quaternion.identity);
    }
}