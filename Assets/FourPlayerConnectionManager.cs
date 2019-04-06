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
	public class FourPlayerConnectionManager : MonoBehaviourPunCallbacks 
	{
		public static bool isMaster,isRemote1,isRemote2,isRemote3,JoinedRoomFlag;
		public Text WarningText;
		private GameObject QuitPanel;
		public GameObject LoadingImage;
		public string RandomRoomName = null;
		public bool isSelectedAmount;
		public List<GameObject> Amounts;
		public GameObject CreateRoomButton;

		void Start()
		{
			PlayerPrefs.SetString ("amount",null);
		}
	// Use this for initialization
		void Awake()
		{
			DontDestroyOnLoad (this);
		}
		public void AmountSelectionMethod()
		{
			print ("AmountSelectionMethod()");
			LoadingImage.SetActive (true);
			PlayerPrefs.SetString("amount",EventSystem.current.currentSelectedGameObject.name);
			LoadingImage.SetActive (true);
			CreateRoomButton.SetActive (false);
			StartCoroutine (AmountCheckingBeforeEntering ());
		}

		IEnumerator AmountCheckingBeforeEntering()
		{
			print ("AmountCheckingBeforeEntering");
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Balance/balancefetch?userid="+PlayerPrefs.GetString("userid")+"&gamesessionid=1&dblbidamt="+EventSystem.current.currentSelectedGameObject.name);
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
				print ("Have enough balance:"+PlayerPrefs.GetString("amount"));
				StartCoroutine (WarningForRoom ("You can bid",.5f));
				CreateRoomButton.SetActive (true);
				LoadingImage.SetActive (false);
			} else if (msg.Equals ("Youdon'thavesufficientbalanceforbid")) {
				print ("You don't have sufficient balance for bid");
				PlayerPrefs.SetString("amount","nothing");
				StartCoroutine (WarningForRoom ("You don't have sufficient balance for bid",2));
			}
		}

		public void CreateOrJoinRoomMethod()
		{
			print ("CreateRoomMethod()");
			if (PlayerPrefs.GetString("amount").Length == 0) {
				StartCoroutine (WarningForRoom ("Please select the amount", 2));
			} else if (PlayerPrefs.GetString("amount").Equals ("nothing")) {
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
				string PlayerName = PlayerPrefs.GetString ("userid");
				PhotonNetwork.AuthValues.UserId = PlayerName;
				PhotonNetwork.LocalPlayer.NickName = PlayerName;
				PhotonNetwork.ConnectUsingSettings ();
			} 
		}


		IEnumerator WarningForRoom(string msg,float timer)
		{
			WarningText.text = msg;
			LoadingImage.SetActive (false);
			yield return new WaitForSeconds (timer);	
			WarningText.text = "";
		}





		IEnumerator RoomCreationMethod()
		{
			print ("RoomCreationMethod");

			string userid = PlayerPrefs.GetString ("userid");
			string sessionId = "" + 1;
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Room/roomcreate?struserid="+userid+"&strgamesessionid=1&intgametype=4&dblamount="+PlayerPrefs.GetString("amount"));
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
				TypedLobby sqlLobby = new TypedLobby (PlayerPrefs.GetString("amount"), LobbyType.SqlLobby);
				print (PhotonNetwork.CurrentLobby.Name);
				PhotonNetwork.CreateRoom (RandomRoomName, new Photon.Realtime.RoomOptions { 
					MaxPlayers = 4,
					PlayerTtl = 15000, 
					EmptyRoomTtl = 3000 
				}, typedLobby:sqlLobby);
			}
		}


		IEnumerator RoomNameTracking()
		{
			//http://apienjoybtc.exioms.me/api/Room/roomallusers?struserid=2&strgamesessionid=1&intgametype=2&roomid=abc&dblamt=100&date=2019-01-01
			print("http://apienjoybtc.exioms.me/api/Room/roomallusers?struserid="+PlayerPrefs.GetString("userid")+"&strgamesessionid=1&intgametype=4&roomid="+PlayerPrefs.GetString("roomname")+"&dblamt="+PlayerPrefs.GetString ("amount")+"&date="+System.DateTime.Now.ToString ("yyyy-MM-dd hh-mm-ss"));
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Room/roomallusers?struserid="+PlayerPrefs.GetString("userid")+"&strgamesessionid=1&intgametype=4&roomid="+PlayerPrefs.GetString("roomname")+"&dblamt="+PlayerPrefs.GetString ("amount")+"&date="+System.DateTime.Now.ToString ("yyyy-MM-dd hh-mm-ss"));
			www.chunkedTransfer = false;
			www.downloadHandler = new DownloadHandlerBuffer ();
			yield return www.SendWebRequest ();
			if (www.error != null) {
				print ("Something went wrong");
			} else {
				print (www.downloadHandler.text);

				SceneManager.LoadScene ("ColorPickingFor4playerRandom");
			}
		}
			

		#region Room Creation And Join Related CallbackMethods

		public override void OnConnectedToMaster()
		{
			print ("Conneced to master server and Lobby Name:"+PlayerPrefs.GetString("amount"));
			TypedLobby sqlLobby = new TypedLobby (PlayerPrefs.GetString("amount"), LobbyType.SqlLobby);
			PhotonNetwork.JoinLobby (sqlLobby);
			print ("LobbyName:" + PhotonNetwork.CurrentLobby.Name);
		}
		public override void OnJoinedLobby()
		{
			print ("Joined lobby");                                                    
			PhotonNetwork.JoinRandomRoom ();
		}
		public override void OnCreatedRoom()
		{
			
//			SceneManager.LoadScene ("OneOnOneGameBoard");
//			StartCoroutine(RoomNameTracking());
			print ("Room Created Successfully");
			isMaster = true;
		}
		public override void OnCreateRoomFailed(short msg,string msg1)
		{
			print (msg1);
		}
		public override void OnJoinRandomFailed (short returnCode, string message)
		{
			print (PhotonNetwork.CurrentLobby.Name);
			print ("No rooms are available in this lobby, So the Room Creation Failed");
			StartCoroutine (RoomCreationMethod ());
		}
		public override void OnJoinedRoom()
		{
			PlayerPrefs.SetString ("amount", PhotonNetwork.CurrentLobby.Name);
			print ("Joined Random Successfully");
			print (PhotonNetwork.MasterClient.NickName);
			print (PhotonNetwork.CurrentRoom.Name);
			PlayerPrefs.SetString ("roomname",PhotonNetwork.CurrentRoom.Name);
			switch(PhotonNetwork.CurrentRoom.PlayerCount)
			{
			case 1:
				isMaster = true;
				if (!JoinedRoomFlag) {
					JoinedRoomFlag = true;
					StartCoroutine (RoomNameTracking ());
				}
				break;
			case 2:
				isRemote1 = true;
				if (!JoinedRoomFlag && !PhotonNetwork.IsMasterClient) {
					JoinedRoomFlag = true;
					StartCoroutine(RoomNameTracking ());
				}
				break;
			case 3:
				isRemote2 = true;
				if (!JoinedRoomFlag && !PhotonNetwork.IsMasterClient) {
					JoinedRoomFlag = true;
					StartCoroutine(RoomNameTracking ());
				}
				break;
			case 4:
				isRemote3 = true;
				if (!JoinedRoomFlag && !PhotonNetwork.IsMasterClient) {
					JoinedRoomFlag = true;
					StartCoroutine(RoomNameTracking ());
				}
				break;
			}
		}
		#endregion
	}
}
