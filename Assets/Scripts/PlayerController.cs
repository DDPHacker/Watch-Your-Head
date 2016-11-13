using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : Photon.MonoBehaviour {

	public int maxHP;
	public float maxHorizontalSpeed;
	public float maxVerticalSpeed;	
	public float maxAngularSpeed;
	public float angularForce;
	public float bounceForce;
	public float jumpSpeed;
	public float acceleration;
	public float HP_dir;
	public int HP;
	public Sprite[] emoji;
	public BoxCollider2D bc2D;
	public BoxCollider2D bc2D_trigger;

	private int emojiState = 0;
	private Rigidbody2D rb2D;
	private SpriteRenderer spr2D;
	private float distToGround;
	private Vector2 normsize;
	private Vector2 fallsize;
	private Vector3 correctPosition;
	private Quaternion correctRotation;
	private float originalJumpSpeed;
	private float maxPositionY;
	private float minPositionY;

	private bool isMine;
	private bool falling = false;
	private GameObject HPBar;
	private bool invincible;
	private bool teleFlag = false;
	private Vector2 portalPos1;
	private Vector2 portalPos2;
	private int emojiIndex = 0;
	private MapGenerate mg;

	// Use this for initialization
	void Start() {
		HPBar = GameObject.FindGameObjectWithTag ("HPBar");
		HP = maxHP;
		rb2D = GetComponent<Rigidbody2D>();
		spr2D = GetComponent<SpriteRenderer>();
		mg = GameObject.FindGameObjectWithTag("Map Generator").GetComponent<MapGenerate>();
		distToGround = bc2D.bounds.extents.y * 1.5f;
		minPositionY = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y;
		maxPositionY = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).y;
		portalPos1 = mg.pos1;
		portalPos2 = mg.pos2;
		normsize = bc2D.size;
		fallsize = new Vector2 (normsize.x * 1.3f, normsize.y * 1.3f);
		originalJumpSpeed = jumpSpeed;
		isMine = photonView.isMine;
		if (!isMine) {
			GetComponent<Rigidbody2D>().isKinematic = true;
		} else {
			StartCoroutine(Invin ());
			GetMyEmoji();
			photonView.RPC("setName", PhotonTargets.All, PhotonNetwork.playerName);
			HPBar.GetComponent<HPController> ().show (maxHP);
		}
	}

	void GetMyEmoji () {
		System.Random rand = new System.Random();
		int tmp = rand.Next(emoji.Length / 3);
		emojiIndex = tmp * 3;
	}

	[PunRPC]
	public void setName (string name) {
		GetComponentInChildren<Text>().text = name;
	}

	// Update is called once per frame
	void Update() {
		if (HP > 0 && isMine) {
			float moveHorizontal = Input.GetAxisRaw("Horizontal");

			// Move
			if (moveHorizontal != 0) {
				Move(moveHorizontal);
			}

			// Jump
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				if (isStanding()) {
					Jump();
				}
			}

			// Fall
			if (Input.GetKey (KeyCode.DownArrow)) {
				if (!falling && Physics2D.IsTouchingLayers (bc2D, LayerMask.GetMask ("Ground"))) {
					falling = true;
					rb2D.WakeUp (); //wake up rigidbody to trigger OnCollisionStay
				}
			}

			// Refresh Map
			if (Input.GetKeyDown(KeyCode.R)) {
				photonView.RPC("RefreshMap", PhotonTargets.All);
				PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "seed", mg.GetLastSeed() } });
			}
		}

		spr2D.sprite = emoji[emojiIndex + emojiState];
	}

	[PunRPC]
	void RefreshMap() {
		mg.GenerateMap(mg.GetLastSeed() + 1);
	}

	void FixedUpdate() {
		ClampSpeed();
		ClampVerticalPosition();
		if (!isMine) {
			UpdatePlayerPosition();
		}
	}

	void Move(float moveHorizontal) {
		rb2D.AddForce(new Vector2(moveHorizontal * acceleration, 0));	
		rb2D.AddTorque(-moveHorizontal * angularForce);
	}

	void Jump() {
		rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
	}

	public void SetCantJump() {
		if (jumpSpeed != 0) {
			StartCoroutine(JumpFucked());
		} else {
			StopCoroutine(JumpFucked());
			StartCoroutine(JumpFucked());
		}
	}

	IEnumerator JumpFucked() {
		jumpSpeed = 0.0f;
		yield return new WaitForSeconds(2.00f);
		jumpSpeed = originalJumpSpeed;
	}

	public void SetBounce() {
		if (rb2D.velocity.y < 0) {
			rb2D.velocity = new Vector2(rb2D.velocity.x, -rb2D.velocity.y + 3.0f);
		}
	}

	void ClampSpeed() {
		float speedHorizontal = rb2D.velocity.x;
		float speedVertical = rb2D.velocity.y;
		speedHorizontal = Mathf.Clamp(speedHorizontal, -maxHorizontalSpeed, maxHorizontalSpeed);
		speedVertical = Mathf.Clamp(speedVertical, -maxVerticalSpeed, maxVerticalSpeed);
		rb2D.velocity = new Vector2(speedHorizontal, speedVertical);
		rb2D.angularVelocity = Mathf.Clamp(rb2D.angularVelocity, -maxAngularSpeed, maxAngularSpeed);
	}

	void ClampVerticalPosition() {
		float verticalPosition = transform.position.y;
		if (verticalPosition > maxPositionY) {
			verticalPosition = minPositionY;
		} else if (verticalPosition < minPositionY) {
			verticalPosition = maxPositionY;
		}
		transform.position = new Vector3(transform.position.x, verticalPosition, transform.position.z);
	}

	bool isStanding(){
		RaycastHit2D[] results = new RaycastHit2D[3];
		Vector2 coordinate2D = new Vector2(transform.position.x,transform.position.y);
		return Physics2D.RaycastNonAlloc(coordinate2D, -Vector2.up, results, distToGround) > 2;
	}

	void UpdatePlayerPosition() {
		if (Mathf.Abs(transform.position.y - correctPosition.y) > Mathf.Abs(maxPositionY - minPositionY) - 3.0) {
			transform.position = correctPosition;
		} else if (isNear(transform.position, portalPos1) && isNear(correctPosition, portalPos2) 
			|| isNear(transform.position, portalPos2) && isNear(correctPosition, portalPos1)) {
			transform.position = correctPosition;
		} else {
			transform.position = Vector3.Lerp(transform.position, correctPosition, Time.deltaTime * 10);
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, correctRotation, Time.deltaTime * 10);
	}

	bool isNear(Vector3 a, Vector2 b) {
		float error = 5.0f;
		if ((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) > error)
			return false;
		return true;
	}

	public void GoDie(){
		photonView.RPC("Die", PhotonTargets.All);
		StartCoroutine(BackToLife());
	}

	IEnumerator Invin() {
		invincible = true;
		yield return new WaitForSeconds(2.0f);
		invincible = false;
	}

	IEnumerator BackToLife() {
		yield return new WaitForSeconds(10.0f);
		transform.localScale = new Vector3 (transform.localScale.x * 0.5f, transform.localScale.y * 2f, 1);
		rb2D.velocity = new Vector2(0.0f, 5.0f);
		HP = 5;

		StartCoroutine(Invin());
		HPBar.GetComponent<HPController>().show(HP);
		photonView.RPC("NameBack", PhotonTargets.All);
	}

	[PunRPC]
	void NameBack() {
		GetComponentInChildren<Text>().enabled = true;
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (isMine && coll.gameObject.tag == "Player") {
			Vector2 colpos = coll.transform.position;
			Vector2 dir = new Vector2 (transform.position.x - colpos.x, transform.position.y -colpos.y);
			dir.Normalize ();
			rb2D.AddForce (new Vector2(dir.x * bounceForce, dir.y * bounceForce));
			if (!invincible && -dir.y > HP_dir && HP>0) {
				StartCoroutine(Invin ());
				ReceiveDamage();
			}
		}

	}

	void OnCollisionStay2D(Collision2D coll){
		if (falling && coll.gameObject.layer == LayerMask.NameToLayer("Ground")) {
			Physics2D.IgnoreCollision (coll.collider, bc2D, true);
		}
	}

	void OnTriggerStay2D(Collider2D coll){
		if (!falling && coll.gameObject.layer == LayerMask.NameToLayer("Ground")) {
			Physics2D.IgnoreCollision (coll, bc2D, false);
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (falling && !Physics2D.IsTouchingLayers (bc2D_trigger, LayerMask.GetMask ("Ground"))) {
			falling = false;
		}
	}

	public void ReceiveDamage() {
		if (isMine) {
			int tmp = Random.Range(0, 2);
			if (tmp == 0)
				emojiState = 1;
			else
				emojiState = 2;
			photonView.RPC("Damaged", PhotonTargets.All);
			HPBar.GetComponent<HPController>().show(HP);
			if (HP == 0) {
				GoDie();
			}
			StartCoroutine(emojiBack());
		}
	}

	IEnumerator emojiBack() {
		yield return new WaitForSeconds(1.0f);
		emojiState = 0;
	}

	public void Teleported(float x, float y, float direction) {
		if (isMine) {
			if (teleFlag) return;
			teleFlag = true;
			transform.position = new Vector3 (x, y, 0.0f);
			rb2D.velocity = new Vector2(direction * 3.0f, 5.0f);
			StartCoroutine(teleAgain());
		}
	}

	IEnumerator teleAgain() {
		yield return new WaitForSeconds(0.5f);
		teleFlag = false;
	}

	[PunRPC]
	public void Damaged() {
		HP--;
	}

	[PunRPC]
	public void Die() {
		transform.localScale = new Vector3 (transform.localScale.x * 2f, transform.localScale.y * 0.5f, 1);
		GetComponentInChildren<Text>().enabled = false;
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(transform.localScale);
			stream.SendNext(PhotonNetwork.playerName);
			stream.SendNext(emojiIndex + emojiState);
		} else {
			correctPosition = (Vector3)stream.ReceiveNext();
			correctRotation = (Quaternion)stream.ReceiveNext();
			transform.localScale = (Vector3)stream.ReceiveNext();
			GetComponentInChildren<Text>().text = (string)stream.ReceiveNext();
			emojiState = (int)stream.ReceiveNext();
		}
	}
}