using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	[SerializeField] public GameObject playerPrefab;
	[SerializeField] public Transform[] spawnPoints;

	private GameObject player;

	// Use this for initialization
	void Start () {
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings("v0.1");
	}

	void OnJoinedLobby() {
		Debug.Log("On joined lobby!!");
		JoinRoom();
	}

	void JoinRoom() {
		Debug.Log("Join Room!!");
		RoomOptions roomOption = new RoomOptions() {isVisible = true, isOpen = true, maxPlayers = 4};
		PhotonNetwork.JoinOrCreateRoom("Test", roomOption, TypedLobby.Default);
	}

	void OnJoinedRoom() {
		Debug.Log("On Joined Room!!!");
		StartCoroutine(SpawnPlayer());
	}

	IEnumerator SpawnPlayer() {
		yield return new WaitForSeconds(0.0f);
		//messageWindow.SetActive(true);
		int spawnIndex = Random.Range(0, spawnPoints.Length);
		player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation, 0);
	}
}
