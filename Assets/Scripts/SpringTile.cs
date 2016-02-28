using UnityEngine;
using System.Collections;

public class SpringTile : GameTile {

	// Use this for initialization
	void Start () {
		isFall = false;
		type = 3;
	}
	void OnTriggerEnter2D(Collider2D other){
		other.gameObject.GetComponent<PlayerController> ().jumpSpeed *= 2;
	}
}
