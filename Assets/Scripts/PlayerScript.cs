using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {


    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float score = 0;

    [SerializeField]
    Text scoreField;

    //Time/cron
    float timeSince_TakeDamage = 0;
    float timeBetween_TakeDamage = 1f;
    float timeSince_Fire = 0;
    float timeBetween_Fire = 0.1f;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

        #region Movement, Rotation & Camera Tracking

        // Player Movement
        float upDown = Input.GetAxis("Vertical") * Time.deltaTime * 5f;
        float leftRight = Input.GetAxis("Horizontal") * Time.deltaTime * 5f;
        transform.Translate(new Vector3(0, upDown, 0), Space.World);
        transform.Translate(new Vector3(leftRight, 0, 0), Space.World);

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
            transform.rotation = Quaternion.AngleAxis(theta, Vector3.forward);
        }
        
        // Camera Tracking
        Camera.main.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.gameObject.transform.position.z);
        
        #endregion


        // Cooldowns
        timeSince_Fire += Time.deltaTime;
        timeSince_TakeDamage += Time.deltaTime;

        // Leftclick?
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

            timeSince_Fire = 0;
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.name.Contains("Wall"))
        {
            //...stop?
        }
        else if (other.gameObject.tag == "Enemy")
        {
            if (IsTimeToTakeDamage())
            {
                gameObject.GetComponent<HealthScript>().TakeDamage(other.gameObject.GetComponent<EnemyScript>().damage);
            }
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
        scoreField.text = score.ToString();
    }
}
