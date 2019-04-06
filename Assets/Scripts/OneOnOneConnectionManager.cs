using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Realtime;
using UnityEngine.Networking;
using SimpleJSON;
namespace Photon.Pun.UtilityScripts
{
	public class OneOnOneConnectionManager : MonoBehaviourPunCallbacks
	{
		public bool isMaster,isRemote,JoinedRoomFlag;
		public Text WarningText;
		public GameObject LoadingImage,CreateRoomButton;
		public string GameLobbyName=null;
		public string RandomRoomName = null;
		string id=null;
		public List<GameObject> Amounts;
		void Awake()
		{
			DontDestroyOnLoad (this);
		}
		public void AmountSelectionMethod()
		{
			print ("AmountSelectionMethod");
			GameLobbyName = EventSystem.current.currentSelectedGameObject.name;
			LoadingImage.SetActive (true);
			CreateRoomButton.SetActive (false);
			StartCoroutine (AmountCheckingBeforeEntering ());
		}

		IEnumerator AmountCheckingBeforeEntering()
		{
			print ("AmountCheckingBeforeEntering");
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Balance/balancefetch?userid="+PlayerPrefs.GetString("userid")+"&gamesessionid=1&dblbidamt="+GameLobbyName);
			www.chunkedTransfer = false;
			www.downloadHandler = new DownloadHandlerBuffer ();
			yield return www.SendWebRequest ();
			if (www.error != null) {
				print ("Something went Wrong");
			}
			string msg = www.downloadHandler.text;
			msg = msg.Substring (1, msg.Length - 2);
			JSONNode jn = SimpleJSON.JSONData.Parse (msg);
			msg = null;
			msg = jn [0];
			if (msg.Equals ("Successful")) {
				print ("GameLobbyName:"+GameLobbyName);
				print ("Have enough balance");
				StartCoroutine (WarningForRoom ("You can bid",.5f));
				CreateRoomButton.SetActive (true);
				LoadingImage.SetActive (false);
			} else if (msg.Equals ("Youdon'thavesufficientbalanceforbid")) {
				print ("You don't have sufficient balance for bid");
				CreateRoomButton.SetActive (true);
				GameLobbyName = null;
				GameLobbyName = "nothing";
				StartCoroutine (WarningForRoom ("You don't have sufficient balance for bid",2));
			}
		}

		// Use this for initialization  Successfully bid for game  You don't have sufficient balance for bid
		public void CreateRoomMethod()
		{
			print ("CreateRoomMethod");
			if (GameLobbyName.Length == 0) {
				StartCoroutine (WarningForRoom ("PLEASE SELECT THE Amount", 1));
			} else if (GameLobbyName.Equals ("nothing")) {
				StartCoroutine (WarningForRoom ("You don't have sufficient balance for bid",2));
			}
			else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) 
			{
				foreach(GameObject go in Amounts){
					go.GetComponent<Toggle> ().interactable = false;
				}
				CreateRoomButton.GetComponent<Button> ().interactable = false;
				LoadingImage.SetActive (true);
				if (PhotonNetwork.AuthValues == null) {
					PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues ();
				}
				string PlayerName = PlayerPrefs.GetString ("username");
				PhotonNetwork.AuthValues.UserId = PlayerName;
				PhotonNetwork.LocalPlayer.NickName = PlayerName;
				PhotonNetwork.ConnectUsingSettings ();
			} 
		}
		IEnumerator WarningForRoom(string msg,float time)
		{
			WarningText.text = msg;
			LoadingImage.SetActive (false);
			yield return new WaitForSeconds (time);	
			WarningText.text = "";
		}




		IEnumerator RoomCreationMethod()
		{
			print ("RoomCreationMethod");
			string userid = PlayerPrefs.GetString ("userid");
			string sessionId = "" + 1;
			int gameType = 2;
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Room/roomcreate?struserid="+userid+"&strgamesessionid="+sessionId+"&intgametype="+gameType+"&dblamount="+GameLobbyName);
			www.chunkedTransfer = false;
			www.downloadHandler = new DownloadHandlerBuffer ();
			yield return www.SendWebRequest ();
			if (www.error != null) {
				print (www.error);
			} else {
				print (www.downloadHandler.text);
				string msg = www.downloadHandler.text;
				msg = msg.Substring (1, msg.Length - 2);
				JSONNode jn = SimpleJSON.JSONData.Parse (msg);
				RandomRoomName = jn [0];

				TypedLobby sqlLobby = new TypedLobby (GameLobbyName, LobbyType.SqlLobby);
				print (PhotonNetwork.CurrentLobby.Name);
				PhotonNetwork.CreateRoom (RandomRoomName, new Photon.Realtime.RoomOptions { 
					MaxPlayers = 2,
					PlayerTtl=15000,
					EmptyRoomTtl = 3000 
				}, typedLobby:sqlLobby);
			}
		}








		public override void OnConnectedToMaster()
		{
			print ("Conneced to master server:");
			TypedLobby sqlLobby = new TypedLobby (GameLobbyName, LobbyType.SqlLobby);
			PhotonNetwork.JoinLobby (sqlLobby);
			print (PhotonNetwork.CurrentLobby.Name);
		}
		public override void OnJoinedLobby()
		{
			print ("Joined lobby");                                                    
			PhotonNetwork.JoinRandomRoom ();
		}
		public override void OnCreatedRoom()
		{
//			StartCoroutine(AmountCheckingAfterEntering ());
//			SceneManager.LoadScene ("OneOnOneGameBoard");

			print ("Room Created Successfully");
			isMaster = true;
		}
		public override void OnCreateRoomFailed(short msg,string msg1)
		{
			print(msg1);
		}
		public override void OnJoinRandomFailed (short returnCode, string message)
		{
			
			print (PhotonNetwork.CurrentLobby.Name);
			print ("No rooms are available in this lobby, So the Room Creation Failed");
			StartCoroutine (RoomCreationMethod ());
//			Scene currscene = SceneManager.GetActiveScene ();
//			string CurrSceneName = currscene.name;
		}
		public override void OnJoinedRoom()
		{
			PlayerPrefs.SetString ("amount", PhotonNetwork.CurrentLobby.Name);
			print ("Joined Random Successfully");
			print (PhotonNetwork.MasterClient.NickName);
			print (PhotonNetwork.CurrentRoom.Name);
			PlayerPrefs.SetString ("roomname",PhotonNetwork.CurrentRoom.Name);
			if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == 1 && !JoinedRoomFlag) {
				JoinedRoomFlag = true;
				StartCoroutine (RoomNameTracking ());
			}
			else if (PhotonNetwork.PlayerList.Length == 2 && !JoinedRoomFlag) 
			{
				JoinedRoomFlag = true;
				print ("Calling RoomNameTrack() API");
				if (!PhotonNetwork.IsMasterClient) {
					StartCoroutine (RoomNameTracking ());
				}
			}
		}

		IEnumerator RoomNameTracking()
		{
			//http://apienjoybtc.exioms.me/api/Room/roomallusers?struserid=2&strgamesessionid=1&intgametype=2&roomid=abc&dblamt=100&date=2019-01-01
			print("http://apienjoybtc.exioms.me/api/Room/roomallusers?struserid="+PlayerPrefs.GetString("userid")+"&strgamesessionid=1&intgametype=2&roomid="+PlayerPrefs.GetString("roomname")+"&dblamt="+PlayerPrefs.GetString ("amount")+"&date="+System.DateTime.Now.ToString ("yyyy-MM-dd hh-mm-ss"));
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Room/roomallusers?struserid="+PlayerPrefs.GetString("userid")+"&strgamesessionid=1&intgametype=2&roomid="+PlayerPrefs.GetString("roomname")+"&dblamt="+PlayerPrefs.GetString ("amount")+"&date="+System.DateTime.Now.ToString ("yyyy-MM-dd hh-mm-ss"));
			www.chunkedTransfer = false;
			www.downloadHandler = new DownloadHandlerBuffer ();
			yield return www.SendWebRequest ();
			if (www.error != null) {
				print ("Something went wrong");
			} else {
				print (www.downloadHandler.text);
				SceneManager.LoadScene ("OneOnOneGameBoard");
			}
		}
	}
}
