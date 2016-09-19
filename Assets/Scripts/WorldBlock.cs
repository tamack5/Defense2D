using UnityEngine;
using System.Collections;

public class WorldBlock : MonoBehaviour {

    public float worldBlockSize;
    [SerializeField]
    GameObject wallPrefab;
    [SerializeField]
    GameObject worldBlockInterior;
    [SerializeField]
    GameObject worldBockPrefab;
    
	// Use this for initialization
	void Start () {
        // The order is important here...
        GetComponent<BoxCollider2D>().size = new Vector2(worldBlockSize, worldBlockSize);
        worldBlockInterior.GetComponent<BoxCollider2D>().size = new Vector2(worldBlockSize * 0.7f, worldBlockSize * 0.7f);
        worldBlockInterior.GetComponent<CircleCollider2D>().radius = worldBlockSize / 2;
        worldBockPrefab.transform.position = transform.position;

        InitEnvironment();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    // Create new WorldBlock if needed, otherwise just keep walking
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == (int)Constants.LAYERS.PLAYER)
        {
            Vector3 contact = other.transform.position;
            Vector2 axisOfContact = GetAxisOfContact(contact);

            RaycastHit2D ray;
            if (ray = Physics2D.Raycast(contact, axisOfContact, (worldBlockSize * 0.75f), 1 << (int)Constants.LAYERS.WORLDBLOCK))
            {
                // We hit another WorldBlock, don't generate here
                Debug.DrawLine(transform.position, ray.point, Color.red);
                
            }
            else
            {
                // No worldblock found, generate new one 
                Debug.DrawLine(transform.position, ray.point, Color.red);

                GameObject newWorldBlock = Instantiate(worldBockPrefab);
                newWorldBlock.transform.Translate(new Vector3(axisOfContact.x * worldBlockSize, axisOfContact.y * worldBlockSize));
                newWorldBlock.name = "WorldBlock (" + (int)(newWorldBlock.transform.position.x / worldBlockSize) + "," + (int)(newWorldBlock.transform.position.y / worldBlockSize) + ")";
                newWorldBlock.GetComponent<WorldBlock>().worldBockPrefab = worldBockPrefab;
            }
        }
    }

    // We either hit on the top, left, right or bottom of the WorldBlock. Return the axis that we exited on
    // Example: leave at top? return new Vector2(0,1)  --since we are exiting in the positive Y direction
    Vector2 GetAxisOfContact(Vector3 playerLocation)
    {
        Vector2 axisOfContact;

        Vector2 blockSize = GetComponent<BoxCollider2D>().size;
        Vector3 blockLocation = transform.position;
        Vector3 relativePlayerPosition = playerLocation - blockLocation;

        if (Mathf.Abs(relativePlayerPosition.x) >= blockSize.x / 2)
        {
            axisOfContact = new Vector2(Mathf.Sign(relativePlayerPosition.x), 0);
        }
        else //if (Mathf.Abs(relativePlayerPosition.y) >= blockSize.y / 2)
        {
            axisOfContact = new Vector2(0, Mathf.Sign(relativePlayerPosition.y));
        }

        return axisOfContact;
    }

    // Initialize WorldBlock
    void InitEnvironment()
    {
        Debug.Log("Calledit from: " + gameObject.name);
        for (int i = 0; i < 2; i++)
        {

            Vector2 spawnpoint = new Vector2(transform.position.x + Random.Range(-worldBlockSize/2, worldBlockSize/2), transform.position.y + Random.Range(-worldBlockSize / 2, worldBlockSize / 2));
            GameObject newWall = (GameObject)Instantiate(wallPrefab, spawnpoint, Quaternion.AngleAxis(Random.Range(0, 356), Vector3.forward));
            newWall.transform.parent = gameObject.transform;
        }
    }

}
