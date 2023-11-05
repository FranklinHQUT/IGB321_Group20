using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : Pickup {

    public int mana = 20;

    public override void OnTriggerEnter(Collider other) {
        
        if(other.tag == "Player") {
            other.transform.GetComponent<PlayerController>().mana += mana;

            if (other.transform.GetComponent<PlayerController>().mana > 100)
                other.transform.GetComponent<PlayerController>().mana = 100;

            Destroy(this.gameObject);
        }
    }
}
