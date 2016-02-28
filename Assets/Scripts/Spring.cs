using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		GameObject play = GameObject.FindGameObjectWithTag ("Player");
		play.GetComponent<PlayerController> ().jumpSpeed *= 2;
	}
}
