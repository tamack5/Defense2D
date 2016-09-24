using UnityEngine;
using System.Collections;

public class EnemyCreatesChildren : MonoBehaviour {

    [SerializeField]
    GameObject childSpawn;

    [SerializeField]
    int spawnNumber;
    [SerializeField]
    float spawnPeriod;
    float spawnRadius = 1;

	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnChildren());
	}

    IEnumerator SpawnChildren()
    {
        for(;;)
        {
            for (int i = 0; i < spawnNumber; i++)
            {
                Vector3 spawnPoint = transform.position + Quaternion.AngleAxis(Random.Range(0f, 365f), Vector3.forward) * (new Vector3(Random.Range(0f, spawnRadius), 0));
                Instantiate(childSpawn, spawnPoint, Quaternion.AngleAxis(Random.Range(0f, 365f), Vector3.forward));
            }
            yield return new WaitForSeconds(spawnPeriod);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
