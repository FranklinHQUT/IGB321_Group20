using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int isLeftHash;
    int isBackwardHash;
    int isRightHash;
    int isAttackingHash;
    int isMagicHash;    
    int isDeadHash;
    int isBeingHitHash;

    // current level
    int currentLevel = 0;

    // Sword and Player
    public GameObject Kopesh;
    public GameObject avatar;

    // Health and dead?
    public float health = 100.0f;
    private bool dead = false;

    // Attack
    public float areaDamageRadius = 5.0f;
    public float meleeDamage = 10.0f;
    public GameObject raycastAnchor;
    public GameObject magicAnchor;
    public GameObject fireStream;

    // Movement
    public float moveSpeed = 5;
    private Vector3 playerPosition;
    private Rigidbody rb;

    // Anim bools
    private bool isRunning;
    private bool isWalking;
    private bool isLeft;
    private bool isBackward;
    private bool isRight;
    private bool isAttacking;
    private bool isMagic;
    private bool isBeingHit;
    private bool isDead;

    public GameObject playerPrefab;

    ParticleSystem particle;
    public GameObject magicDamage;
    public bool hasRing;
    public int mana = 100;

    void Start()
    {
        animator = GetComponent<Animator>();

        // input
        isWalkingHash = Animator.StringToHash("isForward");
        isRunningHash = Animator.StringToHash("isSprinting");
        isLeftHash = Animator.StringToHash("isLeft");
        isBackwardHash = Animator.StringToHash("isBackward");
        isRightHash = Animator.StringToHash("isRight");
        isAttackingHash = Animator.StringToHash("isAttacking");
        isMagicHash = Animator.StringToHash("isMagic");

        // non-input
        isDeadHash = Animator.StringToHash("isDead");
        isBeingHitHash = Animator.StringToHash("isGettingHit");

        rb = GetComponent<Rigidbody>();
        particle = fireStream.GetComponent<ParticleSystem>();
        particle.Stop();

        // fireStream.GetComponent<ParticleSystem>().Stop();
    }

    void Update()
    {
        if (!GameManager.instance.playerDead)
        {
            HandleInput();
            Movement();
            Shooting();
            UpdateAnimationState();
        }
    }

    void HandleInput()
    {
        isWalking = Input.GetKey("w") && !isRunning;
        isRunning = Input.GetKey("w") && Input.GetKey("left shift");
        isLeft = Input.GetKey("a");
        isBackward = Input.GetKey("s");
        isRight = Input.GetKey("d");
        isAttacking = Input.GetMouseButtonDown(0);
        isMagic = (Input.GetMouseButton(1) && hasRing);
    }

    void Movement()
    {
        playerPosition = transform.position;

        // Forwards and Back
        if (isWalking)
        {
            playerPosition.z = playerPosition.z + moveSpeed * Time.deltaTime;
            // Debug.Log("is walking");
        }
        else if (isBackward)
        {
            playerPosition.z = playerPosition.z - moveSpeed * Time.deltaTime;
            // Debug.Log("is backward");
        }

        // Running
        if (isRunning) { playerPosition.z = playerPosition.z + (moveSpeed * 1.1f) * Time.deltaTime; }// Debug.Log("is running"); }

        // Strafing
        if (isLeft)
        {
            playerPosition.x = playerPosition.x - moveSpeed * Time.deltaTime;
            // Debug.Log("is left");
        }
        else if (isRight)
        {
            playerPosition.x = playerPosition.x + moveSpeed * Time.deltaTime;
            // Debug.Log("is right");
        }
        
        // Attack
        animator.SetBool(isAttackingHash, isAttacking); 

        transform.position = playerPosition;
        rb.velocity = new Vector3(0, 0, 0); // Freeze velocity
    }

    void UpdateAnimationState()
    {
        animator.SetBool(isWalkingHash, isWalking);
        animator.SetBool(isRunningHash, isRunning);
        animator.SetBool(isLeftHash, isLeft);
        animator.SetBool(isBackwardHash, isBackward);
        animator.SetBool(isRightHash, isRight);
        animator.SetBool(isAttackingHash, isAttacking);
        animator.SetBool(isMagicHash, isMagic);        
    }

    void Shooting()
    {
        // if moving at all, set that movement to false if attacking too.  or, if just attacking turn off animations
        if (((isLeft || isRight || isRunning || isWalking || isBackward) && isAttacking) || isAttacking)
        {
            isLeft = false; isRight = false; isRunning = false; isWalking = false; isBackward = false;
            DealAreaDamage();
        }

        if (isMagic && (mana > 0)) 
        { 
            mana -= 1;
            isLeft = false; isRight = false; isRunning = false; isWalking = false; isBackward = false;
            HideKopesh(); 
            Instantiate(magicDamage, magicAnchor.transform.position, transform.rotation);
            particle.Play();
        }
        else { 
            particle.Stop(); 
            isMagic = false;
        }
    }

    public void HideKopesh()
    {
        Kopesh.SetActive(false);
        HideForDelay(0.8f);
        Kopesh.SetActive(true);    

    }

    private IEnumerator HideForDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public void takeDamage(float damage)
    {
        health -= damage; 
        // Debug.Log("is damaged");       
        animator.SetBool(isBeingHitHash, isBeingHit);

        if (health <= 0)
        {
            isDead = true;
            // Debug.Log("is dead");

            animator.SetBool(isDeadHash, isDead);
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isLeftHash, false);
            animator.SetBool(isBackwardHash, false);
            animator.SetBool(isRightHash, false);
            animator.SetBool(isAttackingHash, false);
            animator.SetBool(isMagicHash, false);

            GameManager.instance.playerDead = isDead;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        int nextLevel = currentLevel + 1;

        if (other.tag == "Goal")
        {
            GameManager.instance.levelComplete = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void DealAreaDamage()
    {
        // Define a ray in the forward direction of the player
        Ray ray = new Ray(raycastAnchor.transform.position, raycastAnchor.transform.forward);

        // Create a RaycastHit variable to store information about the hit
        RaycastHit hit;

        // Cast the ray and check for collisions up to a specified distance (e.g., X units)
        if (Physics.Raycast(ray, out hit, areaDamageRadius))
        {
            // Check if the collided object is an enemy
            Enemy enemy = hit.collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                // Deal damage to the enemy
                enemy.takeDamage(meleeDamage);
            }
        }
    }

    public void PickedUpRing() {
        hasRing = true;
    }


    public void Teleport(float x, float y, float z) {
        transform.position = new Vector3(x, y, z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaDamageRadius);
    }

    private IEnumerator DestroyAfterDelay(float delay, GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(delay);
        Destroy(objectToDestroy);
    }
}