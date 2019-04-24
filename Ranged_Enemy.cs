using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged_Enemy : MonoBehaviour
{

    public float fireRate;
    public Rigidbody2D projectile;
    public Transform projectileSpawnPoint;
    public float projectileForce;


    Transform target;
    float timeSinceLastFire;
    public bool isFacingRight=false;

    // Makes a private reference to Animator Component
    Animator anim;

    // Use this for initialization
    void Start()
    {
        // Used to get and save a reference to the Animator Component
        anim = GetComponent<Animator>();

        //find the player's object
        target = GameObject.FindWithTag("Player").transform;

        // Check if 'fireRate' variable was set in the inspector
        if (fireRate <= 0)
        {
            // Assign a default value if one was not set
            fireRate = 2.0f;

            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogWarning("fireRate not set. Defaulting to " + fireRate);
        }

        // Check if 'projectile' variable was set in the inspector
        if (!projectile)
        {
            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogError("Projectile not found on " + name);
        }

        // Check if 'projectileSpawnPoint' variable was set in the inspector
        if (!projectileSpawnPoint)
        {
            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogError("ProjectileSpawnPoint not found on " + name);
        }

        // Check if 'projectileForce' variable was set in the inspector
        if (projectileForce <= 0)
        {
            // Assign a default value if one was not set
            projectileForce = 5.0f;

            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogWarning("ProjectileForce not set. Defaulting to " + projectileForce);
        }
    }

    // Update is called once per frame
    void Update()
    {
       if (target.position.x > transform.position.x && isFacingRight != true)
        {
            flip();
        }
       else if(target.position.x < transform.position.x && isFacingRight == true)
        {
            flip();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // coolDown += Time.deltaTime;
            // if(coolDown > fireRate)

            if (Time.time > timeSinceLastFire + fireRate)
            {
                fire();

                timeSinceLastFire = Time.time;
            }
        }
    }

    void fire()
    {
        anim.SetTrigger("attack");
        // Check if 'projectileSpawnPoint' and 'projectile' exist
        if (projectileSpawnPoint && projectile)
        {
            // Create the 'Projectile' and add to Scene
            Rigidbody2D temp = Instantiate(projectile, projectileSpawnPoint.position,
                projectileSpawnPoint.rotation);

            // Stop 'Character' from hitting 'Projectile'
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(),
                temp.GetComponent<Collider2D>(), true);

            //temp.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
            //temp.tag = "Enemy_Projectile";

            // Check what direction 'Character' is facing before firing
            if (isFacingRight)
            {
                temp.transform.Rotate(0, 180, 0);
                temp.AddForce(projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
            }
            else
                temp.AddForce(-projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
        }

    }

    void flip()
    {
        // Toggle variable
        isFacingRight = !isFacingRight;

        // Keep a copy of 'localScale' because scale cannot be changed directly
        Vector3 scaleFactor = transform.localScale;

        // Change sign of scale in 'x'
        scaleFactor.x *= -1; // or - -scaleFactor.x

        // Assign updated value back to 'localScale'
        transform.localScale = scaleFactor;

    }
}
