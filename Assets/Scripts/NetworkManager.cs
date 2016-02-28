using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings("v0.1");
	}

	void OnJoinedLobby() {
		Debug.Log("On joined lobby");
	}

	void JoinRoom() {
		RoomOptions roomOption = new RoomOptions() {isVisible = true, isOpen = true, maxPlayers = 4};
		PhotonNetwork.JoinOrCreateRoom("Test", roomOption, TypedLobby.Default);
		Debug.Log("Join Room");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
