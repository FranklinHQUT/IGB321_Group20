using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    public GameObject avatar;
    private Animator anim;
    int isDeadHash;
    int isBeingHit;

    public float health = 100.0f;
    private bool dead = false;

    // Movement
    public float moveSpeed = 5;
    private Vector3 playerPosition;
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        anim = avatar.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        isDeadHash = Animator.StringToHash("isDead"); 
        isBeingHit = Animator.StringToHash("isGettingHit"); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.playerDead)
        {
            Movement();
        }
    }

    void Movement()
    {
        playerPosition = transform.position;

        // Forwards and Back
        if (Input.GetKey("w"))
        {
            playerPosition.z = playerPosition.z + moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("s"))
        {
            playerPosition.z = playerPosition.z - moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey("w") && Input.GetKey("left shift")) { playerPosition.z = playerPosition.z + (moveSpeed * 1.02f) * Time.deltaTime; }

        // Strafing
        if (Input.GetKey("a"))
        {
            playerPosition.x = playerPosition.x - moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("d"))
        {
            playerPosition.x = playerPosition.x + moveSpeed * Time.deltaTime;
        }

        // Animation Controls - Move Vector Dot Product
        Vector3 moveVector = (playerPosition - transform.position).normalized;
        float direction = Vector3.Dot(moveVector, transform.forward);

        transform.position = playerPosition;
        rb.velocity = new Vector3(0, 0, 0); // Freeze velocity

        // Check if the player's health is zero or below, and set "isDead" based on StringToHash.
        if (health <= 0)
        {
            anim.SetBool(isDeadHash, true);
            GameManager.instance.playerDead = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void takeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            anim.SetBool(isDeadHash, true);
            GameManager.instance.playerDead = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        anim.SetBool(isBeingHit, true);

    }

    // End of Level Goal Interaction
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {
            GameManager.instance.levelComplete = true;
            StartCoroutine(GameManager.instance.LoadLevel(GameManager.instance.nextLevel));
        }
    }
}
