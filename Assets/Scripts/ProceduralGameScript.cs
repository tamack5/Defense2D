using UnityEngine;
using System.Collections;

public class ProceduralGameScript : MonoBehaviour
{
    GameObject player;

    [SerializeField]
    GameObject enemySpawner;

    //Time/cron
    float timeSince_SpawnedEnemy = 0f;
    float timeBetween_SpawnedEnemy = 1f;


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // Initial spawns
        for (int i = 0; i < 100; i++)
        {
            CreateSpawn();
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        timeSince_SpawnedEnemy += Time.deltaTime;
        if (IsTimeToSpawnEnemy())
        {
            Debug.Log("Creating Spawn");
            CreateSpawn();
        }

    }

    void CreateSpawn()
    {
        Vector3 spawnpoint = new Vector3(player.transform.position.x + (Random.Range(-20, 20) + 5), player.transform.position.y + (Random.Range(-20, 20) + 5), 0);
        GameObject spawn = (GameObject)Instantiate(enemySpawner, spawnpoint, transform.rotation);
        EnemySpawnerScript spawnSettings = enemySpawner.GetComponent<EnemySpawnerScript>();
        spawnSettings.isOneTimeSpawn = true;
        spawnSettings.maxNumSpawn = (int)Mathf.Round(Random.Range(3, 10));
        spawnSettings.spawnRadius = Random.Range(0, 5);
        spawnSettings.spawnTimer = 0;
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
}
