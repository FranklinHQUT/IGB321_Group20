using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health: Pickup {

    public int health = 20;

    public override void OnTriggerEnter(Collider other) {
        
        if(other.tag == "Player") {
            other.transform.GetComponent<PlayerAvatar>().health += health;

            if (other.transform.GetComponent<PlayerAvatar>().health > 100)
                other.transform.GetComponent<PlayerAvatar>().health = 100;

            Destroy(this.gameObject);
        }
    }
}
