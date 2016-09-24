using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WavesGameScript : MonoBehaviour {

    [SerializeField]
    Text levelTextField;

    [SerializeField]
    GameObject[] enemy1Spawners;
    [SerializeField]
    GameObject[] enemy2Spawners;

    [SerializeField]
    float difficulty;

    EnemySpawnerScript[] enemySpawner1Scripts;
    EnemySpawnerScript[] enemySpawner2Scripts;

    //time/cron
    float timeSince_LevelUp = 0f;
    float timeBetween_LevelUp = 10f;

    int level;

	// Use this for initialization
	void Start () {
        level = 1;

        enemySpawner1Scripts = new EnemySpawnerScript[enemy1Spawners.Length];
        for (int i = 0; i < enemy1Spawners.Length; i++)
        {
            enemySpawner1Scripts[i] = enemy1Spawners[i].GetComponent<EnemySpawnerScript>();
        }

        enemySpawner2Scripts = new EnemySpawnerScript[enemy2Spawners.Length];
        for (int i = 0; i < enemy2Spawners.Length; i++)
        {
            enemySpawner2Scripts[i] = enemy2Spawners[i].GetComponent<EnemySpawnerScript>();
        }
    }
	
	// Update is called once per frame
	void Update () {

        timeSince_LevelUp += Time.deltaTime;

        if (IsTimeToLevelUp())
        {
            levelTextField.text = "Level: " + LevelUp();
        }

	}

    int LevelUp()
    {
        foreach (EnemySpawnerScript script in enemySpawner1Scripts)
        {
            script.maxNumSpawn++;
        }

        level++;

        if (Mathf.RoundToInt(level % 5) == 0)
        {
            foreach (EnemySpawnerScript script in enemySpawner2Scripts)
            {
                script.maxNumSpawn++;
            }
        }

        return level;
    }

    bool IsTimeToLevelUp()
    {
        if (timeSince_LevelUp >= timeBetween_LevelUp)
        {
            timeSince_LevelUp = 0;
            return true;
        }

        return false;
    }
}
