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
        currentHealth -= dmg;

        StartCoroutine(Flicker());

        

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        Destroy(gameObject);
    }

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
