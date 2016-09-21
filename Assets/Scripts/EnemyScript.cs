using UnityEngine;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour
{

    public float movementSpeed;
    public float baseDamage;
    public GameObject spawnedFrom;
    public float playerDetectRadius;
    bool playerInRange = false;
    GameObject target;
    List<GameObject> targets;

    // Time/cron job variables
    float timeSince_CheckTargets = 0;
    static float timeBetween_CheckTargets = 0.25f;
    float timeSince_Attack = 0;
    static float timeBetween_Attack = 0.5f;
    bool gotTarget = false;

    // Use this for initialization
    void Start()
    {
        // Get all player objects on creation (HEY! if you make this networked, be sure to update this)
        GetComponent<CircleCollider2D>().radius = playerDetectRadius;
        targets = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        // Cooldowns 
        timeSince_CheckTargets += Time.deltaTime;
        timeSince_Attack += Time.deltaTime;



    }

    void FixedUpdate()
    {
        // Reset angular velocity to zero. If we want to be rotating in the future, might want to change this
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0f;

        if (playerInRange == false )

        // Target Finding
        if (targets.Count > 0 && IsTimeToCheckTargets())
        {
            target = FindClosest();
        }

        // If there is a player
        if (target != null)
        {
            Vector3 direction = target.transform.position - gameObject.transform.position;
            Vector2 direction2D = new Vector2(direction.x, direction.y);

            // Player detection radius (on dection, the detection radius increases, so it's more difficult to run)
            RaycastHit2D cast;
            if (cast = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), direction2D, playerDetectRadius, 1 << 8))
            {
                //Debug.DrawLine(transform.position, cast.point, Color.red);
                playerInRange = true;
            }
            else if (cast = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), direction2D, playerDetectRadius*2, 1 << 8))
            {
                playerInRange = false;
            }

            // If there is a valid player target in range
            if (playerInRange)
            {
                // Rotate towards player
                float lookAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
                gameObject.GetComponent<Rigidbody2D>().MoveRotation(lookAngle);

                // Move towards player
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.normalized.x * Time.deltaTime * movementSpeed, direction.normalized.y * Time.deltaTime * movementSpeed);
                //transform.Translate(-1 * direction.normalized * Time.deltaTime * movementSpeed, Space.World);
            }
        }
    }


    // Return the closest gameobject in the list (by vector magnitude)
    GameObject FindClosest()
    {
        GameObject closest = null;
        foreach (GameObject target in targets)
        {
            if (closest == null || (transform.position.magnitude - target.transform.position.magnitude) < closest.transform.position.magnitude)
            {
                closest = target;
            }
        }

        return closest;
    }

    // While we are colliding with something
    void OnCollisionStay2D (Collision2D other)
    {
        if (other.gameObject.layer == (int)Constants.LAYERS.PLAYER)
        {
            // Hit a player
            if (IsTimeToAttack())
            {
                Attack();
            }
            
        }
    }

    // Something came in range
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.layer == (int)Constants.LAYERS.PLAYER)
        {
            targets.Add(other.gameObject);
        }
    }

    // Something left range
    void OnTriggerExit2D (Collider2D other)
    {
        if (other.gameObject.layer == (int)Constants.LAYERS.PLAYER)
        {
            if (targets.Contains(other.gameObject))
            {
                targets.Remove(other.gameObject);
                target = FindClosest();
            }
           
        }
    }

    // Calculate & Check if we should check targets
    bool IsTimeToCheckTargets()
    {
        if (timeSince_CheckTargets >= timeBetween_CheckTargets)
        {
            timeSince_CheckTargets = 0;
            return true;
        }

        return false;
    }

    bool IsTimeToAttack()
    {
        if (timeSince_Attack >= timeBetween_Attack)
        {
            timeSince_Attack = 0;
            return true;
        }

        return false;
    }

    // Execute an attack
    void Attack()
    {
        if (target.GetComponent<HealthScript>() != null)
        {
            target.GetComponent<HealthScript>().TakeDamage(baseDamage);
        }
    }

    public void Die()
    {
        if (spawnedFrom != null)
        {
            spawnedFrom.GetComponent<EnemySpawnerScript>().SpawnDied();
        }

        Destroy(gameObject);
    }

    public void SetSpawnedFrom(GameObject spawner)
    {
        spawnedFrom = spawner;
    }
}
