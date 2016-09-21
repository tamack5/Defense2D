using UnityEngine;
using System.Collections;


public class PlayerScript : MonoBehaviour {


    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    [SerializeField]
    float movementSpeed;
    public float score = 0;

    [SerializeField]
    GameObject GameManager;
    

    //Time/cron
    float timeSince_TakeDamage = 0;
    float timeBetween_TakeDamage = 1f;
    float timeSince_Fire = 0;
    float timeBetween_Fire = 0.1f;

    // Use this for initialization
    void Start ()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        if (GameManager == null)
        {
            Debug.Log("No game manager!");
        }
    }

    void FixedUpdate()
    {
        // Player Movement
        float upDown = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;
        float leftRight = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(leftRight, upDown);
        
        //transform.Translate(new Vector3(0, upDown, 0), Space.World);
        //transform.Translate(new Vector3(leftRight, 0, 0), Space.World);

    }
	
	// Update is called once per frame
	void Update () {

        #region Rotation & Camera Tracking
        // Poor man's mouse turning
        //float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * 300f;
        //transform.Rotate(new Vector3(0, 0, mouseX));

        // Rich man's mouse turning (hard mode)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float mouseHeight;
        Plane groundPlane = new Plane(Vector3.forward, transform.position);
        if (groundPlane.Raycast(ray, out mouseHeight))
        {
            Vector3 mouse = ray.GetPoint(mouseHeight) - transform.position;
            //Debug.Log("Ray: Cast! =" + Input.mousePosition + " me: " + transform.position);
            float theta = Mathf.Atan2(mouse.y, mouse.x) * Mathf.Rad2Deg - 90f;
            gameObject.GetComponent<Rigidbody2D>().MoveRotation(theta);
            //transform.rotation = Quaternion.AngleAxis(theta, Vector3.forward);
        }

        // Camera Tracking
        Camera.main.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.gameObject.transform.position.z);
        
        #endregion


        // Cooldowns
        timeSince_Fire += Time.deltaTime;
        timeSince_TakeDamage += Time.deltaTime;

        // Leftclick
        if (Input.GetKey(KeyCode.Mouse0))
        {
            FireWeapon();
        }

        
    }

    void FireWeapon()
    {
        // When holding down the mouse, determine how fast we should shoot
        if (timeSince_Fire >= timeBetween_Fire || timeSince_Fire < 0)
        {
            var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            bullet.GetComponent<BulletScript>().source = gameObject;

            bullet.GetComponent<Rigidbody2D>().velocity = bulletSpawn.up * 8f;
            Destroy(bullet, 5f);

            timeSince_Fire = 0;
        }
    }

    // Return true if enough time has passed since the last time we took damage
    bool IsTimeToTakeDamage()
    {
        if (timeSince_TakeDamage >= timeBetween_TakeDamage)
        {
            timeSince_TakeDamage = 0;
            return true;
        }

        return false;
    }

    public void AddScore(float points)
    {
        score += points;
        UpdateScore();
    }

    void UpdateScore()
    {
        GameManager.GetComponent<GameManager>().UpdateScore(score);
    }
}
