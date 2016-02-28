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
				other.gameObject.transform.position = new Vector3 (x + 0.64f, y + 0.64f, 0.0f);
				other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(3.0f, 4.0f);
			} else {
				other.gameObject.transform.position = new Vector3 (x - 0.64f, y + 0.64f, 0.0f);
				other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-3.0f, 4.0f);
			}
		}
	}

	public void getPosition(GameObject other)
	{
		x = other.transform.position.x;
		y = other.transform.position.y;
	}
}
