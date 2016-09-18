using UnityEngine;
using System.Collections;

public class EnemySpawnerScript : MonoBehaviour {

    public GameObject spawn;
    public int maxNumSpawn = 5;
    int currentNumSpawn = 0;
    public float spawnRadius = 2f;
    public float spawnTimer = 3f;
    public bool isOneTimeSpawn = false;

    float lastSpawnTime = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        lastSpawnTime += Time.deltaTime;

        if (currentNumSpawn < maxNumSpawn && lastSpawnTime >= spawnTimer)
        {
            // Spawn randomly, radially from center
            Vector3 spawnPoint = transform.position + Quaternion.AngleAxis(Random.Range(0f, 365f), Vector3.forward) * (new Vector3(Random.Range(0f, spawnRadius), 0));
            GameObject newSpawn = (GameObject)Instantiate(spawn, spawnPoint, Quaternion.AngleAxis(Random.Range(0f, 365f), Vector3.forward));
            newSpawn.GetComponent<EnemyScript>().SetSpawnedFrom(gameObject);
            currentNumSpawn++;

            lastSpawnTime = 0;
        }
        else if (isOneTimeSpawn && currentNumSpawn >= maxNumSpawn)
        {
            Destroy(gameObject);
        }
	}

    public void SpawnDied()
    {
        currentNumSpawn--;
        lastSpawnTime = 0;
    }
}
