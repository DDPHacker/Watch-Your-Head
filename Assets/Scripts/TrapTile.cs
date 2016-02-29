using UnityEngine;
using System.Collections;

public class TrapTile : GameTile {
	private bool flag;
	public Sprite TrapOff;
	private SpriteRenderer spriteRenderer; 
	public GameObject hp;


	private Sprite TrapOn;

	// Use this for initialization
	void Start () {
		type = 5;
		isFall = false;
		flag = true;
		spriteRenderer = GetComponent<SpriteRenderer>();
		TrapOn = spriteRenderer.sprite;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (flag && other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<PlayerController> ().ReceiveDamage ();
			flag = false;
			spriteRenderer.sprite = TrapOff;
			StartCoroutine(ResetTrap());
		}
	}

	IEnumerator ResetTrap() {
		yield return new WaitForSeconds(6.0f);
		flag = true;
		spriteRenderer.sprite = TrapOn;
	}
}
