using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

    public float startHealth = 100;
    public float currentHealth;

    bool isFlickering;

	// Use this for initialization
	void Start () {
        currentHealth = startHealth;
        isFlickering = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TakeDamage(float dmg)
    {
        TakeDamage(dmg, null);
    }

    public void TakeDamage(float dmg, GameObject dmgSource)
    {
        currentHealth -= dmg;

        StartCoroutine(Flicker());


        if (currentHealth <= 0)
        {
            if (gameObject.tag == "Player")
            {
                // Player death
                Destroy(gameObject);
            }
            else if (gameObject.tag == "Enemy")
            {
                if (dmgSource != null && dmgSource.GetComponent<PlayerScript>() != null)
                {
                    dmgSource.GetComponent<PlayerScript>().AddScore(50);
                }
                gameObject.GetComponent<EnemyScript>().Die();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    // Flicker the color of the damaged object
    IEnumerator Flicker()
    {
        if (!isFlickering)
        {
            isFlickering = true;
            Color originalColor = gameObject.GetComponent<SpriteRenderer>().color;

            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.1f);
            gameObject.GetComponent<SpriteRenderer>().color = originalColor;
            isFlickering = false;
        }
    }
}
