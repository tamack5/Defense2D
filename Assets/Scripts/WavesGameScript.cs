using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WavesGameScript : MonoBehaviour {

    [SerializeField]
    Text levelTextField;

    [SerializeField]
    GameObject[] enemySpawners;

    [SerializeField]
    float difficulty;

    EnemySpawnerScript[] enemySpawnerScripts;

    //time/cron
    float timeSince_LevelUp = 0f;
    float timeBetween_LevelUp = 10f;

    int level;

	// Use this for initialization
	void Start () {
        level = 1;

        enemySpawnerScripts = new EnemySpawnerScript[enemySpawners.Length];
        for (int i = 0; i < enemySpawners.Length; i++)
        {
            enemySpawnerScripts[i] = enemySpawners[i].GetComponent<EnemySpawnerScript>();
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
        foreach (EnemySpawnerScript script in enemySpawnerScripts)
        {
            script.maxNumSpawn++;
        }

        level++;
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
