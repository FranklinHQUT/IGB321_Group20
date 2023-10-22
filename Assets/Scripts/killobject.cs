using UnityEngine;
using System.Collections;

public class killobject : MonoBehaviour {
	
	public float lifeTimeDuration = 1.0f;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, lifeTimeDuration);
	}
}
