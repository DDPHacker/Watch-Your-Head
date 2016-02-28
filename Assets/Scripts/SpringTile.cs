using UnityEngine;
using System.Collections;

public class SpringTile : GameTile {

	// Use this for initialization
	void Start () {
		type = 1;
		isFall = true;
	}

	void OnTriggerEnter2D(Collider2D other){
		other.gameObject.GetComponent<PlayerController> ().SetBounce();
	}
}
