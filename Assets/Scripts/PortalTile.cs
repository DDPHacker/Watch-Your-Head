using UnityEngine;
using System.Collections;

public class PortalTile : GameTile {
	private float x,y;
	// Use this for initialization
	void Start () {
		isFall = false;
		type = 2;
	}
	public override void stepOn(GameObject Player)
	{
		//Player.transform.position = ();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
