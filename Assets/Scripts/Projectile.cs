using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour{

    public float speed = 10.0f;
    public float damage = 5.0f;

    public float lifeTime = 1.5f;

    //Effects
    public GameObject hitEffect;

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


    public virtual void OnTriggerEnter(Collider otherObject) {

    }
}
