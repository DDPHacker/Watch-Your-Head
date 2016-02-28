using UnityEngine;
using System.Collections;

public class TrapTile : GameTile {

	// Use this for initialization
	void Start () {
		type = 5;
		isFall = false;
	}
	override void stepOn(GameObject Player)
	{
		if (isFall == false) {
			//damage
			isFall = true;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
