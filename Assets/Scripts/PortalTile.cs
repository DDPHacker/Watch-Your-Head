using UnityEngine;
using System.Collections;

public class PortalTile : GameTile {
	private float x,y;
	// Use this for initialization
	void Start () {
		isFall = false;
		type = 2;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player")
			other.gameObject.transform.position = new Vector3 (x, y + 0.32f, 0.0f);
	}

	public void getPosition(GameObject other)
	{
		x = other.transform.position.x;
		y = other.transform.position.y;
	}
}
