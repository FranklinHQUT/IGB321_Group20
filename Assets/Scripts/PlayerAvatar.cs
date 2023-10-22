using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour {

    public GameObject avatar;
    private Animator anim;

    public float health = 100.0f;
    private bool dead = false;

    //Movement
    public float moveSpeed = 5;
    private Vector3 playerPosition;
    private Rigidbody rb;

    //Weapons
    public GameObject bullet;
    private float MGFireTime = 0.075f;
    private float MGFireTimer;
    public int ammo = 500;

    public GameObject fireDamage;
    private float FTFireTime = 0.2f;
    private float FTFireTimer;
    public int fuel = 50;

    //Weapon Effects
    public GameObject muzzleFlash;
    public GameObject flameStream;

    // Use this for initialization
    void Start () {
        anim = avatar.GetComponent<Animator>();
        flameStream.GetComponent<ParticleSystem>().Stop();
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        if (!GameManager.instance.playerDead) {
            Movement();
            Shooting();
        } 
    }

    void Shooting() {
        //Left Mouse Button
        if (Input.GetMouseButton(0) && ammo >= 1) {

            muzzleFlash.SetActive(true);

            if (Time.time > MGFireTimer) {
                Instantiate(bullet, muzzleFlash.transform.position, transform.rotation);
                ammo -= 1;
                MGFireTimer = Time.time + MGFireTime;
            }

            anim.SetBool("Shooting", true);
        }
        else {
            muzzleFlash.SetActive(false);
            anim.SetBool("Shooting", false);
        }

        //Right Mouse Button
        if (Input.GetMouseButtonDown(1) && fuel >= 1) {
            flameStream.GetComponent<ParticleSystem>().Play();
            anim.SetBool("Flamer", true);
        }
        else if (Input.GetMouseButtonUp(1) && fuel >= 1) {
            flameStream.GetComponent<ParticleSystem>().Stop();
            anim.SetBool("Flamer", false);
        }
        else if (fuel <= 0) {
            flameStream.GetComponent<ParticleSystem>().Stop();
            anim.SetBool("Flamer", false);
        }

        if (Input.GetMouseButton(1) && fuel >= 1) {
            if (Time.time > FTFireTimer) {
                Instantiate(fireDamage, flameStream.transform.position, transform.rotation);
                fuel -= 1;
                FTFireTimer = Time.time + FTFireTime;
            }
        }
    }

    void Movement() {

        playerPosition = transform.position;

        //Forwards and Back
        if (Input.GetKey("w")) {
            playerPosition.z = playerPosition.z + moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("s")) {
            playerPosition.z = playerPosition.z - moveSpeed * Time.deltaTime;
        }

        //Strafing 
        if (Input.GetKey("a")) {
            playerPosition.x = playerPosition.x - moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("d")) {
            playerPosition.x = playerPosition.x + moveSpeed * Time.deltaTime;
        }

        //Animation Controls - Move Vector Dot Product
        Vector3 moveVector = (playerPosition - transform.position).normalized;
        float direction = Vector3.Dot(moveVector, transform.forward);

        //Relative Forwards
        if (direction > 0.8f) {
            anim.SetBool("Walking F", true);
            anim.SetBool("Walking B", false);
            anim.SetBool("Strafe R", false);
            anim.SetBool("Strafe L", false);
        }
        //Relative Right
        else if (direction < 0.8f && direction > 0) {
            anim.SetBool("Walking F", false);
            anim.SetBool("Walking B", false);
            anim.SetBool("Strafe R", true);
            anim.SetBool("Strafe L", false);
        }
        //Relative Backwards
        else if (direction < -0.8f) {
            anim.SetBool("Walking F", false);
            anim.SetBool("Walking B", true);
            anim.SetBool("Strafe R", false);
            anim.SetBool("Strafe L", false);
        }
        //Relative Left
        else if (direction > -0.8f && direction < 0) {
            anim.SetBool("Walking F", false);
            anim.SetBool("Walking B", false);
            anim.SetBool("Strafe R", false);
            anim.SetBool("Strafe L", true);
        }
        //Turn off all anims
        else {
            anim.SetBool("Walking F", false);
            anim.SetBool("Walking B", false);
            anim.SetBool("Strafe R", false);
            anim.SetBool("Strafe L", false);
        }

        transform.position = playerPosition;
        rb.velocity = new Vector3(0,0,0);   //Freeze velocity
    }


    public void takeDamage(float damage) {

        health -= damage;

        if (health <= 0) {
            //Disable irrelvant animation bools
            anim.SetBool("Dead", true);
            anim.SetBool("Shooting", false);
            muzzleFlash.SetActive(false);
            anim.SetBool("Flamer", false);
            flameStream.GetComponent<ParticleSystem>().Stop();
            anim.SetBool("Walking F", false);
            anim.SetBool("Walking B", false);
            anim.SetBool("Strafe R", false);
            anim.SetBool("Strafe L", false);
            GameManager.instance.playerDead = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    //End of Level Goal Interaction
    public void OnTriggerEnter(Collider other) {

        if (other.tag == "Goal") {
            GameManager.instance.levelComplete = true;
            StartCoroutine(GameManager.instance.LoadLevel(GameManager.instance.nextLevel));
        }
    }
}
