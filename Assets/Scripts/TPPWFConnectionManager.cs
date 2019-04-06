using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VoxelBusters;
using VoxelBusters.NativePlugins;
using SimpleJSON;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
namespace Photon.Pun.UtilityScripts
{
	public class TPPWFConnectionManager : MonoBehaviourPunCallbacks {
		public bool isMaster,isRemote,JoinedRoomFlag;
		public bool isSelectedAmount;
		private GameObject QuitPanel;
		public GameObject AmountCheckButton;
		public GameObject RoomEnterButton;
		public GameObject LoadingImage;
		public Text WarningText;
		public string GameLobbyName=null;
		public int PlayerLength=0;
		public List<GameObject> Amounts;



		void Awake()
		{
			DontDestroyOnLoad (this);
		}

		void Start()
		{
			string SelectedAmount = PlayerPrefs.GetString ("amountSelected");
			if (SelectedAmount.Length > 0) {
				foreach (GameObject go in Amounts) {
					if (!go.name.Equals (SelectedAmount)) {
						go.GetComponent<Toggle> ().interactable = false;
					}
				}
			}
		}

		public void WarningMethod()
		{
			if (Application.internetReachability == NetworkReachability.NotReachable) {
				print ("no Internet connection is there");
				StartCoroutine (RoomNameWarning ("PLEASE CONNECT TO INTERNET"));
			} else if (!isSelectedAmount) {
				StartCoroutine (RoomNameWarning ("PLEASE select the amount"));
			} else if (isSelectedAmount && !RoomEnterButton.activeInHierarchy) {
				StartCoroutine (WarningForRoom ("You don't have sufficient balance for bid",2));
			}
		}


		public void CreateOrJoinRoomMethod()
		{
			foreach(GameObject go in Amounts){
				go.GetComponent<Toggle> ().interactable = false;
			}
			RoomEnterButton.GetComponent<Button> ().interactable = false;
			 if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) {

				if (PhotonNetwork.AuthValues == null) {
					PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues ();
				}

				LoadingImage.SetActive (true);
				string PlayerName = PlayerPrefs.GetString ("username");
				print (PlayerPrefs.GetString ("username"));
				PhotonNetwork.AuthValues.UserId = PlayerName;
				PhotonNetwork.LocalPlayer.NickName = PlayerName;
				PhotonNetwork.ConnectUsingSettings ();
			}
		}





		public IEnumerator RoomNameWarning(string warn)
		{
			WarningText.text = warn;
			yield return new WaitForSeconds (1);
			WarningText.text = null;
		}

		public void InviteFriend()
		{
			ShareSheet shareSheet = new ShareSheet ();
			shareSheet.Text +="Lets Play LudoBtc Game Together. Please Join My Room "+"Room Name:"+ PlayerPrefs.GetString("roomname")+" Amount: "+ PlayerPrefs.GetString("amount");
			NPBinding.Sharing.ShowView (shareSheet, FinishSharing);
		}
		private void FinishSharing(eShareResult _result)
		{
			print (_result);
			print ("FinishSharing ()");
			StartCoroutine (RoomNameTracking());
		}

		public void AmountSelectionMethod()
		{
			print ("AmountSelectionMethod");
			GameLobbyName= EventSystem.current.currentSelectedGameObject.name;
//			foreach(GameObject go in Amounts){
//				go.GetComponent<Toggle> ().interactable = false;
//			}
			LoadingImage.SetActive (true);
			isSelectedAmount = true;

			PlayerPrefs.SetString ("amount", GameLobbyName);
			AmountCheckButton.SetActive (false);
			StartCoroutine (AmountCheckingBeforeEntering ());
		}

		IEnumerator WarningForRoom(string msg,float time)
		{
			WarningText.text = msg;
			yield return new WaitForSeconds (time);	
			WarningText.text = "";
		}


		IEnumerator RoomNameTracking()
		{
			//http://apienjoybtc.exioms.me/api/Room/roomallusers?struserid=2&strgamesessionid=1&intgametype=2&roomid=abc&dblamt=100&date=2019-01-01
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Room/roomallusers?struserid="+PlayerPrefs.GetString("userid")+"&strgamesessionid=1&intgametype=2&roomid="+PlayerPrefs.GetString("roomname")+"&dblamt="+PlayerPrefs.GetString("amount")+"&date="+System.DateTime.Now.ToString ("yyyy-MM-dd hh-mm-ss"));
			www.chunkedTransfer = false;
			www.downloadHandler = new DownloadHandlerBuffer ();
			yield return www.SendWebRequest ();
			if (www.error != null) {
				print ("Something went wrong");
			} else {
				print (www.downloadHandler.text);
				isMaster = true;
				SceneManager.LoadScene ("OneOnOneGameBoard");
//				StartCoroutine (AmountCheckingAfterEntering ());
			}
		}


		IEnumerator AmountCheckingBeforeEntering()
		{
			print ("AmountCheckingBeforeEntering");
//			GameLobbyName = EventSystem.current.currentSelectedGameObject.name;
//			http://apienjoybtc.exioms.me/api/Balance/balancefetch?userid=2&gamesessionid=1&dblbidamt=100
//			id=null;
//			id=PlayerPrefs.GetString("userid");
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
				print ("Have enough balance");
				StartCoroutine (WarningForRoom ("You can bid",.5f));
				LoadingImage.SetActive (false);
				RoomEnterButton.SetActive (true);
				AmountCheckButton.SetActive (false);
			} else if (msg.Equals ("Youdon'thavesufficientbalanceforbid")) {
				print ("You don't have sufficient balance for bid");
				GameLobbyName = null;
				GameLobbyName = "nothing";
				LoadingImage.SetActive (false);
				RoomEnterButton.SetActive (false);
				AmountCheckButton.SetActive (true);
				PlayerPrefs.SetString ("amount", null);
				StartCoroutine (WarningForRoom ("You don't have sufficient balance for bid",2));
			}
		}






		public override void OnConnectedToMaster()
		{
			print ("Conneced to master server:");
			PhotonNetwork.JoinLobby ();


		}
		public override void OnJoinedLobby()
		{

			if (PlayerPrefs.GetString ("getroomname").Length > 0) {
				StartCoroutine (GettingRoomName ());
			} else {
				PhotonNetwork.JoinOrCreateRoom (PlayerPrefs.GetString ("roomname"), new Photon.Realtime.RoomOptions {
					MaxPlayers = 2,
					PlayerTtl = 60000,
					EmptyRoomTtl = 3000
				}, null);
			}
		}



		IEnumerator GettingRoomName()
		{
			print ("GettingRoomName()");
//			LoaddingImage.SetActive (true);
			print (PlayerPrefs.GetString("amount"));
			UnityWebRequest www =new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Room/roomcreatebyuser?struserid="+ PlayerPrefs.GetString("userid")+"&strgamesessionid=1&intgametype=2&dblamount="+PlayerPrefs.GetString("amount"));
			www.chunkedTransfer = false;
			www.downloadHandler = new DownloadHandlerBuffer ();
			yield return www.SendWebRequest ();
			if (www.error != null) {
				print ("Someting went wrong");
			} else {
				print (www.downloadHandler.text);
				string msg = www.downloadHandler.text;
				msg = msg.Substring (1, msg.Length - 2);
				JSONNode jn = SimpleJSON.JSONData.Parse (msg);
				msg = jn [0];
				if (jn [0].Value.Equals ("InvalidID")) {
					PlayerPrefs.SetString ("userid",null);
					SceneManager.LoadScene ("Home");
				}
				else if (msg.Contains ("2PLDO")) {
//					SceneManager.LoadScene ("BettingAmountFor2PlayerPlayWithFriends");

					PlayerPrefs.SetString ("roomname", msg);
					PhotonNetwork.JoinOrCreateRoom (PlayerPrefs.GetString("roomname"), new Photon.Realtime.RoomOptions {
						MaxPlayers = 2,
						PlayerTtl = 60000,
						EmptyRoomTtl = 3000
					}, null);

				} else {
//					LoaddingImage.SetActive (false);
				}
			}
		}



		public override void OnCreatedRoom()
		{
			print ("Room Created Successfully");
			InviteFriend ();
		}
		public override void OnCreateRoomFailed(short msg,string msg1)
		{
			print(msg1);

//			PhotonNetwork.JoinRoom ("nsd");
		}

		public override void OnJoinRandomFailed (short returnCode, string message)
		{
			StartCoroutine (WarningForRoom ("You don't have sufficient balance for bid",2));
		}
		public override void OnJoinedRoom()
		{
			print (PhotonNetwork.MasterClient.NickName);
			print ("Room Joined successfully");
			if (!PhotonNetwork.IsMasterClient && !JoinedRoomFlag) {
				isRemote = true;
				StartCoroutine (RoomNameTracking ());
			}
			if (PhotonNetwork.PlayerList.Length == 2) 
			{
				JoinedRoomFlag = true;
			}
		}



		void OnApplicationQuit()
		{
			if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) 
			{
				print ("Quit the Application When internet connection is There and Logged out");
				StartCoroutine (HitUrl11 (0));
				PlayerPrefs.SetString ("userid", null);
			}
			else if(Application.internetReachability == NetworkReachability.NotReachable)
			{
				print ("no Internet connection is there");
			}
		}
		IEnumerator HitUrl11(int status)
		{
			print ("HitUrl11");
			string id= PlayerPrefs.GetString ("userid");
			UnityWebRequest request11 =new UnityWebRequest("http://apienjoybtc.exioms.me/api/Home/login_session?userid="+id+"&gamesessionid="+status);

			request11.chunkedTransfer = false;

			request11.downloadHandler = new DownloadHandlerBuffer ();

			yield return request11.SendWebRequest ();

			if (request11.error != null) {

			} else {
				print (request11.downloadHandler.text);
			}
		}
	}
}
