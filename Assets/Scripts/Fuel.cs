using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : Pickup {

    public int fuel = 15;

    public override void OnTriggerEnter(Collider other) {

        if (other.tag == "Player") {
            other.transform.GetComponent<PlayerAvatar>().fuel += fuel;

            if (other.transform.GetComponent<PlayerAvatar>().fuel > 50)
                other.transform.GetComponent<PlayerAvatar>().fuel = 50;

            Destroy(this.gameObject);
        }
    }
}
