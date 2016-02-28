using UnityEngine;
using System.Collections;

public class NameTagPos : Photon.MonoBehaviour {

	// Update is called once per frame
	void Update () {
		Quaternion parentRotation = transform.parent.rotation;
		transform.localRotation = Quaternion.Euler(parentRotation.eulerAngles.x, parentRotation.eulerAngles.y, -parentRotation.eulerAngles.z);
		transform.position = transform.parent.position + new Vector3(0.0f, 0.7f, 0.0f) ;
	}
}
