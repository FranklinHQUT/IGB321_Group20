using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Projectile {

    public override void OnTriggerEnter(Collider otherObject) {

        if (otherObject.tag == "Player") {
            otherObject.GetComponent<PlayerAvatar>().takeDamage(damage);
            //Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
        else if (otherObject.tag == "Environment") {
            //Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
