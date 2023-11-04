using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    NavMeshAgent agent;
    Animator animator;

    public GameObject burning;

    // Animations
    int isMovingHash;
    int isAttackingHash;
    int isDeadHash;
    bool isDead;

    float distToPlayer;

    public float stoppingDistance; // 1 for melee so no clip, 5-10 for ranged??
    public float startingDistance;

    public GameObject player;

    public float health = 10.0f;
    public float agroRange = 10.0f;
    public float damage = 5.0f;
    public bool isRanged;
    public float damageRange;
    public GameObject particleSystem;

    // Rotation vars
    public float rotationSpeed;
    private float adjRotSpeed;
    public Quaternion targetRotation;

    // Damage
    private float meleeTimer;
    public float damageTime = 1.0f;

    // Collision Damage
    private float damageTimer;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        // Animations
        isMovingHash = Animator.StringToHash("Run");
        isAttackingHash = Animator.StringToHash("Atack_0");
        isDeadHash = Animator.StringToHash("Die");

        if (!player) { player = GameObject.FindGameObjectWithTag("Player"); }

    }

    void Update() {
        if (isDead == false) { Behaviour(); }

        // Kill check - moved from takeDamage due to bug
        if (health <= 0)
        {
            isDead = true;
            animator.SetBool(isDeadHash, true);
            animator.SetBool(isMovingHash, false);
            animator.SetBool(isAttackingHash, false);
            StartCoroutine(DestroyPlayerAfterDelay(2.0f));
        }
    }

    void Behaviour() {
        if (player && !GameManager.instance.playerDead && !isDead) {
            
            // Raycast in direction of Player
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -(transform.position - player.transform.position).normalized, out hit, agroRange)) {
                // If Raycast hits player
                distToPlayer = (Vector3.Distance(player.transform.position, transform.position));

                if (hit.transform.tag == "Player") {
                    Debug.DrawLine(transform.position, player.transform.position, Color.red);
                    // Rotate slowly towards player
                    targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                    adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);
                    // Move towards player
                    if (distToPlayer >= startingDistance) {
                        agent.SetDestination(player.transform.position);
                        animator.SetBool(isMovingHash, true);
                        //Debug.Log("started moving");
                    }
                    // Stop if close to player
                    else if (distToPlayer < stoppingDistance) { // if distance is less than stopping distance
                        agent.SetDestination(transform.position);
                        //Debug.Log("stopped moving");
                        animator.SetBool(isMovingHash, false);

                        if (distToPlayer < damageRange)
                        {
                            // attack - attempt to simulate slower attack, so enemy stops, attacks, then only hits if in melee distance
                            animator.SetBool(isAttackingHash, true);
                            //Debug.Log("attacking");
                            if (Time.time > damageTimer)
                            {
                                hit.transform.GetComponent<PlayerController>().takeDamage(damage);
                                if (isRanged) { particleSystem.SetActive(true); }
                                damageTimer = Time.time + damageTime;

                            }
                            else { animator.SetBool(isAttackingHash, false); }
                        }
                    }
                }
            }
            else {
                // If the player is not in agro range, set "Run" parameter to false.
                animator.SetBool(isMovingHash, false);
            }
        }
    }

    public void takeDamage(float thisDamage) {
        health -= thisDamage;
    }

    private IEnumerator DestroyPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
