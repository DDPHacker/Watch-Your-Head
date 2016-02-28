using UnityEngine;
using System.Collections;

public class TrapTile : GameTile {
	private bool flag;
	// Use this for initialization
	void Start () {
		type = 5;
		isFall = false;
		flag = true;
	}
	void OnTriggerEnter2D(Collider2D other){
		if (flag) {
			other.gameObject.GetComponent<PlayerController> ().HP--;
			flag = false;
		}
	}
}
