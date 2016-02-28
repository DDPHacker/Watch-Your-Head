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
		if (other.gameObject.tag == "Player") {
			int sign = Random.Range(0, 2);
			if (sign == 0) {
				other.gameObject.GetComponent<PlayerController>().Teleported(x, y, 1.0f);
			} else {
				other.gameObject.GetComponent<PlayerController>().Teleported(x, y, -1.0f);
			}
		}
	}

	public void getPosition(GameObject other)
	{
		x = other.transform.position.x;
		y = other.transform.position.y;
	}
}
