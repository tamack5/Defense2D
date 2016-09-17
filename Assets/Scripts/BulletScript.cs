using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public GameObject source = null;
    public float damage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D (Collision2D other)
    {
        // Don't collide with other bullets
        if (other.gameObject.tag != gameObject.tag)
        {
            // Don't collide with the source of this bullet, if there is one
            if (source.gameObject != other.gameObject)
            {
                if (other.gameObject.GetComponent<HealthScript>() != null)
                {
                    other.gameObject.GetComponent<HealthScript>().TakeDamage(damage, source);
                }


                Destroy(gameObject);
            }
        }
    }
}
