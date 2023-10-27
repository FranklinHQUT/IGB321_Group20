using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Sword and Player
    public GameObject Kopesh;
    public GameObject avatar;

    // Health and dead?
    public float health = 100.0f;
    private bool dead = false;

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

        // start with isRunning as false by def
        isRunning = false;

    }

    void Update()
    {
        if (!GameManager.instance.playerDead)
        {
            HandleInput();
            Movement();
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
        isMagic = Input.GetMouseButtonDown(1);
    }

    void Movement()
    {
        playerPosition = transform.position;

        // Forwards and Back
        if (isWalking)
        {
            playerPosition.z = playerPosition.z + moveSpeed * Time.deltaTime;
            Debug.Log("is walking");
        }
        else if (isBackward)
        {
            playerPosition.z = playerPosition.z - moveSpeed * Time.deltaTime;
            Debug.Log("is backward");
        }

        // Running
        if (isRunning) { playerPosition.z = playerPosition.z + (moveSpeed * 1.1f) * Time.deltaTime; Debug.Log("is running"); }

        // Strafing
        if (isLeft)
        {
            playerPosition.x = playerPosition.x - moveSpeed * Time.deltaTime;
            Debug.Log("is left");
        }
        else if (isRight)
        {
            playerPosition.x = playerPosition.x + moveSpeed * Time.deltaTime;
            Debug.Log("is right");
        }

        // Check for attacking when left mouse button is clicked, check kopesh is turned on, turn on if not already
        if (isAttacking) { animator.SetBool(isAttackingHash, isAttacking); Debug.Log("is attacking"); }
        else if (!isAttacking) { animator.SetBool(isAttackingHash, !isAttacking); }

        // Check for using magic when right mouse button is clicked, turn off Kopesh whilst magicking
        if (Input.GetMouseButtonDown(1) && !isMagic) { animator.SetBool(isMagicHash, isMagic); Debug.Log("is magicking");}
        if (Input.GetMouseButtonUp(1) && isMagic) { animator.SetBool(isMagicHash, !isMagic); }
        if (isMagic) { HideKopesh(true); } else if (!isMagic) { HideKopesh(false); }

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

        HideKopesh(isMagic);
    }

    public void HideKopesh(bool hideOrNot)
    {
        Kopesh.SetActive(!hideOrNot);
    }

    public void takeDamage(float damage)
    {
        health -= damage; 
        Debug.Log("is damaged");       
        animator.SetBool(isBeingHitHash, isBeingHit);

        if (health <= 0)
        {
            isDead = true;
            Debug.Log("is dead");

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
        if (other.tag == "Goal")
        {
            GameManager.instance.levelComplete = true;
            StartCoroutine(GameManager.instance.LoadLevel(GameManager.instance.nextLevel));
        }
    }
}