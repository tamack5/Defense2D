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
        
        InitEnvironment();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void MakeWorldBlock(Vector3 position)
    {
        
        GameObject newWorldBlock = (GameObject)Instantiate(worldBockPrefab, position, Quaternion.AngleAxis(0, Vector3.forward));
        newWorldBlock.name = "WorldBlock (" + (int)(position.x / worldBlockSize) + "," + (int)(position.y / worldBlockSize) + ")";
        newWorldBlock.GetComponent<WorldBlock>().worldBockPrefab = worldBockPrefab;

        Debug.Log("Created a worldblock with name: " + newWorldBlock.name);
    }

    void GenerateSurroundingWorldBlocks(Vector3 startingWorldBlock, int inceptionLevels)
    {
        inceptionLevels--;
        Debug.Log(startingWorldBlock);

        foreach (Vector3 checkDirection in new Vector3[] { Vector3.up, Vector3.right, Vector3.down, Vector3.left })
        {
            RaycastHit2D ray;
            Vector3 worldBlockBorder = (startingWorldBlock + checkDirection * worldBlockSize / 2f) + checkDirection;

            if (ray = Physics2D.Raycast(worldBlockBorder, checkDirection, (worldBlockSize * 0.75f), 1 << (int)Constants.LAYERS.WORLDBLOCK))
            {
                Debug.DrawLine((startingWorldBlock + checkDirection * worldBlockSize) + checkDirection, ray.point, Color.red, 5f);
                Debug.Log("hit a worldblock: " + ray.collider.gameObject.transform.position + "with name: " + ray.collider.gameObject.name);
                // We hit ANOTHER worldblock, so we really don't need to spawn here
            }
            else
            {
                Vector3 newPosition = startingWorldBlock + checkDirection * worldBlockSize;
                Debug.DrawLine((startingWorldBlock + checkDirection * worldBlockSize / 2f) + checkDirection, newPosition, Color.red, 5f);
                Debug.Log("did not hit a worldblock");

                MakeWorldBlock(newPosition);
            }
        }
    }

    // Create new WorldBlocks if needed, otherwise just keep walking
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == (int)Constants.LAYERS.PLAYER)
        {
            Vector3 contact = other.transform.position;
            Vector2 axisOfContact = GetAxisOfContact(contact);
            // OH OH make this recursive! More flexibility, and recursion is cool
            RaycastHit2D ray;
            if (ray = Physics2D.Raycast(contact, axisOfContact, (worldBlockSize * 0.75f), 1 << (int)Constants.LAYERS.WORLDBLOCK))
            {
                // We hit another WorldBlock
                Debug.DrawLine(ray.collider.gameObject.transform.position, ray.point, Color.red, 5f);
                Vector3 baseWorldBlock = ray.collider.gameObject.transform.position;

                GenerateSurroundingWorldBlocks(baseWorldBlock, 1);

            }
            else
            {
                // No worldblock found, generate new one 
                // ...but in new design this particular case shouldn't happen

                /*GameObject newWorldBlock = Instantiate(worldBockPrefab);
                newWorldBlock.transform.Translate(new Vector3(axisOfContact.x * worldBlockSize, axisOfContact.y * worldBlockSize));
                newWorldBlock.name = "WorldBlock (" + (int)(newWorldBlock.transform.position.x / worldBlockSize) + "," + (int)(newWorldBlock.transform.position.y / worldBlockSize) + ")";
                newWorldBlock.GetComponent<WorldBlock>().worldBockPrefab = worldBockPrefab;*/
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
        for (int i = 0; i < 1; i++)
        {
            Debug.Log("Made a wall");
            Vector2 spawnpoint = new Vector2(transform.position.x + Random.Range(-worldBlockSize/2, worldBlockSize/2), transform.position.y + Random.Range(-worldBlockSize / 2, worldBlockSize / 2));
            GameObject newWall = (GameObject)Instantiate(wallPrefab, spawnpoint, Quaternion.AngleAxis(Random.Range(0, 356), Vector3.forward));
            newWall.transform.parent = gameObject.transform;
        }
    }

}
