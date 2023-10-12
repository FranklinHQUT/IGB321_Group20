using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCamera : MonoBehaviour {

    public GameObject player;

    public float height;
    public float zDisp;

    public float cameraSpeed = 1.0f;
    private Vector3 newCamPos;

    // Use this for initialization
    void Start () {
        transform.position = new Vector3(transform.position.x, height, transform.position.z - zDisp);
	}
	
	// Update is called once per frame
	void Update () {

        //If Player Alive...
        if (player) {
            CameraMovement();
        }
	}


    //Camera Pans (Lerps) towards position above player avatar
    void CameraMovement() {

        newCamPos = player.transform.position;

        newCamPos.y = player.transform.position.y + height;
        newCamPos.z = player.transform.position.z - zDisp;

        transform.position = Vector3.Lerp(transform.position, newCamPos, cameraSpeed * Time.deltaTime);
    }
}
