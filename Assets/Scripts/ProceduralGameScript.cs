using UnityEngine;
using System.Collections;

public class ProceduralGameScript : MonoBehaviour
{
    GameObject player;

    [SerializeField]
    GameObject enemySpawnerPrefab;
    [SerializeField]
    GameObject wallPrefab;
    [SerializeField]
    GameObject worldBlockPrefab;

    [SerializeField]
    bool isSpawningEnemies = true;

    //Time/cron
    float timeSince_SpawnedEnemy = 0f;
    float timeBetween_SpawnedEnemy = 5f;
    float timeSince_GenerateEnv = 0f;
    float timeBetween_GenerateEnv = 1f;


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (enemySpawnerPrefab == null)
        {
            isSpawningEnemies = false;
        }

        // Initial spawns
        for (int i = 0; i < 400; i++)
        {
            CreateSpawn();
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        timeSince_SpawnedEnemy += Time.deltaTime;
        timeSince_GenerateEnv += Time.deltaTime;

        if (IsTimeToSpawnEnemy())
        {
            CreateSpawn();
        }
        if (IsTimeFor(ref timeSince_GenerateEnv, timeBetween_GenerateEnv))
        {
            GenerateEnvironment();
        }
    }

    void CreateSpawn()
    {
        if (isSpawningEnemies)
        {
            Vector3 spawnpoint = new Vector3(player.transform.position.x + (Random.Range(-20, 20) + 5), player.transform.position.y + (Random.Range(-20, 20) + 5), 0);
            GameObject spawn = (GameObject)Instantiate(enemySpawnerPrefab, spawnpoint, transform.rotation);
            EnemySpawnerScript spawnSettings = enemySpawnerPrefab.GetComponent<EnemySpawnerScript>();
            spawnSettings.isOneTimeSpawn = true;
            spawnSettings.maxNumSpawn = (int)Mathf.Round(Random.Range(3, 10));
            spawnSettings.spawnRadius = Random.Range(0, 5);
            spawnSettings.spawnTimer = 0;
        }
    }



    void GenerateEnvironment()
    {

    }

    bool IsTimeToSpawnEnemy()
    {
        if (timeSince_SpawnedEnemy >= timeBetween_SpawnedEnemy)
        {
            timeSince_SpawnedEnemy = 0;
            return true;
        }

        return false;
    }

    bool IsTimeFor(ref float sinceLast, float interval)
    {
        if (sinceLast >= interval)
        {
            sinceLast = 0;
            return true;
        }

        return false;
    }

}
