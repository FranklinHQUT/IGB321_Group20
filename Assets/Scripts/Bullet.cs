using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile {

    public override void OnTriggerEnter(Collider otherObject) {

        if (otherObject.tag == "Enemy") { 
            otherObject.GetComponent<Enemy>().takeDamage(damage);
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
        else if (otherObject.tag == "Environment") {
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
