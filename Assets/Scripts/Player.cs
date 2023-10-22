using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour {

    NavMeshAgent agent;
    public Camera camera;
    public Vector3 moveTo;

    public float health = 100.0f;

    public GameObject projectile;
    private float fireTimer;
    private float fireTime = 0.5f;
    public GameObject fireLocation;

	// Use this for initialization
	void Start () {

        agent = GetComponent<NavMeshAgent>();

        moveTo = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        //Basic Player Movement - Left Mouse Button on Environment
        if(Input.GetMouseButton(0))
            PlayerMovement();

        //Basic Player Weapon - Right Mouse Button
        if (Input.GetMouseButton(1))
            FireProjectile();

        agent.SetDestination(moveTo);
    }

    //Basic Movement Controls - Move to cursor position
    void PlayerMovement() {

        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            if(hit.transform.tag == "Ground") {
                moveTo = hit.point;
            }
        }
    }

    //Basic Weapon Controls - Track mouse location and spawn projectile at intersecting plane
    void FireProjectile() {

        Vector3 targetPoint;

        // Generate a plane that intersects the transform's position with an upwards normal.
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        // Generate a ray from the cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Determine the point where the cursor ray intersects the plane.
        float hitdist = 0.0f;
        // If the ray is parallel to the plane, Raycast will return false.
        if (playerPlane.Raycast(ray, out hitdist)) {
            // Get the point along the ray that hits the calculated distance.
            targetPoint = ray.GetPoint(hitdist);

            fireLocation.transform.LookAt(targetPoint);

            if (Time.time > fireTimer) {

                Instantiate(projectile, fireLocation.transform.position, fireLocation.transform.rotation);

                fireTimer = Time.time + fireTime;
            }
        }
    }

    public void takeDamage(float damage) {

        health -= damage;

        if (health <= 0) {
            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter(Collider other) {
        
        if(other.tag == "Goal") {
            GameManager.instance.levelComplete = true;
            StartCoroutine(GameManager.instance.LoadLevel(GameManager.instance.nextLevel));
        }
    }
}
