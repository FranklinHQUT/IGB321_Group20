using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //Singleton Setup
    public static GameManager instance = null;

    public bool playerDead = false;

    public bool levelComplete = false;

    string thisLevel;
    public string nextLevel;

    public bool isPaused;
    public GameObject pausedText;

    // Awake Checks - Singleton setup
    void Awake() {

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () 
    {
        thisLevel = SceneManager.GetActiveScene().name;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            pausedText.SetActive(!isPaused);
            PauseGame();
        }
	}

    public IEnumerator LoadLevel(string level) {

        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(level);
    }

    void PauseGame()
    {
        if (isPaused) { Time.timeScale = 0; }
        else { Time.timeScale = 1; }
    }

}
