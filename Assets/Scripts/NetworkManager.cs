using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	[SerializeField] public GameObject playerPrefab;
	[SerializeField] public Transform[] spawnPoints;

	private GameObject UIWindow;
	private GameObject loginWindow;
	private InputField usernameInput;
	private InputField roomNameInput;
	private InputField roomListInput;
	private Text loadingText;
	private GameObject player;

	// Use this for initialization
	void Start () {
		UIWindow = GameObject.FindGameObjectWithTag("UICanvas");
		loginWindow = UIWindow.transform.FindChild("Login").gameObject;
		loadingText = UIWindow.transform.FindChild("Loading Text").GetComponent<Text>();
		usernameInput = loginWindow.transform.FindChild("Username Input").GetComponent<InputField>();
		roomNameInput = loginWindow.transform.FindChild("Roomname Input").GetComponent<InputField>();
		roomListInput = loginWindow.transform.FindChild("Roomname List").GetComponent<InputField>();
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings("v0.2");
		StartCoroutine(UpdateStateText());
	}

	IEnumerator UpdateStateText() {
		while (true) {
			loadingText.text = PhotonNetwork.connectionStateDetailed.ToString();
			yield return null;
		}
	}

	void OnJoinedLobby() {
		Debug.Log("On joined lobby!!");
		loginWindow.SetActive(true);
	}
		
	void OnReceivedRoomListUpdate() {
		Debug.Log("On Received Room List!!");
		roomListInput.text = "";
		RoomInfo[] rooms = PhotonNetwork.GetRoomList();
		foreach (RoomInfo room in rooms)
			roomListInput.text += room.name + "\n";
	}

	public void JoinRoom() {
		Debug.Log("Join Room!!");
		RoomOptions roomOption = new RoomOptions() {isVisible = true, isOpen = true, maxPlayers = 4};
		PhotonNetwork.JoinOrCreateRoom(roomNameInput.text, roomOption, TypedLobby.Default);
	}

	void OnJoinedRoom() {
		Debug.Log("On Joined Room!!!");
		PhotonNetwork.player.name = usernameInput.text;
		StopCoroutine(UpdateStateText());
		UIWindow.SetActive(false);
		StartCoroutine(SpawnPlayer());
	}

	IEnumerator SpawnPlayer() {
		yield return new WaitForSeconds(0.0f);
		int spawnIndex = Random.Range(0, spawnPoints.Length);
		player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation, 0);
	}
}
