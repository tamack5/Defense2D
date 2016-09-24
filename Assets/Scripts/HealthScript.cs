using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

    public float startHealth = 100;
    public float currentHealth;

    [SerializeField]
    public RectTransform healthbar;


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

        if (healthbar != null)
        {
            healthbar.transform.localScale = new Vector3((currentHealth / startHealth), healthbar.transform.localScale.y, healthbar.transform.localScale.z);
            //healthbar.transform.position = new Vector3(healthbarStartX - (healthbar.transform.localScale.x - 1) * healthbarPosX, healthbar.transform.position.y);
            //Debug.Log(healthbarStartX + " + " + (healthbar.transform.localScale.x - 1) + " * " + healthbarPosX);

            //healthbar.sizeDelta = new Vector2((currentHealth / startHealth), healthbar.sizeDelta.y);
            //healthbar.sizeDelta = new Vector2(healthbar.sizeDelta.x * (currentHealth - startHealth), healthbar.sizeDelta.y);
            //healthbar.transform.position = (healthbar.anchoredPosition - healthbar.rect.xMin)
        }

        if (currentHealth <= 0)
        {
            if (gameObject.layer == (int)Constants.LAYERS.PLAYER)
            {
                // Player death
                Destroy(gameObject);
            }
            else if (gameObject.layer == (int)Constants.LAYERS.ENEMY)
            {
                if (dmgSource != null && dmgSource.GetComponent<PlayerScript>() != null)
                {
                    dmgSource.GetComponent<PlayerScript>().AddScore(GetComponent<EnemyScript>().GetWorthInPoints());
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
