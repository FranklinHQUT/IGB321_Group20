using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    NavMeshAgent agent;
    Animator animator;

    // Animations
    int isMovingHash;
    int isAttackingHash;

    float distToPlayer;

    public float stoppingDistance; // 1 for melee so no clip, 5-10 for ranged??

    public GameObject player;

    public float health = 10.0f;
    public float agroRange = 10.0f;
    public float damage = 5.0f;

    // Rotation vars
    public float rotationSpeed;
    private float adjRotSpeed;
    public Quaternion targetRotation;

    // Damage
    private float meleeTimer;
    private float meleeTime = 1.0f;

    // Collision Damage
    private float damageTimer;
    private float damageTime = 0.5f;

    void Start () {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        // Animations
        isMovingHash = Animator.StringToHash("Run");
        isAttackingHash = Animator.StringToHash("Atack_0");

        if (!player) { player = GameObject.FindGameObjectWithTag("Player"); }

    }

    void Update() {
        Behaviour();

        // Kill check - moved from takeDamage due to bug
        if (health <= 0) {
            Destroy(this.gameObject);
        }
    }

    void Behaviour() {
        if (player && !GameManager.instance.playerDead) {
            
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
                    if (distToPlayer >= stoppingDistance) {
                        agent.SetDestination(player.transform.position);
                        animator.SetBool(isMovingHash, true);
                    }
                    // Stop if close to player
                    else if (distToPlayer < stoppingDistance) { // if distance is less than stopping distance
                        agent.SetDestination(transform.position);
                        animator.SetBool(isMovingHash, false);

                        // attack
                        animator.SetBool(isAttackingHash, true);
                        if (Time.time > damageTimer)
                        {
                            hit.transform.GetComponent<PlayerController>().takeDamage(damage);
                            damageTimer = Time.time + meleeTime;;

                        }
                        else { animator.SetBool(isAttackingHash, false); }
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
}
