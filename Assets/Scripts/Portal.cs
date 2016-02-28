using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {
	private float x;
	private float y;
	void OnTriggerEnter2D(Collider2D other){
		GameObject play = GameObject.FindGameObjectWithTag ("Player");
		play.transform.position = new Vector3 (x, y, 0.0f);
	}
	public void getPosition(GameObject other)
	{
		x = other.transform.position.x;
		y = other.transform.position.y;
	}
}
