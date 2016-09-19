using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour
{

    public GameObject spawn;
    public float spawnRadius = 0f;
    public RectTransform healthbar;
    
    // Use this for initialization
    void Start()
    {
        Vector3 spawnPoint = transform.position + Quaternion.AngleAxis(Random.Range(0f, 365f), Vector3.forward) * (new Vector3(Random.Range(0f, spawnRadius), 0));
        GameObject newSpawn = (GameObject)Instantiate(spawn, spawnPoint, Quaternion.AngleAxis(0, Vector3.forward));
        if (healthbar != null)
        {
            newSpawn.GetComponent<HealthScript>().healthbar = healthbar;
        }
    }

}
