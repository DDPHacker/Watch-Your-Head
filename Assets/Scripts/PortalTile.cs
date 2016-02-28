using UnityEngine;
using System.Collections;

public class PortalTile : GameTile {
	private int x,y;
	// Use this for initialization
	void Start () {
		isFall = false;
		type = 2;
	}
	void getPosition (int x1, int y1)
	{
		x = x1;
		y = y1;
	}
	override void stepOn(GameObject Player)
	{
		Player.transform.position = ();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
