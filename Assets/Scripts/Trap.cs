using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {
	private bool flag = true;
	void OnTriggerEnter2D(Collider2D other){
		GameObject play = GameObject.FindGameObjectWithTag ("Player");
		if (flag) {
			play.GetComponent<PlayerController> ().HP--;
			flag = false;
		}
	}
}
