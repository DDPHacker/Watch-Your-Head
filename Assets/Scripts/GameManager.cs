using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	void Awake(){
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);

		initGame ();
	}

	void initGame(){
		SceneManager.LoadScene ("PickTeamScene");

	}

	void Update(){
		
	}
}
