using UnityEngine;
using System.Collections;

public class TrapTile : GameTile {
	private bool flag;
	public Sprite TrapOff;
	private SpriteRenderer spriteRenderer; 
	// Use this for initialization
	void Start () {
		type = 5;
		isFall = false;
		flag = true;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	void OnTriggerEnter2D(Collider2D other){
		if (flag) {
			other.gameObject.GetComponent<PlayerController> ().Damaged();
			flag = false;
			spriteRenderer.sprite = TrapOff;
		}
	}
}
