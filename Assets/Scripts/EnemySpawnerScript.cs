using UnityEngine;
using System.Collections;

public class EnemySpawnerScript : MonoBehaviour {

    public GameObject spawn;
    public int totalEnemies;
    public float spawnRadius;
    public float spawnTimer;

    float lastSpawnTime;

	// Use this for initialization
	void Start () {
        lastSpawnTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        lastSpawnTime += Time.deltaTime;

        if (lastSpawnTime >= spawnTimer)
        {
            // Spawn randomly, radially from center
            Vector3 spawnPoint = transform.position + Quaternion.AngleAxis(Random.Range(0f, 365f), Vector3.forward) * (new Vector3(Random.Range(0f, spawnRadius), 0));
            GameObject newSpawn = (GameObject)Instantiate(spawn, spawnPoint, Quaternion.AngleAxis(Random.Range(0f, 365f), Vector3.forward));

            lastSpawnTime = 0;
        }
	}
}
