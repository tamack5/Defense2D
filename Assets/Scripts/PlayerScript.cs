using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    float timeLastShot;
    float timeBetweenShot;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    //Time/cron
    float timeSince_TakeDamage = 0;
    float timeBetween_TakeDamage = 1f;

	// Use this for initialization
	void Start () {
        timeLastShot = -1;
        timeBetweenShot = 0.1f;
    }
	
	// Update is called once per frame
	void Update () {

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
            Debug.Log("Ray: Cast! =" + Input.mousePosition + " me: " + transform.position);
            float theta = Mathf.Atan2(mouse.y, mouse.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(theta, Vector3.forward);
        }


        //Quaternion attempt = Quaternion.AngleAxis(Mathf.Atan(Vector3.Angle(Input.mousePosition, Vector3.up)), Vector3.forward);
        //Debug.Log(Vector3.Angle(Input.mousePosition, Vector3.up));
        //transform.rotation = Quaternion.AngleAxis(Mathf.Atan(Vector3.Angle(Input.mousePosition, Vector3.up)), Vector3.forward);

        Camera.main.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.gameObject.transform.position.z);

        // Cooldowns
        timeLastShot += Time.deltaTime;
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
        if (timeLastShot >= timeBetweenShot || timeLastShot < 0)
        {
            var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            bullet.GetComponent<BulletScript>().sourceName = gameObject.name;

            bullet.GetComponent<Rigidbody2D>().velocity = bulletSpawn.up * 8f;

            timeLastShot = 0;
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        Debug.Log("Collided!");
        if (other.gameObject.name.Contains("Wall"))
        {
            //...stop?
        }
        else if (other.gameObject.tag == "Enemy")
        {
            if (IsTimeToTakeDamage())
            {
                Debug.Log("It's damage time");
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
}
