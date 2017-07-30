using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	[SerializeField] public GameObject playerPrefab;
	[SerializeField] public Transform[] spawnPoints;

	private GameObject UIWindow;
	private GameObject loginWindow;
	private GameObject HPBar;
	private InputField usernameInput;
	private InputField roomNameInput;
	private InputField roomListInput;
	private Text loadingText;
	private GameObject player;

	// Use this for initialization
	void Start () {
		UIWindow = GameObject.FindGameObjectWithTag("UICanvas");
		loginWindow = UIWindow.transform.Find("Login").gameObject;
		loadingText = UIWindow.transform.Find("Loading Text").GetComponent<Text>();
		HPBar = UIWindow.transform.Find ("HPBar").gameObject;
		usernameInput = loginWindow.transform.Find("Username Input").GetComponent<InputField>();
		roomNameInput = loginWindow.transform.Find("Roomname Input").GetComponent<InputField>();
		roomListInput = loginWindow.transform.Find("Roomname List").GetComponent<InputField>();
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
		if (PhotonNetwork.player.isMasterClient) {
			int seed = Random.Range(0, 1000);
			GameObject.FindGameObjectWithTag("Map Generator").GetComponent<MapGenerate>().GenerateMap(seed);
			PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "seed", seed } });
		} else {
			int seed = (int)(PhotonNetwork.room.customProperties["seed"]);
			GameObject.FindGameObjectWithTag("Map Generator").GetComponent<MapGenerate>().GenerateMap(seed);
		}
		PhotonNetwork.player.name = usernameInput.text;
		StopCoroutine(UpdateStateText());
		loginWindow.SetActive(false);
		UIWindow.transform.Find ("Background").gameObject.SetActive (false);
		loadingText.gameObject.SetActive (false);
		HPBar.SetActive (true);
		StartCoroutine(SpawnPlayer());
	}

	IEnumerator SpawnPlayer() {
		yield return new WaitForSeconds(0.0f);
		System.Random rand = new System.Random();
		int spawnIndex = rand.Next(spawnPoints.Length);
		player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation, 0);
	}
}
