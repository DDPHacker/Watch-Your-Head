using UnityEngine;
using System.Collections;

public class PlayerController : Photon.MonoBehaviour {

	public int maxHP;
	public float maxSpeed;
	public float jumpSpeed;
	public float acceleration;
	[HideInInspector]public int team;
	[HideInInspector]public int HP;

	private Rigidbody2D rb2D;
	private float distToGround;
	private bool isMine;
	private Vector3 correctPosition;
	private Quaternion correctRotation;

	// Use this for initialization
	void Start() {
		HP = maxHP;
		rb2D = GetComponent<Rigidbody2D>();
		distToGround = GetComponent<Collider2D>().bounds.extents.y + 0.1f;
		isMine = photonView.isMine;
		if (!isMine) {
			GetComponent<Rigidbody2D>().isKinematic = true;
		}
	}
		
	// Update is called once per frame
	void Update() {
		if (isMine) {
			float moveHorizontal = Input.GetAxisRaw("Horizontal");

			// Move
			if (moveHorizontal != 0) {
				Move(moveHorizontal);
			}

			// Jump
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				if (isGrounded()) {
					Jump();
				}
			}

			// Fall
			if (Input.GetKeyDown(KeyCode.DownArrow)) {
				if (isGrounded()) {
					Fall();
				}
			}
		}
	}

	void FixedUpdate() {
		ClampHorizontalSpeed();
		if (!isMine) {
			UpdatePlayerPosition();
		}
	}

	void Move(float moveHorizontal) {
		rb2D.AddForce(new Vector2(moveHorizontal * acceleration, 0));	
	}

	void Jump() {
		rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
	}

	void Fall() {
	
	}

	void ClampHorizontalSpeed() {
		float speedHorizontal = rb2D.velocity.x;
		speedHorizontal = Mathf.Clamp(speedHorizontal, -maxSpeed, maxSpeed);
		rb2D.velocity = new Vector2(speedHorizontal, rb2D.velocity.y);
	}

	bool isGrounded(){
		Vector2 coordinate2D =new Vector2(transform.position.x,transform.position.y-distToGround);
		return Physics2D.Raycast(coordinate2D, -Vector2.up, 0.1f);
	}

	void UpdatePlayerPosition() {
		transform.position = Vector3.Lerp(transform.position, correctPosition, Time.deltaTime * 10);
		transform.rotation = Quaternion.Slerp(transform.rotation, correctRotation, Time.deltaTime * 10);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		} else {
			correctPosition = (Vector3)stream.ReceiveNext();
			correctRotation = (Quaternion)stream.ReceiveNext();
			Debug.Log(correctPosition);
		}
	}
}
