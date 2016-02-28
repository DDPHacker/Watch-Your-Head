using UnityEngine;
using System.Collections;

public class GameTile : MonoBehaviour {
	protected int type;
	public bool isFall;
	public virtual void stepOn(GameObject Player)
	{

	}
	// Use this for initialization
	void Start () {
		type = 0;
	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
}
