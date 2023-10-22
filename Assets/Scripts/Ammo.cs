using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Pickup {

    public int ammo = 50;

    public override void OnTriggerEnter(Collider other) {

        if (other.tag == "Player") {
            other.transform.GetComponent<PlayerAvatar>().ammo += ammo;

            if (other.transform.GetComponent<PlayerAvatar>().ammo > 500)
                other.transform.GetComponent<PlayerAvatar>().ammo = 500;

            Destroy(this.gameObject);
        }
    }
}
