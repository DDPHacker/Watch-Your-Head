using UnityEngine;
using System.Collections;

public class Ice : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other){
		GameObject play = GameObject.FindGameObjectWithTag ("Player");
		play.GetComponent<PlayerController> ().acceleration = 10.0f;
	}
}
