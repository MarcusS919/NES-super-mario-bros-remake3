using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Character : MonoBehaviour
{

    // Makes a private reference to Rigidbody2D Component
    Rigidbody2D rb;

    // Makes a public reference to Rigidbody2D Component
    // - Shown in Inspector
    public Rigidbody2D rb2;

    // Variable to control speed of GameObject
    public float speed;

    // Variable to control jumpForce of GameObject
    public float jumpForce;

    // Variables to tell if Character should jump or not
    public bool isGrounded;
    public LayerMask isGroundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;

    // Makes a private reference to Animator Component
    Animator anim;

    // Used when flipping character
    public bool isFacingRight;

    // Handles projectile spawning
    public Rigidbody2D projectile;          // What to spawn
    public Transform projectileSpawnPoint;  // Where to spawn 
    public float projectileForce;           // How fast to fire it

    // Might be used later on as an add on

    public float SpeedBoost;
    public bool SpeedMode;
    public float powerUpTime;

    int _points;
    public Text scoreText;

    public int Lives;
    public Text LivesText;

    public Transform RespawnPoint;
    public int RespawnWaitTime;

    //Used for Player health
    public float Health=1.0f;

    public GameObject GameOverCanvas;

    // Use this for initialization
    void Start()
    {

        // Assigning 'tags' and 'name' through script
        tag = "Player";
        name = "Player";

        points = 0;
        RespawnWaitTime = 2;

        // Used to get and save a reference to the Rigidbody2D Component
        rb = GetComponent<Rigidbody2D>();

        // Change variables of Rigidbody2D after saving a reference
        rb.mass = 1.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;


        if(Lives<=0 || Lives> 5)
        {
            Lives = 3;
            Debug.LogWarning("Lives not set on" + name + ". Defaulting to " + Lives);
        }

        if (RespawnWaitTime <= 0 || RespawnWaitTime > 5)
        {
            RespawnWaitTime = 1;
            Debug.LogWarning("RespawnWaitTime not set on" + name + ". Defaulting to " + RespawnWaitTime);
        }

        // Check if speed variable was set in the inspector
        if (speed <= 0 || speed > 15.0f)
        {
            // Assign a default value if one was not set
            speed = 5.0f;

            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogWarning("Speed not set on " + name + ". Defaulting to " + speed);
        }

        // Check if jumpForce variable was set in the inspector
        if (jumpForce <= 0 || jumpForce > 10.0f)
        {
            jumpForce = 10.0f;
            Debug.LogWarning("JumpForce not set on " + name + ". Defaulting to " + jumpForce);
        }

        // Check if groundCheckRadius variable was set in the inspector
        if (groundCheckRadius <= 0)
        {
            // Assign a default value if one was not set
            groundCheckRadius = 0.1f;

            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogWarning("Ground Check Radius not set. Defaulting to " + groundCheckRadius);
        }

        // Check if groundCheck variable was set in the inspector
        if (!groundCheck)
        {
            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogError("Ground Check not set in Inspector.");

            // Find gameObject to attach
            groundCheck = GameObject.Find("GroundCheck").GetComponent<Transform>();
        }

        // Used to get and save a reference to the Animator Component
        anim = GetComponent<Animator>();

        // Check if 'anim' variable was set in the inspector
        if (!anim)
        {
            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogError("Animator not found on " + name);
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

        // Check if 'RespawnPoint' variable was set in the inspector
        if (!RespawnPoint)
        {
            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogError("RespawnPoint not found on " + name);
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

        // Check if 'scoreText' variable was set in the inspector
        if (!scoreText)
        {
            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogError("ScoreText not found on " + name);
        }
        else
            scoreText = GameObject.Find("Text_Score").GetComponent<Text>();

        // Check if 'LivesText' variable was set in the inspector
        if (!LivesText)
        {
            // Prints a warning message to the Console
            // - Open Console by going to Window-->Console (or Ctrl+Shift+C)
            Debug.LogError("LivesText not found on " + name);
        }
        else
            LivesText = GameObject.Find("Text_Lives").GetComponent<Text>();


        if (SpeedBoost <= 0)
        {
            SpeedBoost = 10.0f;

            Debug.LogWarning("SpeedBoost not set. Defaulting to " + SpeedBoost);
        }

        if (LivesText) { 
            LivesText.text = "Lives: " + Lives;
    }

    powerUpTime = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if left or right keys are pressed
        // - Gives decimals from -1 to +1
        //float moveValue = Input.GetAxis("Horizontal");

        // - Gives -1, 0, +1
        float moveValue = Input.GetAxisRaw("Horizontal");

        // Check if Character is touching anything labeled as Ground/Platform/Jumpable
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,
            groundCheckRadius, isGroundLayer);

        // Check if Character is grounded
        if (isGrounded)
        {
            // Check if Jump was pressed (aka Space)
            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("Jump");

                //rb.AddForce(new Vector2(0, 10.0f), ForceMode2D.Impulse);

                // Vector2.up --> new Vector2(0,1)
                // Vector2.down --> new Vector2(0,-1)
                // Vector2.left --> new Vector2(-1,0)
                // Vector2.right --> new Vector2(1,0)
                // Vector2.zero --> new Vector2(0,0)
                // Vector2.one --> new Vector2(1,1)

                // Applies a force in UP direction
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        // Check if Space was pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }

        // Check if 'Fire1' input was pressed
        // - Left Ctrl or Left Click
        if (Input.GetButtonDown("Fire1"))
        {
            fire();

            // Puts animator into Attack animation and calls function
            // set in Animation Clip using Animation Events
            anim.SetTrigger("Attack");

        }

        // Make sure Rigidbody2D is attached before doing stuff
        if (rb)
            // Make player move left or right based off moveValue
            rb.velocity = new Vector2(moveValue * speed, rb.velocity.y);

        // Make sure Animator is attached before doing stuff
        if (anim)
        {
            // Activate tranisitions in Animator
            anim.SetBool("Grounded", isGrounded);
            anim.SetFloat("Movement", Mathf.Abs(moveValue));
            anim.SetFloat("Health", Health);
        }

        // Checks if 'Characer' should be flipped or not
        if ((moveValue < 0 && isFacingRight) ||
            (moveValue > 0 && !isFacingRight))
            flip();
    }

    // Check if Two GameObjects collide
    // - Both GameObjects need Colliders
    // - Minimum one needs a Rigidbody2D
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if 'Character' collides with something tagged as 'Collectible'
        if (collision.gameObject.tag == "Collectible")
        {
            // Destroy GameObject 'Character' collides with
            Destroy(collision.gameObject);
        }
    }

    // Check if Two GameObjects overlap
    // - Both GameObjects need Colliders
    // - One Collider should be set to 'Is Trigger'
    // - Minimum one needs a Rigidbody2D
    void OnTriggerEnter2D(Collider2D collision)
    {

        // Check if 'Character' collides with something tagged as 'Collectables'
        if (collision.gameObject.tag == "Collectables")
        {
            // Create a reference to the Script on 'Collectables'
            Collectible c = collision.GetComponent<Collectible>();

            // Check if 'Collectible' Script exists on GameObject being collided with
            if (c)
            {
                // Increase points using Scripts variable
                points += c.points;
            }

            // Delete gameObject that collided with 'Character'
            Destroy(collision.gameObject);
        }

        // Check if player collided with an PowerUp_Grow tag
        if (collision.gameObject.tag == "PowerUp_Grow")
        {
            Debug.Log("Health is " + Health);
            // Begin countdown to losing power
            Health = 2.0f;
            Debug.Log("Health is " + Health);
            anim.SetTrigger("Grow");
           // StartCoroutine(stopGodMode());

            // Delete gameObject that collided with 'Character'
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "PowerUp_Speed")
        {
            // Add the powerup
            speed += SpeedBoost;
            anim.SetTrigger("Run");
            // Begin countdown to losing power
            StartCoroutine(stopSpeedMode());

            // Delete gameObject that collided with 'Character'
            Destroy(collision.gameObject);
        }

        //check if player collided with an enemy tag
        if (collision.gameObject.tag == "Enemy")
        {

            Debug.Log("Health is " + Health);
            Health -= 1.0f;
            Debug.Log("Health is " + Health);

            if (Health <= 0)
            {
                death();
            }
            //play the hit animation if the player have more than 1 health
            anim.SetTrigger("Hit");

        }

        //check if player collided with an 1Hit_KO tag
        if (collision.gameObject.tag == "1Hit_KO")
        {

            Debug.Log("Health is " + Health);
            Health = 0.0f;
            Debug.Log("Health is " + Health);
            death();
        }
    }

    //manages lives, respawning, and play game over screen
    void death()
    {
        Lives -= 1;
        if (LivesText)
        {
            LivesText.text = "Lives: " + Lives;
        }

        StartCoroutine(RespawnWait());

    }

    //flips the player sprite
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

    void fire()
    {
        Debug.Log("Pew Pew");

        // Check if 'projectileSpawnPoint' and 'projectile' exist
        if (projectileSpawnPoint && projectile)
        {
            // Create the 'Projectile' and add to Scene
            Rigidbody2D temp = Instantiate(projectile, projectileSpawnPoint.position,
                projectileSpawnPoint.rotation);

            // Stop 'Character' from hitting 'Projectile'
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(),
                temp.GetComponent<Collider2D>(), true);

            // Check what direction 'Character' is facing before firing
            if (isFacingRight)
                temp.AddForce(projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
            else
                temp.AddForce(-projectileSpawnPoint.right * projectileForce, ForceMode2D.Impulse);
        }

    }


    IEnumerator RespawnWait()
    {
        yield return new WaitForSeconds(RespawnWaitTime);

        Health = 1.0f;
        this.transform.position = RespawnPoint.transform.position;
        if (Lives <= 0)
        {
          //  GameOverCanvas.SetActive(true);
           GameOverCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
            //  SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }

    IEnumerator stopSpeedMode()
    {
        yield return new WaitForSeconds(powerUpTime);

        // Turn off Powerup after specified time
        speed -= SpeedBoost;
    }

    // Create variable and make getter and setter
    public int points
    {
        get
        {
            return _points;
        }
        set
        {
            _points = value;

            if (scoreText)
                scoreText.text = "Score: " + points;
        }
    }
}
