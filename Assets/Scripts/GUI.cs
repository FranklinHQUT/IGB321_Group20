using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour {

    GameObject player;

    public Slider healthbar;
    public Slider manabar;
    public GameObject levelCompleteText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (!player) {
            player = GameObject.FindGameObjectWithTag("Player");
            healthbar.value = 0;
            manabar.value = 0;
        }
        else if (player) {
            healthbar.value = player.GetComponent<PlayerController>().health;
            manabar.value = player.GetComponent<PlayerController>().mana;
        }

        //Level Status Text
        if (GameManager.instance.levelComplete)
        {
            levelCompleteText.SetActive(true);
            StartCoroutine(Pause(5, levelCompleteText));
        }
            
	}

    private IEnumerator Pause(float delay, GameObject obj)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}
