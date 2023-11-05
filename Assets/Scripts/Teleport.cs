using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public float x;
    public float y;
    public float z;

    private void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Player") {
            other.GetComponent<PlayerController>().Teleport(x, y, z);
        }
    }
}
