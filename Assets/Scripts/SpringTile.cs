using UnityEngine;
using System.Collections;

public class SpringTile : GameTile {

	// Use this for initialization
	void Start () {
		isFall = false;
		type = 3;
	}
	override void stepOn(GameObject Player)
	{
		// speed up
	}
	// Update is called once per frame
	void Update () {
	
	}
}
