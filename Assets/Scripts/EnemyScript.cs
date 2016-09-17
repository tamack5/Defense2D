using UnityEngine;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour {

    public float movementSpeed = 5f;
    public float damage = 100f;
    float playerDetectRadius = 5;
    GameObject[] targets;

    // Time/cron job variables
    float timeSince_CheckTargets = 0;
    static float timeBetween_CheckTargets = 0.1f;
    bool gotTarget = false;

	// Use this for initialization
	void Start () {
        // Get all player objects on creation (HEY! if you make this networked, be sure to update this)
        targets = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

        // Cooldowns 
        timeSince_CheckTargets += Time.deltaTime;

        // Target Finding
        GameObject closestPlayer = null;
        if (IsTimeToCheckTargets())
        {
            closestPlayer = FindClosest(targets);
        }

        if (closestPlayer != null)
        {
            Vector3 direction = gameObject.transform.position - closestPlayer.transform.position;

            // Rotate towards player
            float lookAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.AngleAxis(lookAngle, Vector3.forward);

            // Move towards player
            transform.Translate(-1 * direction.normalized * Time.deltaTime * movementSpeed, Space.World);
        }

	}


    // Return the closest gameobject in the list (by vector magnitude)
    GameObject FindClosest(GameObject[] targetList)
    {
        GameObject closest = null;
        foreach (GameObject target in targetList)
        {
            if (closest == null || (transform.position.magnitude - target.transform.position.magnitude) < closest.transform.position.magnitude)
            {
                closest = target;
            }
        }

        return closest;
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
}
