using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour {

    public float speed = 10.0f;

    public float lifeTime = 2.5f;

    //Damage and Effects
    public GameObject burnDamage;

    private void Start() {
        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update() {
        Movement();
    }

    void Movement() {
        transform.position += Time.deltaTime * speed * transform.forward;
    }

    //When projectile hits enemy, tell enemy object to respawn and then destroy projectile
    void OnTriggerEnter(Collider otherObject) {

        //Goes through enemies
        if (otherObject.tag == "Enemy") {
            if (!otherObject.GetComponent<Enemy>().burning) {
                GameObject thisHit = Instantiate(burnDamage, otherObject.ClosestPoint(transform.position), transform.rotation) as GameObject;
                thisHit.transform.parent = otherObject.transform;
                otherObject.GetComponent<Enemy>().burning = thisHit;
                Destroy(thisHit.gameObject, lifeTime * 5);
            }
        }
        //Blocked by environment
        else if (otherObject.tag == "Environment") {
            GameObject thisHit = Instantiate(burnDamage, transform.position, transform.rotation) as GameObject;
            thisHit.transform.parent = otherObject.transform;
            Destroy(thisHit.gameObject, lifeTime * 5);
            Destroy(this.gameObject);
        }
    }
}
