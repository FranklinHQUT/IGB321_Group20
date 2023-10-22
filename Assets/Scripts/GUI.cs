using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour {

    GameObject player;

    public Slider healthbar;
    public Text ammoText;
    public Text fuelText;
    public GameObject levelCompleteText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (!player) {
            player = GameObject.FindGameObjectWithTag("Player");
            healthbar.value = 0;
        }
        else if (player) {
            healthbar.value = player.GetComponent<PlayerAvatar>().health;
            ammoText.text = "Ammo: " + player.GetComponent<PlayerAvatar>().ammo.ToString();
            fuelText.text = "Fuel: " + player.GetComponent<PlayerAvatar>().fuel.ToString();
        }


        //Level Status Text
        if (GameManager.instance.levelComplete)
            levelCompleteText.SetActive(true);

	}
}
