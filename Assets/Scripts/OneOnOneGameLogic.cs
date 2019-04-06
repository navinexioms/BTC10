using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using SimpleJSON;
using UnityEngine.EventSystems;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
namespace Photon.Pun.UtilityScripts
{
	public class OneOnOneGameLogic :  MonoBehaviourPunCallbacks,IPunTurnManagerCallbacks,IInRoomCallbacks
	{
		int TempNum=0;

		public Text TurnStatusText;

		public GameObject GameDiceBoard;

		public GameObject ChatPanelParent;

		public GameObject ParentGO;

		public GameObject SoundOn, SoundOff;

		public string soundValue;

		public GameObject AmountDisplay;
		public GameObject AmountDisplay2;

		public int PlayerCount1 = 1;

		public int ActualAmount;
		public int lossAmount;
		public int WinAmount;


		public List<Sprite> ProfilePic;
		public Image dpMaster;
		public Image dpRemote;

		public Text MasterName;
		public Text RemoteText;

		public string ThisLobbyName=null;
		public string ThisRoomName = null;
		public Text Countdown;

		public int TimePeriod;
		public int TimePeriodTriggeredTime;
		bool isTimeOut,isQuitingRoom,PlayerAutoDisconnect,DisconnectBySelf;

		public int OnlyOneBluePiceCanMove;
		public int OnlyOneGreenPiceCanMove;

		public GameObject QuitPanel;

		private PunTurnManager turnManager;

		public int OccuranceOfSix;
		public int TrialOFTossing;

		public int TriggeredTime;
		public int TriggerCounter;
		public int TriggerCounter3;
		public int timer;
		public int playerCounter;
	
		public float FirstTimer=0;
		public int SecondTimer=0;

		public int TriggeredTime2;
		public int TriggerCounter2;
		public int timer2;

		public int BluePlayerBlankTurnCounter;
		public int GreenPlayerBlankTurnCounter;

		public int ImageFillingCounter;

		public GameObject DisconnectPanel;
		public GameObject WinPanel, LosePanel;

		public Text DisconnectText;
		public GameObject ReconnectButton;


		JSONNode rootNode=new JSONClass();
		JSONNode childNode = new JSONClass ();
		JSONNode BlankTurn1 = new JSONClass ();
		JSONNode BlankTurn2=new JSONClass();
		JSONNode StartingTurn=new JSONClass();

		public Image TimerImage;

		public Image ImageOne;
		public Image ImageTwo;

		public Vector3 TimerOnePosition;
		public Vector3 TimerTwoPosition;

		public GameObject GameOver;
		public Text GameOverText;
		private int totalBlueInHouse, totalGreenInHouse;

		public GameObject BlueFrame, GreenFrame;

		public GameObject BluePlayerI_Border,BluePlayerII_Border,BluePlayerIII_Border,BluePlayerIV_Border;
		public GameObject GreenPlayerI_Border,GreenPlayerII_Border,GreenPlayerIII_Border,GreenPlayerIV_Border;

		public Sprite[] DiceSprite=new Sprite[7];

		public Vector3[] BluePlayers_Pos;
		public Vector3[] GreenPlayers_Pos;
		public List<GameObject> TurnImages;

		public Button BluePlayerI_Button, BluePlayerII_Button, BluePlayerIII_Button, BluePlayerIV_Button;
		public Button GreenPlayerI_Button,GreenPlayerII_Button,GreenPlayerIII_Button,GreenPlayerIV_Button;
	
		public string playerTurn="BLUE";

		public Transform diceRoll;

		public Button DiceRollButton;

		public Transform BlueDiceRollPosition, GreenDiceRollPosition;

		private string currentPlayer="none";
		private string currentPlayerName = "none";

		public GameObject BluePlayerI, BluePlayerII, BluePlayerIII, BluePlayerIV;
		public GameObject GreenPlayerI, GreenPlayerII, GreenPlayerIII, GreenPlayerIV;

		public List<AudioClip> AudioClips;

		public List<int> BluePlayer_Steps=new List<int>();
		public List<int> GreenPlayer_Steps=new List<int>();

		public List<Vector3> BluePiceSafeHouse;
		public List<Vector3> GreenPiceSafeHouse;

		public List<GameObject> BluePiceSafeHouseGO;
		public List<GameObject> GreenPiceSafeHouseGO;

		private int bluePlayerI_Steps,bluePlayerII_Steps,bluePlayerIII_Steps,bluePlayerIV_Steps;
		private int GreenPlayerI_Steps,GreenPlayer_StepsII,GreenPlayerIII_Steps,GreenPlayerIV_Steps;

		//----------------Selection of dice number Animation------------------
		private int selectDiceNumAnimation;

		//Players movement corresponding to blocks
		public List<GameObject> blueMovemenBlock=new List<GameObject>();
		public List<GameObject> greenMovementBlock=new List<GameObject>();

		public List<GameObject> BluePlayers=new List<GameObject>();
		public List<GameObject> GreenPlayers=new List<GameObject>();

		private System.Random randomNo;

		public bool isMyTurn,isRemotePlayerConnected;
		public bool PlayerCanPlayAgain;
		public bool ActualPlayerCanPlayAgain,NotConnectedProperly;
		private bool IsPressed=false,isBothPlayerEnteredInRoom,isMatchOver;





		//==========================In this section initial setup is done when player Enter in BoardRoom==========================// 

		void DisablingBordersOFBluePlayer()
		{
			BluePlayerI_Border.SetActive (false);
			BluePlayerII_Border.SetActive (false);
			BluePlayerIII_Border.SetActive (false);
			BluePlayerIV_Border.SetActive (false);
		}
		void DisablingButtonsOFBluePlayes()
		{
			BluePlayerI_Button.interactable = false;
			BluePlayerII_Button.interactable = false;
			BluePlayerIII_Button.interactable = false;
			BluePlayerIV_Button.interactable = false;
		}

		void DisablingBordersOFGreenPlayer ()
		{
			GreenPlayerI_Border.SetActive (false);
			GreenPlayerII_Border.SetActive (false);
			GreenPlayerIII_Border.SetActive (false);
			GreenPlayerIV_Border.SetActive (false);
		}

		void DisablingButtonsOfGreenPlayers()
		{
			GreenPlayerI_Button.interactable = false;
			GreenPlayerII_Button.interactable = false;
			GreenPlayerIII_Button.interactable = false;
			GreenPlayerIV_Button.interactable = false;	
		}

		void DisablingBluePlayersRaycast()
		{
			BluePlayerI_Button.GetComponent<Image> ().raycastTarget = false;
			BluePlayerII_Button.GetComponent<Image> ().raycastTarget = false;
			BluePlayerIII_Button.GetComponent<Image> ().raycastTarget = false;
			BluePlayerIV_Button.GetComponent<Image> ().raycastTarget = false;
		}

		void EnablingBluePlayersRaycast()
		{
			BluePlayerI_Button.GetComponent<Image> ().raycastTarget = true;
			BluePlayerII_Button.GetComponent<Image> ().raycastTarget = true;
			BluePlayerIII_Button.GetComponent<Image> ().raycastTarget = true;
			BluePlayerIV_Button.GetComponent<Image> ().raycastTarget = true;
		}

		void DisablingGreenPlayerRaycast()
		{
			GreenPlayerI_Button.GetComponent<Image> ().raycastTarget = false;
			GreenPlayerII_Button.GetComponent<Image> ().raycastTarget = false;
			GreenPlayerIII_Button.GetComponent<Image> ().raycastTarget = false;
			GreenPlayerIV_Button.GetComponent<Image> ().raycastTarget = false;
		}

		void EnablingGreenPlayerRaycast()
		{
			GreenPlayerI_Button.GetComponent<Image> ().raycastTarget = true;
			GreenPlayerII_Button.GetComponent<Image> ().raycastTarget = true;
			GreenPlayerIII_Button.GetComponent<Image> ().raycastTarget = true;
			GreenPlayerIV_Button.GetComponent<Image> ().raycastTarget = true;
		}
		//End

		//This method is executed after sending the turn By Any Player
		// Blue player can beat green player or viceversa and also who is the winner is also checked
		void InitializeDice()
		{
			print ("InitializeDice()");
			OnlyOneBluePiceCanMove=0;
			OnlyOneGreenPiceCanMove = 0;
			//	print ("Dice interactable becomes true");
			//	print ("Dice interactable becomes true");
			DiceRollButton.interactable = true;
			DiceRollButton.GetComponent<Button> ().enabled = true;

			//==============CHECKING WHO PLAYER WINS SURING===========//
			if (totalBlueInHouse > 3) 
			{
				isMatchOver = true;
//				print ("Playedr Wins");
				playerTurn = "none";
				if (PhotonNetwork.IsMasterClient) {
					if (soundValue.Equals ("on")) {
						this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [3];
						this.gameObject.GetComponent<AudioSource> ().Play ();
					}
					WinPanel.SetActive (true);

					AmountDisplay.GetComponent<Text> ().text = "" + WinAmount;

					StartCoroutine(WinnerAPI("2","2"));

				} else {
					if (soundValue.Equals ("on")) {
						this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [4];
						this.gameObject.GetComponent<AudioSource> ().Play ();
					}
					LosePanel.SetActive (true);

					AmountDisplay2.GetComponent<Text> ().text = "" + lossAmount;

//					StartCoroutine (LosserAPI ("4", "4"));
					PhotonNetwork.LeaveRoom ();
					PhotonNetwork.LeaveLobby ();
					PhotonNetwork.Disconnect ();

				}
			}
			if (totalGreenInHouse > 3) 
			{
				isMatchOver = true;
//				print ("Player 2 Wins");
				playerTurn = "none";
				if (!PhotonNetwork.IsMasterClient) {
					if (soundValue.Equals ("on")) {
						this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [3];
						this.gameObject.GetComponent<AudioSource> ().Play ();
					}
					WinPanel.SetActive (true);

					AmountDisplay.GetComponent<Text> ().text = "" + WinAmount;

					StartCoroutine(WinnerAPI("4","4"));

				} else {
					if (soundValue.Equals ("on")) {
						this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [4];
						this.gameObject.GetComponent<AudioSource> ().Play ();
					}

					LosePanel.SetActive (true);

					AmountDisplay2.GetComponent<Text> ().text = "" + lossAmount;

//					StartCoroutine (LosserAPI ("2", "2"));
					PhotonNetwork.LeaveRoom ();
					PhotonNetwork.LeaveLobby ();
					PhotonNetwork.Disconnect ();
				
				}
			}

			//=============getting currentPlayer Value===========//
			if (currentPlayerName.Contains ("BLUE PLAYER")) 
			{
				if (currentPlayerName == "BLUE PLAYER I") 
					currentPlayer = BluePlayerI_Script.BluePlayerI_ColName;
				if (currentPlayerName == "BLUE PLAYER II") 
					currentPlayer = BluePlayerII_Script.BluePlayerII_ColName;
				if (currentPlayerName == "BLUE PLAYER III") 
					currentPlayer = BluePlayerIII_Script.BluePlayerIII_ColName;
				if (currentPlayerName == "BLUE PLAYER IV") 
					currentPlayer = BluePlayerIV_Script.BluePlayerIV_ColName;
//				print ("currentPlayerName:" + currentPlayerName+"currentPlayer:" + currentPlayer);
			}
			if (currentPlayerName.Contains ("GREEN PLAYER"))
			{
				if (currentPlayerName == "GREEN PLAYER I") 
					currentPlayer = GreenPlayerI_Script.GreenPlayerI_ColName;
				if (currentPlayerName == "GREEN PLAYER II") 
					currentPlayer = GreenPlayerII_Script.GreenPlayerII_ColName;
				if (currentPlayerName == "GREEN PLAYER III") 
					currentPlayer = GreenPlayerIII_Script.GreenPlayerIII_ColName;
				if (currentPlayerName == "GREEN PLAYER IV") 
					currentPlayer = GreenPlayerIV_Script.GreenPlayerIV_ColName;
//				print ("currentPlayerName:" + currentPlayerName+"currentPlayer:" + currentPlayer);
					
			}

			//============PLayer vs Opponent=============//
			if (currentPlayer != "none") 
			{
				int i = 0;
//				print ("Blue Player vs GreenPlayer currentPlayerName :"+currentPlayerName+" currentPlayer:" + currentPlayer);
				if (currentPlayerName.Contains ("BLUE PLAYER")) 
				{
					for (i = 0; i < 4; i++) {
//							print ("Blue Player vs GreenPlayer");
						if ((i == 0 && currentPlayer == GreenPlayerI_Script.GreenPlayerI_ColName && (currentPlayer != "Star" && GreenPlayerI_Script.GreenPlayerI_ColName != "Star")) ||
							(i == 1 && currentPlayer == GreenPlayerII_Script.GreenPlayerII_ColName && (currentPlayer != "Star" && GreenPlayerII_Script.GreenPlayerII_ColName != "Star")) ||
							(i == 2 && currentPlayer == GreenPlayerIII_Script.GreenPlayerIII_ColName && (currentPlayer != "Star" && GreenPlayerIII_Script.GreenPlayerIII_ColName != "Star")) ||
							(i == 3 && currentPlayer == GreenPlayerIV_Script.GreenPlayerIV_ColName && (currentPlayer != "Star" && GreenPlayerIV_Script.GreenPlayerIV_ColName != "Star"))) {
							print ("Blue Player vs GreenPlayer");
							if (soundValue.Equals ("on")) {
								this.gameObject.GetComponent<AudioSource> ().Stop ();

								this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [1];
								this.gameObject.GetComponent<AudioSource> ().Play ();
							}
//							print (" BluePlayer  Beaten GreenPlayer"+(i+1));
							//	GreenPlayerI.transform.position = GreenPlayerI_Pos;
							//	GreenPlayerI_Script.GreenPlayerI_ColName = "none";
							GreenPlayers [i].transform.position = GreenPlayers_Pos [i];
							GreenPlayer_Steps [i] = 0;
							playerTurn = "BLUE";
							if (i == 0) {
								GreenPlayerI_Script.GreenPlayerI_ColName = "none";
								print (" BluePlayer  Beaten GreenPlayer"+(i+1));
							} else if (i == 1) {
								GreenPlayerII_Script.GreenPlayerII_ColName = "none";	
								print (" BluePlayer  Beaten GreenPlayer"+(i+1));
							} else if (i == 2) {
								GreenPlayerIII_Script.GreenPlayerIII_ColName = "none";	
								print (" BluePlayer  Beaten GreenPlayer"+(i+1));
							} else if (i == 3) {
								GreenPlayerIV_Script.GreenPlayerIV_ColName = "none";
								print (" BluePlayer  Beaten GreenPlayer"+(i+1));
							}
							if (PhotonNetwork.IsMasterClient) {

							}
							else 
							{
								PlayerCanPlayAgain = true;
							}
						}
					}
				}

				if (currentPlayerName.Contains ("GREEN PLAYER")) 
				{
						
					i = 0;
//					print ("GreenPlayer VS blue Player currentPlayerName:"+currentPlayerName+" currentPlayer:" + currentPlayer);
					for (i = 0; i < 4; i++) {
//						print ("GreenPlayer VS blue Player ");
						if ((i == 0 && currentPlayer == BluePlayerI_Script.BluePlayerI_ColName && (currentPlayer != "Star" && BluePlayerI_Script.BluePlayerI_ColName != "Star")) ||
							(i == 1 && currentPlayer == BluePlayerII_Script.BluePlayerII_ColName && (currentPlayer != "Star" && BluePlayerII_Script.BluePlayerII_ColName != "Star")) ||
							(i == 2 && currentPlayer == BluePlayerIII_Script.BluePlayerIII_ColName && (currentPlayer != "Star" && BluePlayerIII_Script.BluePlayerIII_ColName != "Star")) ||
							(i == 3 && currentPlayer == BluePlayerIV_Script.BluePlayerIV_ColName && (currentPlayer != "Star" && BluePlayerIV_Script.BluePlayerIV_ColName != "Star"))) {
//							print ("GreenPlayer VS blue Player ");
							if (soundValue.Equals ("on")) {
								this.gameObject.GetComponent<AudioSource> ().Stop ();

								this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [1];
								this.gameObject.GetComponent<AudioSource> ().Play ();
							}
							BluePlayers [i].transform.position = BluePlayers_Pos [i];
							if (i == 0) {
								print (currentPlayerName+" GreenPlayer  Beaten BluePlayer"+(i+1));
								BluePlayerI_Script.BluePlayerI_ColName = "none";
							} else if (i == 1) {
								BluePlayerII_Script.BluePlayerII_ColName = "none";
								print (currentPlayerName+" GreenPlayer  Beaten BluePlayer"+(i+1));
							} else if (i == 2) {
								BluePlayerIII_Script.BluePlayerIII_ColName = "none";
								print (currentPlayerName+" GreenPlayer  Beaten BluePlayer"+(i+1));
							} else if (i == 3) {
								BluePlayerIV_Script.BluePlayerIV_ColName = "none";
								print (currentPlayerName+" GreenPlayer  Beaten BluePlayer"+(i+1));
							}
							BluePlayer_Steps [i] = 0;
							playerTurn = "GREEN";
							if (PhotonNetwork.IsMasterClient) {
								PlayerCanPlayAgain = true;
							}
							else 
							{

							}
						}
					}
				}
			}

			if (playerTurn == "BLUE") 
			{
				if (PhotonNetwork.IsMasterClient) {
					diceRoll.position = BlueDiceRollPosition.position;
					TimerImage.transform.position = TimerOnePosition;
				} else {
					diceRoll.position = GreenDiceRollPosition.position;
					TimerImage.transform.position = TimerTwoPosition;
				}

				//if this is Blue player then dice position will be at master's side
				//else dice position will be on opponent side
				DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [6];

				EnablingBluePlayersRaycast ();
				DisablingGreenPlayerRaycast ();
				BlueFrame.SetActive (true);
				GreenFrame.SetActive (false);

				TimerImage.fillAmount = 1;
				TriggerCounter = 0;
				TriggeredTime = 0;
				if (TriggerCounter < 1) 
				{
					TriggerCounter += 1;
					TriggeredTime = (int)Time.time;
				}

			}
			if (playerTurn == "GREEN") 
			{
				//if this is green then dice position will be at master's side
				//else dice position will be on opponent side
				if (!PhotonNetwork.IsMasterClient) {
					diceRoll.position = BlueDiceRollPosition.position;
					TimerImage.transform.position = TimerOnePosition;
				} else {
					diceRoll.position = GreenDiceRollPosition.position;
					TimerImage.transform.position = TimerTwoPosition;
				}

				DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [6];
				EnablingGreenPlayerRaycast ();
				DisablingBluePlayersRaycast ();
				BlueFrame.SetActive (false);
				GreenFrame.SetActive (true);
			}
			//=================disabling buttons==============//
			DisablingBordersOFBluePlayer();
			DisablingButtonsOFBluePlayes ();
			DisablingBordersOFGreenPlayer ();
			DisablingButtonsOfGreenPlayers ();
			selectDiceNumAnimation = 0;

			TimerImage.fillAmount = 1;
			TriggerCounter = 0;
			TriggeredTime = 0;
			timer = 0;
			if (TriggerCounter < 1) 
			{
				TriggerCounter += 1;
				TriggeredTime = (int)Time.time;
			}
		}
		//End

		//These are the Winner and Loser API which will be called When the game is finished at the winner's end Winner API is Called and At loser's end Loser API is called 
		IEnumerator WinnerAPI(string playerid,string playerColor)
		{
			//http://apienjoybtc.exioms.me/api/Balance/gamewinlossbalance?userid=2&gamesessionid=1&intWalletType=1&dblamt=100&dbllossamt=300&gametype=2&roomid=2PLDO1&date=27/02/2019&playercolor=1&playerid=1
			print ("http://apienjoybtc.exioms.me/api/Balance/gamewinlossbalance?userid=" + PlayerPrefs.GetString ("userid") + "&gamesessionid=1&intWalletType=1&dblamt=" + PlayerPrefs.GetString ("amount") + "&dbllossamt=" + PlayerPrefs.GetString ("amount") + "&gametype=2&roomid=" + PhotonNetwork.CurrentRoom.Name + "&date=" + System.DateTime.Now.ToString ("yyyy-MM-dd hh:mm:ss") + "&playercolor="+playerColor+"&playerid="+playerid);
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Balance/gamewinlossbalance?userid=" + PlayerPrefs.GetString ("userid") + "&gamesessionid=1&intWalletType=1&dblamt=" + PlayerPrefs.GetString ("amount") + "&dbllossamt=" + PlayerPrefs.GetString ("amount") + "&gametype=2&roomid=" + PhotonNetwork.CurrentRoom.Name + "&date=" + System.DateTime.Now.ToString ("yyyy-MM-dd hh:mm:ss") + "&playercolor="+playerColor+"&playerid="+playerid);
			www.chunkedTransfer=false;
			www.downloadHandler=new DownloadHandlerBuffer();
			yield return www.SendWebRequest();
			if(www.error!=null){
				print("Something went Wrong");
			}else{
				print(www.downloadHandler.text);
				string msg = www.downloadHandler.text;
				msg = msg.Substring (1, msg.Length - 2);
				JSONNode jn = SimpleJSON.JSONData.Parse (msg);
				string msg2 = jn [0];
				print (msg2);


				PhotonNetwork.LeaveRoom ();
				PhotonNetwork.LeaveLobby ();
				PhotonNetwork.Disconnect ();


			}
		}

		IEnumerator LosserAPI(string playerid,string playerColor){
			//http://apienjoybtc.exioms.me/api/Balance/gameloss?userid=1007&gamesessionid=1&intWalletType=2&gametype=2&dblamt=100&roomid=2PLDO1&playercolor=4&playerid=4&date=2019-03-06
			print ("http://apienjoybtc.exioms.me/api/Balance/gameloss?userid=" + PlayerPrefs.GetString ("userid") + "&gamesessionid=1&intWalletType=2&dblamt=" + PlayerPrefs.GetString ("amount") + "&gametype=2&roomid=" + PhotonNetwork.CurrentRoom.Name + "&date=" + System.DateTime.Now.ToString ("yyyy-MM-dd hh:mm:ss") + "&playercolor=" + playerColor + "&playerid=" + playerColor);
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Balance/gameloss?userid="+PlayerPrefs.GetString("userid")+"&gamesessionid=1&intWalletType=2&dblamt="+PlayerPrefs.GetString("amount")+"&gametype=2&roomid="+PhotonNetwork.CurrentRoom.Name+"&date="+System.DateTime.Now.ToString ("yyyy-MM-dd hh:mm:ss")+"&playercolor="+playerColor+"&playerid="+playerColor);
			www.chunkedTransfer = false;
			www.downloadHandler = new DownloadHandlerBuffer ();
			yield return www.SendWebRequest ();
			if (www.error != null) {
				print ("Something went wrong " + www.error);
			} else {
				print (www.downloadHandler.text);
				string msg = www.downloadHandler.text;
				msg = msg.Substring (1, msg.Length - 2);
				JSONNode jn = SimpleJSON.JSONData.Parse (msg);
				msg = jn [0];
				if (msg.Equals ("Recordadded")) {
					print (msg);

					PhotonNetwork.LeaveRoom ();
					PhotonNetwork.LeaveLobby ();
					PhotonNetwork.Disconnect ();

				}
			}
		}
		//end

		//This method is executed after the DiceRoll()
		//In this section, The possibility of two pices of same color is checked if there is possibility then through the below logic  there will be no possibility
		void CheckToChangePossibilityOfTwoPlayerATSamePosition ()
		{
			if (playerTurn == "BLUE") {
				if (BluePlayer_Steps [0] > 0 && BluePlayer_Steps [0] < 52 &&
					((BluePlayer_Steps [0] + selectDiceNumAnimation == BluePlayer_Steps [1]) || (BluePlayer_Steps [0] + selectDiceNumAnimation == BluePlayer_Steps [2]) || (BluePlayer_Steps [0] + selectDiceNumAnimation == BluePlayer_Steps [3]))) {
					if (BluePlayer_Steps [0] + selectDiceNumAnimation != 9 && BluePlayer_Steps [0] + selectDiceNumAnimation != 14 && BluePlayer_Steps [0] + selectDiceNumAnimation != 22 && BluePlayer_Steps [0] + selectDiceNumAnimation != 27 &&
						BluePlayer_Steps [0] + selectDiceNumAnimation != 35 && BluePlayer_Steps [0] + selectDiceNumAnimation != 40 && BluePlayer_Steps [0] + selectDiceNumAnimation != 48 ) {
//						print ("Possibility was there AnimationNum" + selectDiceNumAnimation);
						selectDiceNumAnimation = 1;
						bool TempFlag = false;
						while (selectDiceNumAnimation != 6) {
							if ((BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [2] && BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [3]) &&
								BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [2] && BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [3] && 
								BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [3] &&
								BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [2]) {
								selectDiceNumAnimation = selectDiceNumAnimation;
								DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
								TempFlag = true;
								break;
							} else {
								selectDiceNumAnimation++;
							}
							if (TempFlag)
								break;
						}
					}
				} else if (BluePlayer_Steps [1] > 0 && BluePlayer_Steps [1] < 52 &&
					 ((BluePlayer_Steps [1] + selectDiceNumAnimation == BluePlayer_Steps [0]) || (BluePlayer_Steps [1] + selectDiceNumAnimation == BluePlayer_Steps [2]) || (BluePlayer_Steps [1] + selectDiceNumAnimation == BluePlayer_Steps [3]))) {
					if (BluePlayer_Steps [1] + selectDiceNumAnimation != 9 && BluePlayer_Steps [1] + selectDiceNumAnimation != 14 && BluePlayer_Steps [1] + selectDiceNumAnimation != 22 && BluePlayer_Steps [1] + selectDiceNumAnimation != 27 &&
						BluePlayer_Steps [1] + selectDiceNumAnimation != 35 && BluePlayer_Steps [1] + selectDiceNumAnimation != 40 && BluePlayer_Steps [1] + selectDiceNumAnimation != 48 ) {
//						print ("Possibility was there AnimationNum" + selectDiceNumAnimation);
						selectDiceNumAnimation = 1;
						bool TempFlag = false;
						while (selectDiceNumAnimation != 6) {
							if ((BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [2] && BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [3]) &&
								BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [2] && BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [3] && 
								BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [3] &&
								BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [2]) {
								selectDiceNumAnimation = selectDiceNumAnimation;
								DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
								TempFlag = true;
								break;
							} else {
								selectDiceNumAnimation++;
							}
							if (TempFlag)
								break;
						}
					}
				} else if (BluePlayer_Steps [2] > 0 && BluePlayer_Steps [2] < 52 &&
					((BluePlayer_Steps [2] + selectDiceNumAnimation == BluePlayer_Steps [0]) || (BluePlayer_Steps [2] + selectDiceNumAnimation == BluePlayer_Steps [1]) || (BluePlayer_Steps [2] + selectDiceNumAnimation == BluePlayer_Steps [3]))) {
					if (BluePlayer_Steps [2] + selectDiceNumAnimation != 9 && BluePlayer_Steps [2] + selectDiceNumAnimation != 14 && BluePlayer_Steps [2] + selectDiceNumAnimation != 22 && BluePlayer_Steps [2] + selectDiceNumAnimation != 27 &&
						BluePlayer_Steps [2] + selectDiceNumAnimation != 35 && BluePlayer_Steps [2] + selectDiceNumAnimation != 40 && BluePlayer_Steps [2] + selectDiceNumAnimation != 48 ) {
//						print ("Possibility was there AnimationNum" + selectDiceNumAnimation);
						selectDiceNumAnimation = 1;
						bool TempFlag = false;
						while (selectDiceNumAnimation != 6) {
							if ((BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [2] && BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [3]) &&
								BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [2] && BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [3] && 
								BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [3] &&
								BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [2]) {
								selectDiceNumAnimation = selectDiceNumAnimation;
								DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
								TempFlag = true;
								break;
							} else {
								selectDiceNumAnimation++;
							}
							if (TempFlag)
								break;
						}
					}
				} else if (BluePlayer_Steps [3] > 0 && BluePlayer_Steps [3] < 52 &&
					((BluePlayer_Steps [3] + selectDiceNumAnimation == BluePlayer_Steps [0]) || (BluePlayer_Steps [3] + selectDiceNumAnimation == BluePlayer_Steps [1]) || (BluePlayer_Steps [3] + selectDiceNumAnimation == BluePlayer_Steps [2]))) {
					if (BluePlayer_Steps [3] + selectDiceNumAnimation != 9 && BluePlayer_Steps [3] + selectDiceNumAnimation != 14 && BluePlayer_Steps [3] + selectDiceNumAnimation != 22 && BluePlayer_Steps [3] + selectDiceNumAnimation != 27 &&
						BluePlayer_Steps [3] + selectDiceNumAnimation != 35 && BluePlayer_Steps [3] + selectDiceNumAnimation != 40 && BluePlayer_Steps [3] + selectDiceNumAnimation != 48 ) {
//						print ("Possibility was there AnimationNum" + selectDiceNumAnimation);
						selectDiceNumAnimation = 1;
						bool TempFlag = false;
						while (selectDiceNumAnimation != 6) {
							if ((BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [2] && BluePlayer_Steps [0] + selectDiceNumAnimation != BluePlayer_Steps [3]) &&
								BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [2] && BluePlayer_Steps [1] + selectDiceNumAnimation != BluePlayer_Steps [3] && 
								BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [2] + selectDiceNumAnimation != BluePlayer_Steps [3] &&
								BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [0] && BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [1] && BluePlayer_Steps [3] + selectDiceNumAnimation != BluePlayer_Steps [2]) {
								selectDiceNumAnimation = selectDiceNumAnimation;
								DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
								TempFlag = true;
								break;
							} else {
								selectDiceNumAnimation++;
							}
							if (TempFlag)
								break;
						}
					}
				}
			}
			if (playerTurn == "GREEN") {
				if (GreenPlayer_Steps [0] > 0 && GreenPlayer_Steps [0] < 52 &&
					((GreenPlayer_Steps [0] + selectDiceNumAnimation == GreenPlayer_Steps [1]) || (GreenPlayer_Steps [0] + selectDiceNumAnimation == GreenPlayer_Steps [2]) || (GreenPlayer_Steps [0] + selectDiceNumAnimation == GreenPlayer_Steps [3]))) {
					if (GreenPlayer_Steps [0] + selectDiceNumAnimation != 9 && GreenPlayer_Steps [0] + selectDiceNumAnimation != 14 && GreenPlayer_Steps [0] + selectDiceNumAnimation != 22 && GreenPlayer_Steps [0] + selectDiceNumAnimation != 27 &&
						GreenPlayer_Steps [0] + selectDiceNumAnimation != 35 && GreenPlayer_Steps [0] + selectDiceNumAnimation != 40 && GreenPlayer_Steps [0] + selectDiceNumAnimation != 48 ) {
//						print ("Possibility was there AnimationNum" + selectDiceNumAnimation);
						selectDiceNumAnimation = 1;
						bool TempFlag = false;
						while (selectDiceNumAnimation != 6) {
							if (GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [2] && GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [3] &&
								GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [2] && GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [3] &&
								GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [3] && 
								GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [2]) {
								selectDiceNumAnimation = selectDiceNumAnimation;
								DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
								TempFlag = true;
								break;
							} else {
								selectDiceNumAnimation++;
							}
							if (TempFlag)
								break;
						}
					}
				} else if (GreenPlayer_Steps [1] > 0 && GreenPlayer_Steps [1] < 52 &&
					((GreenPlayer_Steps [1] + selectDiceNumAnimation == GreenPlayer_Steps [0]) || (GreenPlayer_Steps [1] + selectDiceNumAnimation == GreenPlayer_Steps [2]) || (GreenPlayer_Steps [1] + selectDiceNumAnimation == GreenPlayer_Steps [3]))) {
					if (GreenPlayer_Steps [1] + selectDiceNumAnimation != 9 && GreenPlayer_Steps [1] + selectDiceNumAnimation != 14 && GreenPlayer_Steps [1] + selectDiceNumAnimation != 22 && GreenPlayer_Steps [1] + selectDiceNumAnimation != 27 &&
						GreenPlayer_Steps [1] + selectDiceNumAnimation != 35 && GreenPlayer_Steps [1] + selectDiceNumAnimation != 40 && GreenPlayer_Steps [1] + selectDiceNumAnimation != 48 ) {
//						print ("Possibility was there AnimationNum" + selectDiceNumAnimation);
						selectDiceNumAnimation = 1;
						bool TempFlag = false;
						while (selectDiceNumAnimation != 6) {
							if (GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [2] && GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [3] &&
								GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [2] && GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [3] &&
								GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [3] && 
								GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [2]){
								selectDiceNumAnimation = selectDiceNumAnimation;
								DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
								TempFlag = true;
								break;
							} else {
								selectDiceNumAnimation++;
							}
							if (TempFlag)
								break;
						}
					}
				} else if (GreenPlayer_Steps [2] > 0 && GreenPlayer_Steps [2] < 52 &&
					((GreenPlayer_Steps [2] + selectDiceNumAnimation == GreenPlayer_Steps [0]) || (GreenPlayer_Steps [2] + selectDiceNumAnimation == GreenPlayer_Steps [1]) || (GreenPlayer_Steps [2] + selectDiceNumAnimation == GreenPlayer_Steps [3]))) {
					if (GreenPlayer_Steps [2] + selectDiceNumAnimation != 9 && GreenPlayer_Steps [2] + selectDiceNumAnimation != 14 && GreenPlayer_Steps [2] + selectDiceNumAnimation != 22 && GreenPlayer_Steps [2] + selectDiceNumAnimation != 27 &&
						GreenPlayer_Steps [2] + selectDiceNumAnimation != 35 && GreenPlayer_Steps [2] + selectDiceNumAnimation != 40 && GreenPlayer_Steps [2] + selectDiceNumAnimation != 48 ) {
//						print ("Possibility was there AnimationNum" + selectDiceNumAnimation);
						selectDiceNumAnimation = 1;
						bool TempFlag = false;
						while (selectDiceNumAnimation != 6) {
							if (GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [2] && GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [3] &&
								GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [2] && GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [3] &&
								GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [3] && 
								GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [2]) {
								selectDiceNumAnimation = selectDiceNumAnimation;
								DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
								TempFlag = true;
								break;
							} else {
								selectDiceNumAnimation++;
							}
							if (TempFlag)
								break;
						}
					}
				} else if (GreenPlayer_Steps [3] > 0 && GreenPlayer_Steps [3] < 52 &&
					((GreenPlayer_Steps [3] + selectDiceNumAnimation == GreenPlayer_Steps [0]) || (GreenPlayer_Steps [3] + selectDiceNumAnimation == GreenPlayer_Steps [1]) || (GreenPlayer_Steps [3] + selectDiceNumAnimation == GreenPlayer_Steps [2]))) {
					if (GreenPlayer_Steps [3] + selectDiceNumAnimation != 9 && GreenPlayer_Steps [3] + selectDiceNumAnimation != 14 && GreenPlayer_Steps [3] + selectDiceNumAnimation != 22 && GreenPlayer_Steps [3] + selectDiceNumAnimation != 27 &&
						GreenPlayer_Steps [3] + selectDiceNumAnimation != 35 && GreenPlayer_Steps [3] + selectDiceNumAnimation != 40 && GreenPlayer_Steps [3] + selectDiceNumAnimation != 48 ) {
//						print ("Possibility was there AnimationNum" + selectDiceNumAnimation);
						selectDiceNumAnimation = 1;
						bool TempFlag = false;
						while (selectDiceNumAnimation != 6) {
							if (GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [2] && GreenPlayer_Steps [0] + selectDiceNumAnimation != GreenPlayer_Steps [3] &&
								GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [2] && GreenPlayer_Steps [1] + selectDiceNumAnimation != GreenPlayer_Steps [3] &&
								GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [2] + selectDiceNumAnimation != GreenPlayer_Steps [3] && 
								GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [0] && GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [1] && GreenPlayer_Steps [3] + selectDiceNumAnimation != GreenPlayer_Steps [2]) {
								selectDiceNumAnimation = selectDiceNumAnimation;
								DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
								TempFlag = true;
								break;
							} else {
								selectDiceNumAnimation++;
							}
							if (TempFlag)
								break;
						}
					}
				}
			}
		}
		//end



		//===========================this method generate the random number======================//
		public void DiceRoll()
		{
			if ((playerTurn.Equals ("BLUE") && PhotonNetwork.IsMasterClient) || (playerTurn.Equals ("GREEN") && !PhotonNetwork.IsMasterClient)) {
				if (!turnManager.IsFinishedByMe) {
					if (PhotonNetwork.IsMasterClient) {
						BluePlayerBlankTurnCounter = 0;

						TurnImages [0].GetComponent<Image> ().enabled = true;
						TurnImages [1].GetComponent<Image> ().enabled = true;
						TurnImages [2].GetComponent<Image> ().enabled = true;

					} else {
						GreenPlayerBlankTurnCounter = 0;
						TurnImages [5].GetComponent<Image> ().enabled = true;
						TurnImages [4].GetComponent<Image> ().enabled = true;
						TurnImages [3].GetComponent<Image> ().enabled = true;
					}
//					print ("Hello");
					DiceRollButton.interactable = false;
					DiceRollButton.GetComponent<Button> ().enabled = false;
					selectDiceNumAnimation = Random.Range (0, 6);
					selectDiceNumAnimation += 1;

					CheckToChangePossibilityOfTwoPlayerATSamePosition ();

					if ((selectDiceNumAnimation == 6) && TrialOFTossing < 4) {

						OccuranceOfSix += 1;
						if (OccuranceOfSix == 1) {
							print ("One Time got 1 or 6");
						}
						if (OccuranceOfSix == 2) {
							print ("Two Times got 1 or 6");
						}
						if (OccuranceOfSix == 3) {
							print ("Occurance of six or One three times");
							selectDiceNumAnimation = Random.Range (1, 6);
							print ("Changed Number And Got" + selectDiceNumAnimation);
							DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [selectDiceNumAnimation - 1];
							TrialOFTossing = 0;
							OccuranceOfSix = 0;
						}
					} else {
						print ("TrialOFTossing" + TrialOFTossing + " Reset TrialOFTossing");
						TrialOFTossing = 0;
						OccuranceOfSix = 0;
					}

					print ("selectDiceNumAnimation:" + selectDiceNumAnimation);
					rootNode.Add ("DiceNumber", selectDiceNumAnimation.ToString (temp));
					temp = rootNode.ToString ();

					StartCoroutine (PlayersNotInitializedForTimeDelay (temp));
				}
			}
		}
		//End










		//This method is executed in DiceRoll() after CheckToChangePossibilityOfTwoPlayerATSamePosition () the to provide some delay to Send Blank turn by the opponent if This player can play Again
		IEnumerator PlayersNotInitializedForTimeDelay(string temp)
		{
			yield return new WaitForSeconds (0);
			switch (playerTurn) 
			{
			case "BLUE":
				//=============condition for border glow=============

				if ((blueMovemenBlock.Count - BluePlayer_Steps [0]) >= selectDiceNumAnimation && BluePlayer_Steps [0] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [0])) {
					BluePlayerI_Border.SetActive (true);
					BluePlayerI_Button.interactable = true;
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
//					print ("Border glowing");
				} else {
					BluePlayerI_Border.SetActive (false);
					BluePlayerI_Button.interactable = false;

				}
				if ((blueMovemenBlock.Count - BluePlayer_Steps [1]) >= selectDiceNumAnimation && BluePlayer_Steps [1] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [1])) {
					BluePlayerII_Border.SetActive (true);
					BluePlayerII_Button.interactable = true;
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
//					print ("Border glowing");
				} else {
					BluePlayerII_Border.SetActive (false);
					BluePlayerII_Button.interactable = false;


				}
				if ((blueMovemenBlock.Count - BluePlayer_Steps [2]) >= selectDiceNumAnimation && BluePlayer_Steps [2] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [2])) {
					BluePlayerIII_Border.SetActive (true);
					BluePlayerIII_Button.interactable = true;
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
//					print ("Border glowing");
				} else {
					BluePlayerIII_Border.SetActive (false);
					BluePlayerIII_Button.interactable = false;

				}
				if ((blueMovemenBlock.Count - BluePlayer_Steps [3]) >= selectDiceNumAnimation && BluePlayer_Steps [3] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [3])) {
					BluePlayerIV_Border.SetActive (true);
					BluePlayerIV_Button.interactable = true;
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
//					print ("Border glowing");
//					print ("not Skipping");
				} else {
					BluePlayerIV_Border.SetActive (false);
					BluePlayerIV_Button.interactable = false;

				}
				//===============Players border glow When Opening===============//
				if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && (BluePlayer_Steps [0] == 0 || BluePlayer_Steps [1] == 0 || BluePlayer_Steps [2] == 0 || BluePlayer_Steps [3] == 0)) {
//					print ("Players border glow When Opening");
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [0] == 0) {
						BluePlayerI_Border.SetActive (true);
						BluePlayerI_Button.interactable = true;
					}

					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [1] == 0) {
						BluePlayerII_Border.SetActive (true);
						BluePlayerII_Button.interactable = true;
					}

					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [2] == 0) {
						BluePlayerIII_Border.SetActive (true);
						BluePlayerIII_Button.interactable = true;
					}

					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [3] == 0) {
						BluePlayerIV_Border.SetActive (true);
						BluePlayerIV_Button.interactable = true;
					}
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
//					print ("Added:" + temp);
				}	
				//=========================PLAYERS DON'T HAVE ANY MOVES , SWITCH TO NEXT PLAYER'S TURN=========================//

				if (!BluePlayerI_Border.activeInHierarchy && !BluePlayerII_Border.activeInHierarchy &&
					!BluePlayerIII_Border.activeInHierarchy && !BluePlayerIV_Border.activeInHierarchy) {
					rootNode.Add ("WaitingTime", "DontSkip");
					temp = rootNode.ToString ();
//					print ("not Skipping");
				}

				BluePlayerI_Border.SetActive (false);
				BluePlayerI_Button.interactable = false;

				BluePlayerII_Border.SetActive (false);
				BluePlayerII_Button.interactable = false;

				BluePlayerIII_Border.SetActive (false);
				BluePlayerIII_Button.interactable = false;

				BluePlayerIV_Border.SetActive (false);
				BluePlayerIV_Button.interactable = false;

				break;
			case "GREEN":
				//=============condition for border glow=============

				if ((greenMovementBlock.Count - GreenPlayer_Steps [0]) >= selectDiceNumAnimation && GreenPlayer_Steps [0] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [0])) {
					GreenPlayerI_Border.SetActive (true);
					GreenPlayerI_Button.interactable = true;
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
//					print ("Border glowing");
				} else {
					GreenPlayerI_Border.SetActive (false);
					GreenPlayerI_Button.interactable = false;
				}
				if ((greenMovementBlock.Count - GreenPlayer_Steps [1]) >= selectDiceNumAnimation && GreenPlayer_Steps [1] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [1])) {
					GreenPlayerII_Border.SetActive (true);
					GreenPlayerII_Button.interactable = true;
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
//					print ("Border glowing");
				} else {
					GreenPlayerII_Border.SetActive (false);
					GreenPlayerII_Button.interactable = false;
				}
				if ((greenMovementBlock.Count - GreenPlayer_Steps [2]) >= selectDiceNumAnimation && GreenPlayer_Steps [2] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [2])) {
					GreenPlayerIII_Border.SetActive (true);
					GreenPlayerIII_Button.interactable = true;
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
//					print ("Border glowing");
				} else {
					GreenPlayerIII_Border.SetActive (false);
					GreenPlayerIII_Button.interactable = false;
				}
				if ((greenMovementBlock.Count - GreenPlayer_Steps [3]) >= selectDiceNumAnimation && GreenPlayer_Steps [3] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [3])) {
					GreenPlayerIV_Border.SetActive (true);
					GreenPlayerIV_Button.interactable = true;
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();
//					print ("Border glowing");
				} else {
					GreenPlayerIV_Border.SetActive (false);
					GreenPlayerIV_Button.interactable = false;
				}

				//===============Players border glow When Opening===============//

				if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && (GreenPlayer_Steps [0] == 0 || GreenPlayer_Steps [1] == 0 || GreenPlayer_Steps [2] == 0 || GreenPlayer_Steps [3] == 0)) {
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [0] == 0) {
						GreenPlayerI_Border.SetActive (true);
						GreenPlayerI_Button.interactable = true;
					}
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [1] == 0) {
						GreenPlayerII_Border.SetActive (true);
						GreenPlayerII_Button.interactable = true;
					}
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [2] == 0) {
						GreenPlayerIII_Border.SetActive (true);
						GreenPlayerIII_Button.interactable = true;
					}
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [3] == 0) {
						GreenPlayerIV_Border.SetActive (true);
						GreenPlayerIV_Button.interactable = true;
					}
					rootNode.Add ("WaitingTime", "Skip");
					temp = rootNode.ToString ();

				}
				//=========================PLAYERS DON'T HAVE ANY MOVES , SWITCH TO NEXT PLAYER'S TURN=========================//

				if (!GreenPlayerI_Border.activeInHierarchy && !GreenPlayerII_Border.activeInHierarchy &&
					!GreenPlayerIII_Border.activeInHierarchy && !GreenPlayerIV_Border.activeInHierarchy && selectDiceNumAnimation!=6) 
				{
					rootNode.Add ("WaitingTime", "DontSkip");
					temp = rootNode.ToString ();
//					print ("not Skipping");
				}

				GreenPlayerI_Border.SetActive (false);
				GreenPlayerI_Button.interactable = false;

				GreenPlayerII_Border.SetActive (false);
				GreenPlayerII_Button.interactable = false;

				GreenPlayerIII_Border.SetActive (false);
				GreenPlayerIII_Button.interactable = false;

				GreenPlayerIV_Border.SetActive (false);
				GreenPlayerIV_Button.interactable = false;

				break;
			}
			this.MakeTurn (temp);
		}
		//End












		//This method is executed After the  OnPlayerFinished() call back method
		//This method shows the dice toss animation and shows the randomly ganerated number by DiceRoll() method
		IEnumerator DiceToss(int DiceNumber)
		{
			if (soundValue.Equals ("on")) {
				this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [5];
				this.gameObject.GetComponent<AudioSource> ().Play ();
			}
			int randomDice = 0;
			for (int i = 0; i < 8;i++) 
			{
				randomDice = Random.Range (0, 6);
				DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [randomDice];
				yield return new WaitForSeconds(.12f);
			}
			DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [DiceNumber-1];
			selectDiceNumAnimation = DiceNumber;
			//	print ("selectDiceNumAnimation:" + selectDiceNumAnimation);
			StartCoroutine (PlayersNotInitialized ());
		}
		//end


		//This method is ec=xecuted after the DiceToss() coroutine
		//In this method ,player's which pice can move is shown by highlighting the border of the pice
		//if no one border is highlighted then the turn will be passed to the other player
		IEnumerator PlayersNotInitialized()
		{
			//	print ("Executing PlayersNotInitialized()");
			yield return new WaitForSeconds (.8f);
			//game start initial position of each player(green and blue)
			switch (playerTurn) 
			{
			case "BLUE":
				//=============condition for border glow=============

				if ((blueMovemenBlock.Count - BluePlayer_Steps [0]) >= selectDiceNumAnimation && BluePlayer_Steps [0] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [0])) {
					BluePlayerI_Border.SetActive (true);
					BluePlayerI_Button.interactable = true;
					OnlyOneBluePiceCanMove += 1;
					print (" ");
					//	PlayerCanPlayAgain = true;
				} else {
					BluePlayerI_Border.SetActive (false);
					BluePlayerI_Button.interactable = false;
				}
				if ((blueMovemenBlock.Count - BluePlayer_Steps [1]) >= selectDiceNumAnimation && BluePlayer_Steps [1] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [1])) {
					BluePlayerII_Border.SetActive (true);
					BluePlayerII_Button.interactable = true;
					OnlyOneBluePiceCanMove += 1;
					print (" ");
					//	PlayerCanPlayAgain = true;
				} else {
					BluePlayerII_Border.SetActive (false);
					BluePlayerII_Button.interactable = false;
				}
				if ((blueMovemenBlock.Count - BluePlayer_Steps [2]) >= selectDiceNumAnimation && BluePlayer_Steps [2] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [2])) {
					BluePlayerIII_Border.SetActive (true);
					BluePlayerIII_Button.interactable = true;
					OnlyOneBluePiceCanMove += 1;
					print (" ");
					//	PlayerCanPlayAgain = true;
				} else {
					BluePlayerIII_Border.SetActive (false);
					BluePlayerIII_Button.interactable = false;
				}
				if ((blueMovemenBlock.Count - BluePlayer_Steps [3]) >= selectDiceNumAnimation && BluePlayer_Steps [3] > 0 && (blueMovemenBlock.Count > BluePlayer_Steps [3])) {
					BluePlayerIV_Border.SetActive (true);
					BluePlayerIV_Button.interactable = true;
					OnlyOneBluePiceCanMove += 1;
					print (" ");
					//	PlayerCanPlayAgain = true;
				} else {
					BluePlayerIV_Border.SetActive (false);
					BluePlayerIV_Button.interactable = false;
				}
				//===============Players border glow When Opening===============//
				if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && (BluePlayer_Steps [0] == 0 || BluePlayer_Steps [1] == 0 || BluePlayer_Steps [2] == 0 || BluePlayer_Steps [3] == 0)) {
					print ("Players border glow When Opening");
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [0] == 0) {
						print (" ");
						BluePlayerI_Border.SetActive (true);
						BluePlayerI_Button.interactable = true;
						OnlyOneBluePiceCanMove += 1;
					}

					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [1] == 0) {
						print (" ");
						BluePlayerII_Border.SetActive (true);
						BluePlayerII_Button.interactable = true;
						OnlyOneBluePiceCanMove += 1;
					}

					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [2] == 0) {
						print (" ");
						BluePlayerIII_Border.SetActive (true);
						BluePlayerIII_Button.interactable = true;
						OnlyOneBluePiceCanMove += 1;
					}

					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && BluePlayer_Steps [3] == 0) {
						print (" ");
						BluePlayerIV_Border.SetActive (true);
						BluePlayerIV_Button.interactable = true;
						OnlyOneBluePiceCanMove += 1;
					}
					//	PlayerCanPlayAgain = true;
				}	

				print ("BluePlayerI:"+(BluePlayerI_Button.IsInteractable())+" BluePlayerII"+(BluePlayerII_Button.IsInteractable())+" BluePlayerIII:"+(BluePlayerIII_Button.IsInteractable())+"BluePlayerIV:"+(BluePlayerIV_Button.IsInteractable()));

				//=========================PLAYERS DON'T HAVE ANY MOVES , SWITCH TO NEXT PLAYER'S TURN=========================//

				if (!BluePlayerI_Border.activeInHierarchy && !BluePlayerII_Border.activeInHierarchy &&
				    !BluePlayerIII_Border.activeInHierarchy && !BluePlayerIV_Border.activeInHierarchy) {
					DisablingButtonsOFBluePlayes ();
						print ("PLAYERS DON'T HAVE OPTION TO MOVE , SWITCH TO NEXT PLAYER TURN");

					playerTurn = "GREEN";
					InitializeDice ();
				}
				//==========================When only one blue Pice outside of Home========================//
				if ((BluePlayerI_Border.activeInHierarchy || BluePlayerII_Border.activeInHierarchy || BluePlayerIII_Border.activeInHierarchy || BluePlayerIV_Border.activeInHierarchy) && OnlyOneBluePiceCanMove == 1) {
					StartCoroutine (BluePiceAutoMove ());
				}
				break;
			case "GREEN":
				//=============condition for border glow=============

				if ((greenMovementBlock.Count - GreenPlayer_Steps [0]) >= selectDiceNumAnimation && GreenPlayer_Steps [0] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [0])) {
					GreenPlayerI_Border.SetActive (true);
					GreenPlayerI_Button.interactable = true;
					OnlyOneGreenPiceCanMove += 1;
					//	PlayerCanPlayAgain = true;

					print ("Glowing 1st green Player");
				} else {
					GreenPlayerI_Border.SetActive (false);
					GreenPlayerI_Button.interactable = false;
				}
				if ((greenMovementBlock.Count - GreenPlayer_Steps [1]) >= selectDiceNumAnimation && GreenPlayer_Steps [1] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [1])) {
					GreenPlayerII_Border.SetActive (true);
					GreenPlayerII_Button.interactable = true;
					OnlyOneGreenPiceCanMove += 1;
					print ("Glowing 2nd green Player");
					//	PlayerCanPlayAgain = true;
				} else {
					GreenPlayerII_Border.SetActive (false);
					GreenPlayerII_Button.interactable = false;
				}
				if ((greenMovementBlock.Count - GreenPlayer_Steps [2]) >= selectDiceNumAnimation && GreenPlayer_Steps [2] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [2])) {
					GreenPlayerIII_Border.SetActive (true);
					GreenPlayerIII_Button.interactable = true;
					OnlyOneGreenPiceCanMove += 1;
					//	PlayerCanPlayAgain = true;
					print ("Glowing 3rd green Player");
				} else {
					GreenPlayerIII_Border.SetActive (false);
					GreenPlayerIII_Button.interactable = false;
				}
				if ((greenMovementBlock.Count - GreenPlayer_Steps [3]) >= selectDiceNumAnimation && GreenPlayer_Steps [3] > 0 && (greenMovementBlock.Count > GreenPlayer_Steps [3])) {
					GreenPlayerIV_Border.SetActive (true);
					GreenPlayerIV_Button.interactable = true;
					OnlyOneGreenPiceCanMove += 1;
					//	PlayerCanPlayAgain = true;
					print ("Glowing 4th green Player");
				} else {
					GreenPlayerIV_Border.SetActive (false);
					GreenPlayerIV_Button.interactable = false;
				}

				//===============Players border glow When Opening===============//

				if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && (GreenPlayer_Steps [0] == 0 || GreenPlayer_Steps [1] == 0 || GreenPlayer_Steps [2] == 0 || GreenPlayer_Steps [3] == 0)) {
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [0] == 0) {
						print ("Glowing 1st green Player");
						GreenPlayerI_Border.SetActive (true);
						GreenPlayerI_Button.interactable = true;
						OnlyOneGreenPiceCanMove += 1;
					}
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [1] == 0) {
						print ("Glowing 2nd green Player");
						GreenPlayerII_Border.SetActive (true);
						GreenPlayerII_Button.interactable = true;
					}
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [2] == 0) {
						print ("Glowing 3rd green Player");
						GreenPlayerIII_Border.SetActive (true);
						GreenPlayerIII_Button.interactable = true;
						OnlyOneGreenPiceCanMove += 1;
					}
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation == 1) && GreenPlayer_Steps [3] == 0) {
						print ("Glowing 4th green Player");
						GreenPlayerIV_Border.SetActive (true);
						GreenPlayerIV_Button.interactable = true;
						OnlyOneGreenPiceCanMove += 1;
					}
					//	PlayerCanPlayAgain = true;
				}
				print ("GreenPlayerI:"+(GreenPlayerI_Button.IsInteractable())+" GreenPlayerII"+(GreenPlayerII_Button.IsInteractable())+" GreenPlayerIII:"+(GreenPlayerIII_Button.IsInteractable())+"GreenPlayerIV:"+(GreenPlayerIV_Button.IsInteractable()));

				//=========================PLAYERS DON'T HAVE ANY MOVES , SWITCH TO NEXT PLAYER'S TURN=========================//

				if (!GreenPlayerI_Border.activeInHierarchy && !GreenPlayerII_Border.activeInHierarchy &&
				    !GreenPlayerIII_Border.activeInHierarchy && !GreenPlayerIV_Border.activeInHierarchy) {
					DisablingButtonsOfGreenPlayers ();
					print ("GREEN PLAYER DON'T HAVE OPTION TO MOVE , SWITCH TO NEXT PLAYER TURN");
					playerTurn = "BLUE";
					InitializeDice ();
				}
				//========================When only one green pice outside of home============================//
				if ((GreenPlayerI_Border.activeInHierarchy || GreenPlayerII_Border.activeInHierarchy || GreenPlayerIII_Border.activeInHierarchy || GreenPlayerIV_Border.activeInHierarchy) && OnlyOneGreenPiceCanMove == 1) {
					StartCoroutine (GreenPiceAutoMove ());
				}
				break;
			}
		}

		IEnumerator BluePiceAutoMove()
		{
			yield return new WaitForSeconds (1);
			if (BluePlayerI_Border.activeInHierarchy && OnlyOneBluePiceCanMove == 1) {
				BluePlayerI_UI_Method ();
			} else if (BluePlayerII_Border.activeInHierarchy && OnlyOneBluePiceCanMove == 1) {
				BluePlayerII_UI_Method();
			} else if (BluePlayerIII_Border.activeInHierarchy && OnlyOneBluePiceCanMove == 1) {
				BluePlayerIII_UI_Method();
			} else if (BluePlayerIV_Border.activeInHierarchy && OnlyOneBluePiceCanMove == 1) {
				BluePlayerIV_UI_Method();
			}
		}

		IEnumerator GreenPiceAutoMove()
		{
			yield return new WaitForSeconds(1);
			if (GreenPlayerI_Border.activeInHierarchy && OnlyOneGreenPiceCanMove == 1) {
				GreenPlayerI_UI_Method ();
			} else if (GreenPlayerII_Border.activeInHierarchy && OnlyOneGreenPiceCanMove == 1) {
				GreenPlayerII_UI_Method ();
			} else if (GreenPlayerIII_Border.activeInHierarchy && OnlyOneGreenPiceCanMove == 1) {
				GreenPlayerIII_UI_Method ();
			} else if (GreenPlayerIV_Border.activeInHierarchy && OnlyOneGreenPiceCanMove == 1) {
				GreenPlayerIV_UI_Method ();
			}
		}

		//end

		//This is BluePlayer 1st Pice method by Executing this 1st pice of BluePlayer can move 
		//if  this pice is already came out from home and got 6 by tossing the turn then the turn remains at this player side
		//if three pice of blue player is already came out from home and this 1 pice is in house and got 1 or 6 by tossing the dice then also the turn remains at this player side
		void BluePlayerI_UI()
		{
			if (soundValue.Equals ("on")) {
				this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [0];
				this.gameObject.GetComponent<AudioSource> ().Play ();
			}
//			print ("BluePlayerI_UI()");
			//disabling BluePlayer border and Button script
			if (playerTurn == "BLUE" && (blueMovemenBlock.Count - BluePlayer_Steps [0]) > selectDiceNumAnimation) 
			{
				if (BluePlayer_Steps [0] > 0) 
				{
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						bluePlayer_Path [i] = blueMovemenBlock [BluePlayer_Steps [0] + i].transform.position;
					}

					BluePlayer_Steps [0] += selectDiceNumAnimation;
					//============Keeping the Turn to blue Players side============//
					if (selectDiceNumAnimation == 6) 
					{
						playerTurn = "BLUE";	
						PlayerCanPlayAgain = true;
					} 
					else 
					{
						playerTurn = "GREEN";
					}
					currentPlayerName = "BLUE PLAYER I";
					MovingBlueOrGreenPlayer (BluePlayerI, bluePlayer_Path);
				} 
				else 
				{
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && BluePlayer_Steps [0] == 0)
					{
						if (soundValue.Equals ("on")) {
							this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [6];
							this.gameObject.GetComponent<AudioSource> ().Play ();
						}
//						print ("BluePlayerI Player moving its first move");
						Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];
						bluePlayer_Path [0] = blueMovemenBlock [BluePlayer_Steps [0]].transform.position;
						BluePlayer_Steps [0] += 1;
						playerTurn = "BLUE";
						currentPlayerName = "BLUE PLAYER I";
						iTween.MoveTo (BluePlayerI, iTween.Hash ("position", bluePlayer_Path [0], "speed",600, "time", 0.4f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
						PlayerCanPlayAgain = true;
					}
				}
			}
			else 
			{
				//condition when player coin is reached successfully in house
				if (playerTurn == "BLUE" && (blueMovemenBlock.Count - BluePlayer_Steps [0]) == selectDiceNumAnimation)
				{
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						bluePlayer_Path [i] = blueMovemenBlock [BluePlayer_Steps [0] + i].transform.position;
					}

					BluePlayer_Steps [0] += selectDiceNumAnimation;

					playerTurn = "BLUE";

					if (soundValue.Equals ("on")) {
						this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [2];
						this.gameObject.GetComponent<AudioSource> ().Play ();
					}

					MovingBlueOrGreenPlayerInSafeHouse (BluePlayerI, BluePiceSafeHouse[0] );
//					Vector3.MoveTowards(BluePlayerI.transform.position, BluePiceSafeHouse[0],1);
					totalBlueInHouse += 1;
//					print ("Cool");
					BluePlayerI_Button.enabled = false;

					if (PhotonNetwork.IsMasterClient) {

					}
					else 
					{
						PlayerCanPlayAgain = true;
					}
				}
				else 
				{
//					print ("You need" + (blueMovemenBlock.Count - BluePlayer_Steps [0]).ToString () + "To enter in safe house");
					if (BluePlayer_Steps [1] + BluePlayer_Steps [2] + BluePlayer_Steps [3] == 0 && selectDiceNumAnimation != 6) 
					{
						playerTurn = "GREEN";
						InitializeDice();
					}
				}
			}
			if (BluePlayer_Steps [0] == 9 || BluePlayer_Steps [0] == 22 || BluePlayer_Steps [0] == 35 || BluePlayer_Steps [0] == 48 || BluePlayer_Steps [0] == 14 || BluePlayer_Steps [0] == 40 ) {
				if (soundValue.Equals ("on")) {
					this.GetComponent<AudioSource> ().clip = AudioClips [7];
					this.GetComponent<AudioSource> ().Play ();
				}
			}
		}

		//This is BluePlayer 2n Pice method by Executing this 2nd pice of BluePlayer can move 
		//if  this pice is already came out from home and got 6 by tossing the turn then the turn remains at this player side
		//if three pice of blue player is already came out from home and this 1 pice is in house and got 1 or 6 by tossing the dice then also the turn remains at this player side
		void BluePlayerII_UI()
		{
			if (soundValue.Equals ("on")) {
				this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [0];
				this.gameObject.GetComponent<AudioSource> ().Play ();
			}
//			print ("BluePlayerII_UI()");
			if (playerTurn == "BLUE" && (blueMovemenBlock.Count - BluePlayer_Steps [1]) > selectDiceNumAnimation)
			{
				if (BluePlayer_Steps [1] > 0) 
				{
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						bluePlayer_Path [i] = blueMovemenBlock [BluePlayer_Steps [1] + i].transform.position;
					}

					BluePlayer_Steps [1] += selectDiceNumAnimation;

					//============Keeping the Turn to blue Players side============//
					if (selectDiceNumAnimation == 6) 
					{
						playerTurn = "BLUE";
						PlayerCanPlayAgain = true;
					} 
					else 
					{
						playerTurn = "GREEN";
					}

					currentPlayerName = "BLUE PLAYER II";
					MovingBlueOrGreenPlayer (BluePlayerII, bluePlayer_Path);
				} 
				else 
				{
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && BluePlayer_Steps [1] == 0)
					{

						if (soundValue.Equals ("on")) {
							this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [6];
							this.gameObject.GetComponent<AudioSource> ().Play ();
						}

						Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];
						bluePlayer_Path [0] = blueMovemenBlock [BluePlayer_Steps [1]].transform.position;
						BluePlayer_Steps [1] += 1;
						playerTurn = "BLUE";
						currentPlayerName = "BLUE PLAYER II";
						iTween.MoveTo (BluePlayerII, iTween.Hash ("position", bluePlayer_Path [0], "speed",600, "time", 0.4f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
						PlayerCanPlayAgain = true;
					}
				}
			}
			else 
			{
				//condition when player coin is reached successfully in house
				if (playerTurn == "BLUE" && (blueMovemenBlock.Count - BluePlayer_Steps [1]) == selectDiceNumAnimation) 
				{
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						bluePlayer_Path [i] = blueMovemenBlock [BluePlayer_Steps [1] + i].transform.position;
					}

					BluePlayer_Steps [1] += selectDiceNumAnimation;

					playerTurn = "BLUE";

					if (soundValue.Equals ("on")) {
						this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [2];
						this.gameObject.GetComponent<AudioSource> ().Play ();
					}

					MovingBlueOrGreenPlayerInSafeHouse (BluePlayerII, BluePiceSafeHouse[1]);
//					Vector3.MoveTowards(BluePlayerII.transform.position, BluePiceSafeHouse[1],1);
					totalBlueInHouse += 1;
//					print ("Cool");
					BluePlayerII_Button.enabled = false;
					if (PhotonNetwork.IsMasterClient) {

					}
					else 
					{
						PlayerCanPlayAgain = true;
					}
				}
				else 
				{
//					print ("You need" + (blueMovemenBlock.Count - BluePlayer_Steps [1]).ToString () + "To enter in safe house");
					if (BluePlayer_Steps [0] + BluePlayer_Steps [2] + BluePlayer_Steps [3] == 0 && selectDiceNumAnimation != 6) 
					{
						playerTurn = "GREEN";
						InitializeDice();
					}
				}
			}
			if (BluePlayer_Steps [1] == 9 || BluePlayer_Steps [1] == 22 || BluePlayer_Steps [1] == 35 || BluePlayer_Steps [1] == 48 || BluePlayer_Steps [1] == 14 || BluePlayer_Steps [1] == 40 ) {
				if (soundValue.Equals ("on")) {
					this.GetComponent<AudioSource> ().clip = AudioClips [7];
					this.GetComponent<AudioSource> ().Play ();
				}
			}
		}

		//This is BluePlayer 3rd Pice method by Executing this 3rd pice of BluePlayer can move 
		//if  this pice is already came out from home and got 6 by tossing the turn then the turn remains at this player side
		//if three pice of blue player is already came out from home and this 1 pice is in house and got 1 or 6 by tossing the dice then also the turn remains at this player side
		void BluePlayerIII_UI()
		{
//			print ("BluePlayerIII_UI()");
			if (playerTurn == "BLUE" && (blueMovemenBlock.Count - BluePlayer_Steps [2]) > selectDiceNumAnimation) 
			{

				if (soundValue.Equals ("on")) {
					this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [0];
					this.gameObject.GetComponent<AudioSource> ().Play ();
				}

				if (BluePlayer_Steps [2] > 0) 
				{
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						bluePlayer_Path [i] = blueMovemenBlock [BluePlayer_Steps [2] + i].transform.position;
					}

					BluePlayer_Steps [2] += selectDiceNumAnimation;

					//============Keeping the Turn to blue Players side============//
					if (selectDiceNumAnimation == 6) 
					{
						playerTurn = "BLUE";
						PlayerCanPlayAgain = true;
					} 
					else 
					{
						playerTurn = "GREEN";
					}

					currentPlayerName = "BLUE PLAYER III";
					MovingBlueOrGreenPlayer (BluePlayerIII, bluePlayer_Path);
				} 
				else 
				{
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && BluePlayer_Steps [2] == 0)
					{

						if (soundValue.Equals ("on")) {
							this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [6];
							this.gameObject.GetComponent<AudioSource> ().Play ();
						}
						Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];
						bluePlayer_Path [0] = blueMovemenBlock [BluePlayer_Steps [2]].transform.position;
						BluePlayer_Steps [2] += 1;
						playerTurn = "BLUE";
						currentPlayerName = "BLUE PLAYER III";
						iTween.MoveTo (BluePlayerIII, iTween.Hash ("position", bluePlayer_Path [0], "speed",600, "time", 0.4f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
						PlayerCanPlayAgain = true;

					}
				}
			}
			else 
			{
				//condition when player coin is reached successfully in house
				if (playerTurn == "BLUE" && (blueMovemenBlock.Count - BluePlayer_Steps [2]) == selectDiceNumAnimation) 
				{
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						bluePlayer_Path [i] = blueMovemenBlock [BluePlayer_Steps [2] + i].transform.position;
					}

					BluePlayer_Steps [2] += selectDiceNumAnimation;

					playerTurn = "BLUE";

					if (soundValue.Equals ("on")) {
						this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [2];
						this.gameObject.GetComponent<AudioSource> ().Play ();
					}

					MovingBlueOrGreenPlayerInSafeHouse (BluePlayerIII, BluePiceSafeHouse[2] );
//					Vector3.MoveTowards(BluePlayerIII.transform.position, BluePiceSafeHouse[2],1);
					totalBlueInHouse += 1;
//					print ("Cool");
					BluePlayerIII_Button.enabled = false;
					if (PhotonNetwork.IsMasterClient) {

					}
					else 
					{
						PlayerCanPlayAgain = true;
					}
				}
				else 
				{
//					print ("You need" + (blueMovemenBlock.Count - BluePlayer_Steps [2]).ToString () + "To enter in safe house");
					if (BluePlayer_Steps [0] + BluePlayer_Steps [1] + BluePlayer_Steps [3] == 0 && selectDiceNumAnimation != 6) 
					{
						playerTurn = "GREEN";
						InitializeDice();
					}
				}
			}
			if (BluePlayer_Steps [2] == 9 || BluePlayer_Steps [2] == 22 || BluePlayer_Steps [2] == 35 || BluePlayer_Steps [2] == 48 || BluePlayer_Steps [2] == 14 || BluePlayer_Steps [2] == 40 ) {
				if (soundValue.Equals ("on")) {
					this.GetComponent<AudioSource> ().clip = AudioClips [7];
					this.GetComponent<AudioSource> ().Play ();
				}
			}
		}

		//This is BluePlayer 4th Pice method by Executing this 4th pice of BluePlayer can move 
		//if  this pice is already came out from home and got 6 by tossing the turn then the turn remains at this player side
		//if three pice of blue player is already came out from home and this 1 pice is in house and got 1 or 6 by tossing the dice then also the turn remains at this player side
		void BluePlayerIV_UI()
		{
			if (soundValue.Equals ("on")) {
				this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [0];
				this.gameObject.GetComponent<AudioSource> ().Play ();
			}

//			print ("BluePlayerIV_UI()");
			if (playerTurn == "BLUE" && (blueMovemenBlock.Count - BluePlayer_Steps [3]) > selectDiceNumAnimation) 
			{
				if (BluePlayer_Steps [3] > 0) 
				{
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						bluePlayer_Path [i] = blueMovemenBlock [BluePlayer_Steps [3] + i].transform.position;
					}

					BluePlayer_Steps [3] += selectDiceNumAnimation;

					//============Keeping the Turn to blue Players side============//
					if (selectDiceNumAnimation == 6) 
					{
						playerTurn = "BLUE";
						PlayerCanPlayAgain = true;
					} 
					else 
					{
						playerTurn = "GREEN";
					}

					currentPlayerName = "BLUE PLAYER IV";

					MovingBlueOrGreenPlayer (BluePlayerIV, bluePlayer_Path);
				} 
				else 
				{
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && BluePlayer_Steps [3] == 0)
					{
						
						if (soundValue.Equals ("on")) {
							this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [6];
							this.gameObject.GetComponent<AudioSource> ().Play ();
						}
						Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];
						bluePlayer_Path [0] = blueMovemenBlock [BluePlayer_Steps [3]].transform.position;
						BluePlayer_Steps [3] += 1;
						playerTurn = "BLUE";
						currentPlayerName = "BLUE PLAYER IV";
						iTween.MoveTo (BluePlayerIV, iTween.Hash ("position", bluePlayer_Path [0], "speed",600, "time", 0.4f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
						PlayerCanPlayAgain = true;
					}
				}
			}
			else 
			{
				//condition when player coin is reached successfully in house
				if (playerTurn == "BLUE" && (blueMovemenBlock.Count - BluePlayer_Steps [3]) == selectDiceNumAnimation) 
				{
					Vector3[] bluePlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						bluePlayer_Path [i] = blueMovemenBlock [BluePlayer_Steps [3] + i].transform.position;
					}

					BluePlayer_Steps [3] += selectDiceNumAnimation;

					playerTurn = "BLUE";

					if (soundValue.Equals ("on")) {
						this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [2];
						this.gameObject.GetComponent<AudioSource> ().Play ();
					}

					MovingBlueOrGreenPlayerInSafeHouse (BluePlayerIV, BluePiceSafeHouse[3] );
//					Vector3.MoveTowards (BluePlayerIV.transform.position, BluePiceSafeHouse [3], 1);
					totalBlueInHouse += 1;
//					print ("Cool");
					BluePlayerIV_Button.enabled = false;
					if (PhotonNetwork.IsMasterClient) {

					}
					else 
					{
						PlayerCanPlayAgain = true;
					}
				}
				else 
				{
//					print ("You need" + (blueMovemenBlock.Count - BluePlayer_Steps [3]).ToString () + "To enter in safe house");
					if (BluePlayer_Steps [0] + BluePlayer_Steps [1] + BluePlayer_Steps [2] == 0 && selectDiceNumAnimation != 6) 
					{
						playerTurn = "GREEN";
						InitializeDice();
					}
				}
			}
			if (BluePlayer_Steps [3] == 9 || BluePlayer_Steps [3] == 22 || BluePlayer_Steps [3] == 35 || BluePlayer_Steps [3] == 48 || BluePlayer_Steps [3] == 14 || BluePlayer_Steps [3] == 40 ) {
				if (soundValue.Equals ("on")) {
					this.GetComponent<AudioSource> ().clip = AudioClips [7];
					this.GetComponent<AudioSource> ().Play ();
				}
			}
		}

		//==================Green player Movement==================//

		//This is Green player 1st Pice method by Executing this 1st pice of Green player can move 
		//if  this pice is already came out from home and got 6 by tossing the turn then the turn remains at this player side
		//if three pice of Green player is already came out from home and this 1 pice is in house and got 1 or 6 by tossing the dice then also the turn remains at this player side
		void GreenPlayerI_UI()
		{

			if (soundValue.Equals ("on")) {
				this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [0];
				this.gameObject.GetComponent<AudioSource> ().Play ();
			}

//			print ("Executing GreenPlayerI_UI()");
			if (playerTurn == "GREEN" && (greenMovementBlock.Count - GreenPlayer_Steps[0]) > selectDiceNumAnimation) 
			{
				if (GreenPlayer_Steps[0] > 0) 
				{
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++) 
					{
						greenPlayer_Path [i] = greenMovementBlock [GreenPlayer_Steps[0] + i].transform.position;
					}

					GreenPlayer_Steps[0] += selectDiceNumAnimation;

					//============Keeping the Turn to blue Players side============//
					if (selectDiceNumAnimation == 6) 
					{
						playerTurn = "GREEN";	
						PlayerCanPlayAgain = true;
					}
					else 
					{
						playerTurn = "BLUE";
					}

					currentPlayerName = "GREEN PLAYER I";

					MovingBlueOrGreenPlayer (GreenPlayerI, greenPlayer_Path);
				} 
				else 
				{
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && GreenPlayer_Steps[0] == 0) 
					{


						if (soundValue.Equals ("on")) {
							this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [6];
							this.gameObject.GetComponent<AudioSource> ().Play ();
						}
//						print ("Activating 1st green player 1st time");
						Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];
						greenPlayer_Path [0] = greenMovementBlock [GreenPlayer_Steps[0]].transform.position;
						GreenPlayer_Steps[0] += 1;
						playerTurn = "GREEN";
						currentPlayerName = "GREEN PLAYER I";
						iTween.MoveTo (GreenPlayerI, iTween.Hash ("position", greenPlayer_Path [0], "speed",600, "time", 0.4f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
						PlayerCanPlayAgain = true;
					}
				}
			}
			else 
			{
				//condition when player coin is reached successfully in house

				if (playerTurn == "GREEN" && (greenMovementBlock.Count - GreenPlayer_Steps[0]) == selectDiceNumAnimation) 
				{
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						greenPlayer_Path [i] = greenMovementBlock [GreenPlayer_Steps[0] + i].transform.position;
					}

					GreenPlayer_Steps[0] += selectDiceNumAnimation;

					playerTurn = "GREEN";

					if (soundValue.Equals ("on")) {
						this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [2];
						this.gameObject.GetComponent<AudioSource> ().Play ();
					}

					MovingBlueOrGreenPlayerInSafeHouse (GreenPlayerI, GreenPiceSafeHouse[0] );
//					Vector3.MoveTowards(GreenPlayerI.transform.position, GreenPiceSafeHouse[0],1);
					totalGreenInHouse += 1;
//					print ("Cool");
					GreenPlayerI_Button.enabled = false;
					if (PhotonNetwork.IsMasterClient) {
						PlayerCanPlayAgain = true;
					}
					else 
					{

					}
				}
				else 
				{
//					print ("You need" + (greenMovementBlock.Count - GreenPlayer_Steps[0]).ToString () + "To enter in safe house");
					if (GreenPlayer_Steps[1] + GreenPlayer_Steps[2] + GreenPlayer_Steps[3] == 0 && selectDiceNumAnimation != 6) 
					{
						playerTurn = "BLUE";
						InitializeDice();
					}
				}
			}
			if (GreenPlayer_Steps [0] == 9 || GreenPlayer_Steps [0] == 22 || GreenPlayer_Steps [0] == 35 || GreenPlayer_Steps [0] == 48 || GreenPlayer_Steps [0] == 14 || GreenPlayer_Steps [0] == 40) {
				if (soundValue.Equals ("on")) {
					this.GetComponent<AudioSource> ().clip = AudioClips [7];
					this.GetComponent<AudioSource> ().Play ();
				}
			}
		}

		//This is Green player 2nd Pice method by Executing this 2nd pice of Green player can move 
		//if  this pice is already came out from home and got 6 by tossing the turn then the turn remains at this player side
		//if three pice of Green player is already came out from home and this 1 pice is in house and got 1 or 6 by tossing the dice then also the turn remains at this player side
		void GreenPlayerII_UI()
		{
//			print ("Executing GreenPlayerII_UI()");
			if (playerTurn == "GREEN" && (greenMovementBlock.Count - GreenPlayer_Steps[1]) > selectDiceNumAnimation) 
			{
				if (soundValue.Equals ("on")) {
					this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [0];
					this.gameObject.GetComponent<AudioSource> ().Play ();
				}

				if (GreenPlayer_Steps[1] > 0) 
				{
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++) 
					{
						greenPlayer_Path [i] = greenMovementBlock [GreenPlayer_Steps[1] + i].transform.position;
					}

					GreenPlayer_Steps[1] += selectDiceNumAnimation;

					//============Keeping the Turn to blue Players side============//
					if (selectDiceNumAnimation == 6) 
					{
						playerTurn = "GREEN";	
						PlayerCanPlayAgain = true;
					}
					else 
					{
						playerTurn = "BLUE";
					}

					currentPlayerName = "GREEN PLAYER II";

					MovingBlueOrGreenPlayer (GreenPlayerII, greenPlayer_Path);
				} 
				else 
				{
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && GreenPlayer_Steps[1] == 0) 
					{
						if (soundValue.Equals ("on")) {
							this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [6];
							this.gameObject.GetComponent<AudioSource> ().Play ();
						}

//						print ("Activating 1st green player 1st time");
						Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];
						greenPlayer_Path [0] = greenMovementBlock [GreenPlayer_Steps[1]].transform.position;
						GreenPlayer_Steps[1] += 1;
						playerTurn = "GREEN";
						currentPlayerName = "GREEN PLAYER II";
						iTween.MoveTo (GreenPlayerII, iTween.Hash ("position", greenPlayer_Path [0], "speed",600, "time", 0.4f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
						PlayerCanPlayAgain = true;
					}
				}
			}
			else 
			{
				//condition when player coin is reached successfully in house

				if (playerTurn == "GREEN" && (greenMovementBlock.Count - GreenPlayer_Steps[1]) == selectDiceNumAnimation) 
				{
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						greenPlayer_Path [i] = greenMovementBlock [GreenPlayer_Steps[1] + i].transform.position;
					}

					GreenPlayer_Steps[1] += selectDiceNumAnimation;

					playerTurn = "GREEN";

					if (soundValue.Equals ("on")) {
						this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [2];
						this.gameObject.GetComponent<AudioSource> ().Play ();
					}

					MovingBlueOrGreenPlayerInSafeHouse (GreenPlayerII, GreenPiceSafeHouse[1] );
//					Vector3.MoveTowards(GreenPlayerII.transform.position, GreenPiceSafeHouse[1],1);
					totalGreenInHouse += 1;
//					print ("Cool");
					GreenPlayerII_Button.enabled = false;
					if (PhotonNetwork.IsMasterClient) {
						PlayerCanPlayAgain = true;
					}
					else 
					{

					}
				}
				else 
				{
//					print ("You need" + (greenMovementBlock.Count - GreenPlayer_Steps[1]).ToString () + "To enter in safe house");
					if (GreenPlayer_Steps[0] + GreenPlayer_Steps[2] + GreenPlayer_Steps[3] == 0 && selectDiceNumAnimation != 6) 
					{
						playerTurn = "BLUE";
						InitializeDice();
					}
				}
			}
			if (GreenPlayer_Steps [1] == 9 || GreenPlayer_Steps [1] == 22 || GreenPlayer_Steps [1] == 35 || GreenPlayer_Steps [1] == 48 || GreenPlayer_Steps [1] == 14 || GreenPlayer_Steps [1] == 40) {
				if (soundValue.Equals ("on")) {
					this.GetComponent<AudioSource> ().clip = AudioClips [7];
					this.GetComponent<AudioSource> ().Play ();
				}
			}
		}

		//This is Green player 3rd Pice method by Executing this 3rd pice of Green player can move 
		//if  this pice is already came out from home and got 6 by tossing the turn then the turn remains at this player side
		//if three pice of Green player is already came out from home and this 1 pice is in house and got 1 or 6 by tossing the dice then also the turn remains at this player side
		void GreenPlayerIII_UI()
		{
			if (soundValue.Equals ("on")) {
				this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [0];
				this.gameObject.GetComponent<AudioSource> ().Play ();
			}

//			print ("Executing GreenPlayerIII_UI()");
			if (playerTurn == "GREEN" && (greenMovementBlock.Count - GreenPlayer_Steps[2]) > selectDiceNumAnimation) 
			{
				if (GreenPlayer_Steps[2] > 0) 
				{
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++) 
					{
						greenPlayer_Path [i] = greenMovementBlock [GreenPlayer_Steps[2] + i].transform.position;
					}

					GreenPlayer_Steps[2] += selectDiceNumAnimation;

					//============Keeping the Turn to blue Players side============//
					if (selectDiceNumAnimation == 6) 
					{
						playerTurn = "GREEN";	
//						print ("Keeping the Turn to blue Players side");
						PlayerCanPlayAgain = true;
					}
					else 
					{
						playerTurn = "BLUE";
					}

					currentPlayerName = "GREEN PLAYER III";

					MovingBlueOrGreenPlayer (GreenPlayerIII, greenPlayer_Path);
				} 
				else 
				{
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && GreenPlayer_Steps[2] == 0) 
					{

						if (soundValue.Equals ("on")) {
							this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [6];
							this.gameObject.GetComponent<AudioSource> ().Play ();
						}

//						print ("Activating 3rd green player 1st time");
						Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];
						greenPlayer_Path [0] = greenMovementBlock [GreenPlayer_Steps[2]].transform.position;
						GreenPlayer_Steps[2] += 1;
						playerTurn = "GREEN";
						currentPlayerName = "GREEN PLAYER III";
						iTween.MoveTo (GreenPlayerIII, iTween.Hash ("position", greenPlayer_Path [0], "speed",600, "time", 0.4f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
						PlayerCanPlayAgain = true;
					}
				}
			}
			else 
			{
				//condition when player coin is reached successfully in house

				if (playerTurn == "GREEN" && (greenMovementBlock.Count - GreenPlayer_Steps[2]) == selectDiceNumAnimation) 
				{
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						greenPlayer_Path [i] = greenMovementBlock [GreenPlayer_Steps[2] + i].transform.position;
					}

					GreenPlayer_Steps[2] += selectDiceNumAnimation;

					playerTurn = "GREEN";

					if (soundValue.Equals ("on")) {
						this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [2];
						this.gameObject.GetComponent<AudioSource> ().Play ();
					}

					MovingBlueOrGreenPlayerInSafeHouse (GreenPlayerIII, GreenPiceSafeHouse[2] );
//					Vector3.MoveTowards(GreenPlayerIII.transform.position, GreenPiceSafeHouse[2],1);
					totalGreenInHouse += 1;
//					print ("Cool");
					GreenPlayerIII_Button.enabled = false;
					if (PhotonNetwork.IsMasterClient) {
						PlayerCanPlayAgain = true;
					}
					else 
					{

					}
				}
				else 
				{
//					print ("You need" + (greenMovementBlock.Count - GreenPlayer_Steps[2]).ToString () + "To enter in safe house");
					if (GreenPlayer_Steps[0] + GreenPlayer_Steps[1] + GreenPlayer_Steps[2] == 0 && selectDiceNumAnimation != 6) 
					{
						playerTurn = "BLUE";
						InitializeDice();
					}
				}
			}
			if (GreenPlayer_Steps [2] == 9 || GreenPlayer_Steps [2] == 22 || GreenPlayer_Steps [2] == 35 || GreenPlayer_Steps [2] == 48 || GreenPlayer_Steps [2] == 14 || GreenPlayer_Steps [2] == 40) {
				if (soundValue.Equals ("on")) {
					this.GetComponent<AudioSource> ().clip = AudioClips [7];
					this.GetComponent<AudioSource> ().Play ();
				}
			}
		}

		//This is Green player 4th Pice method by Executing this 4th pice of Green player can move 
		//if  this pice is already came out from home and got 6 by tossing the turn then the turn remains at this player side
		//if three pice of Green player is already came out from home and this 1 pice is in house and got 1 or 6 by tossing the dice then also the turn remains at this player side

		void GreenPlayerIV_UI()
		{

			if (soundValue.Equals ("on")) {
				this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [0];
				this.gameObject.GetComponent<AudioSource> ().Play ();
			}

//			print ("Executing GreenPlayerIV_UI()");
			if (playerTurn == "GREEN" && (greenMovementBlock.Count - GreenPlayer_Steps[3]) > selectDiceNumAnimation) 
			{
				if (GreenPlayer_Steps[3] > 0) 
				{
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++) 
					{
						greenPlayer_Path [i] = greenMovementBlock [GreenPlayer_Steps[3] + i].transform.position;
					}

					GreenPlayer_Steps[3] += selectDiceNumAnimation;

					//============Keeping the Turn to blue Players side============//
					if (selectDiceNumAnimation == 6) 
					{
						playerTurn = "GREEN";	
						PlayerCanPlayAgain = true;
					}
					else 
					{
						playerTurn = "BLUE";
					}

					currentPlayerName = "GREEN PLAYER IV";

					MovingBlueOrGreenPlayer (GreenPlayerIV, greenPlayer_Path);
				} 
				else 
				{
					if ((selectDiceNumAnimation == 6 || selectDiceNumAnimation==1) && GreenPlayer_Steps[3] == 0) 
					{

						if (soundValue.Equals ("on")) {
							this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [6];
							this.gameObject.GetComponent<AudioSource> ().Play ();
						}

//						print ("Activating 1st green player 1st time");
						Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];
						greenPlayer_Path [0] = greenMovementBlock [GreenPlayer_Steps[3]].transform.position;
						GreenPlayer_Steps[3] += 1;
						playerTurn = "GREEN";
						currentPlayerName = "GREEN PLAYER IV";
						iTween.MoveTo (GreenPlayerIV, iTween.Hash ("position", greenPlayer_Path [0], "speed",600, "time", 0.4f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
						PlayerCanPlayAgain = true;
					}
				}
			}
			else 
			{
				//condition when player coin is reached successfully in house

				if (playerTurn == "GREEN" && (greenMovementBlock.Count - GreenPlayer_Steps[3]) == selectDiceNumAnimation) 
				{
					Vector3[] greenPlayer_Path = new Vector3[selectDiceNumAnimation];

					for (int i = 0; i < selectDiceNumAnimation; i++)
					{
						greenPlayer_Path [i] = greenMovementBlock [GreenPlayer_Steps[3] + i].transform.position;
					}

					GreenPlayer_Steps[3] += selectDiceNumAnimation;

					playerTurn = "GREEN";

					if (soundValue.Equals ("on")) {
						this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [2];
						this.gameObject.GetComponent<AudioSource> ().Play ();
					}

					MovingBlueOrGreenPlayerInSafeHouse (GreenPlayerIV, GreenPiceSafeHouse[3] );
					totalGreenInHouse += 1;
//					print ("Cool");
					GreenPlayerIV_Button.enabled = false;
					if (PhotonNetwork.IsMasterClient) {
						PlayerCanPlayAgain = true;
					}
					else 
					{

					}
				}
				else 
				{
					print ("You need" + (greenMovementBlock.Count - GreenPlayer_Steps[3]).ToString () + "To enter in safe house");
					if (GreenPlayer_Steps[0] + GreenPlayer_Steps[1] + GreenPlayer_Steps[3] == 0 && selectDiceNumAnimation != 6) 
					{
						playerTurn = "BLUE";
						InitializeDice();
					}
				}
			}
			if (GreenPlayer_Steps [3] == 9 || GreenPlayer_Steps [3] == 22 || GreenPlayer_Steps [3] == 35 || GreenPlayer_Steps [3] == 48 || GreenPlayer_Steps [3] == 14 || GreenPlayer_Steps [3] == 40) {
				if (soundValue.Equals ("on")) {
					this.GetComponent<AudioSource> ().clip = AudioClips [7];
					this.GetComponent<AudioSource> ().Play ();
				}
			}
		}
		public string temp;


		//These are Pice related methods which will be called only when the pice are active and tapping on the pice 
		public void BluePlayerI_UI_Method()
		{
			//this method calls the BluePlayerI_UI() by sending the RandomNumber and MethodName;
			if (playerTurn.Equals ("BLUE") && PhotonNetwork.IsMasterClient && !turnManager.IsFinishedByMe) {
				DisablingButtonsOFBluePlayes ();
				DisablingBordersOFBluePlayer ();

				childNode.Add ("MethodName", "BluePlayerI_UI");
				childNode.Add ("RandomNumber", selectDiceNumAnimation.ToString ());
				childNode.Add ("Wait", "2.5");

				temp = childNode.ToString ();
				this.MakeTurn (temp);
			}
		}
		public void BluePlayerII_UI_Method()
		{
			//this method calls the BluePlayerII_UI() by sending the RandomNumber and MethodName;
			if (playerTurn.Equals ("BLUE") && PhotonNetwork.IsMasterClient && !turnManager.IsFinishedByMe) {
				DisablingButtonsOFBluePlayes ();
				DisablingBordersOFBluePlayer ();

				childNode.Add ("MethodName", "BluePlayerII_UI");
				childNode.Add ("RandomNumber", selectDiceNumAnimation.ToString ());
				childNode.Add ("Wait", "2.5");

				temp = childNode.ToString ();
				this.MakeTurn (temp);
			}
		}
		public void BluePlayerIII_UI_Method()
		{
			//this method calls the BluePlayerIII_UI() by sending the RandomNumber and MethodName;
			if (playerTurn.Equals ("BLUE") && PhotonNetwork.IsMasterClient && !turnManager.IsFinishedByMe) {
				DisablingButtonsOFBluePlayes ();
				DisablingBordersOFBluePlayer ();

				childNode.Add ("MethodName", "BluePlayerIII_UI");
				childNode.Add ("RandomNumber", selectDiceNumAnimation.ToString ());
				childNode.Add ("Wait", "2.5");

				temp = childNode.ToString ();
				this.MakeTurn (temp);
			}
		}
		public void BluePlayerIV_UI_Method()
		{
			//this method calls the BluePlayerIV_UI() by sending the RandomNumber and MethodName;
			if (playerTurn.Equals ("BLUE") && PhotonNetwork.IsMasterClient && !turnManager.IsFinishedByMe) {
				DisablingButtonsOFBluePlayes ();
				DisablingBordersOFBluePlayer ();

				childNode.Add ("MethodName", "BluePlayerIV_UI");
				childNode.Add ("RandomNumber", selectDiceNumAnimation.ToString ());
				childNode.Add ("Wait", "2.5");

				temp = childNode.ToString ();
				this.MakeTurn (temp);
			}
		}

		public void GreenPlayerI_UI_Method()
		{
			//this method calls the GreenPlayerI_UI() by sending the RandomNumber and MethodName;
			if (playerTurn.Equals ("GREEN") && !PhotonNetwork.IsMasterClient && !turnManager.IsFinishedByMe) {
				DisablingBordersOFGreenPlayer ();
				DisablingButtonsOfGreenPlayers ();

				childNode.Add ("MethodName", "GreenPlayerI_UI");
				childNode.Add ("RandomNumber", selectDiceNumAnimation.ToString ());
				childNode.Add ("Wait", "2.5");

				temp = childNode.ToString ();
				this.MakeTurn (temp);
			}
		}
		public void GreenPlayerII_UI_Method()
		{
			//this method calls the GreenPlayerII_UI() by sending the RandomNumber and MethodName;
			if (playerTurn.Equals ("GREEN") && !PhotonNetwork.IsMasterClient && !turnManager.IsFinishedByMe) {
				DisablingBordersOFGreenPlayer ();
				DisablingButtonsOfGreenPlayers ();

				childNode.Add ("MethodName", "GreenPlayerII_UI");
				childNode.Add ("RandomNumber", selectDiceNumAnimation.ToString ());
				childNode.Add ("Wait", "2.5");

				temp = childNode.ToString ();
				this.MakeTurn (temp);
			}
		}
		public void GreenPlayerIII_UI_Method()
		{
			//this method calls the GreenPlayerIII_UI() by sending the RandomNumber and MethodName;
			if (playerTurn.Equals ("GREEN") && !PhotonNetwork.IsMasterClient && !turnManager.IsFinishedByMe) {
				DisablingBordersOFGreenPlayer ();
				DisablingButtonsOfGreenPlayers ();

				childNode.Add ("MethodName", "GreenPlayerIII_UI");
				childNode.Add ("RandomNumber", selectDiceNumAnimation.ToString ());
				childNode.Add ("Wait", "2.5");

				temp = childNode.ToString ();
				this.MakeTurn (temp);
			}
		}
		public void GreenPlayerIV_UI_Method()
		{
			//this method calls the GreenPlayerIV_UI() by sending the RandomNumber and MethodName;
			if (playerTurn.Equals ("GREEN") && !PhotonNetwork.IsMasterClient && !turnManager.IsFinishedByMe) {
				DisablingBordersOFGreenPlayer ();
				DisablingButtonsOfGreenPlayers ();

				childNode.Add ("MethodName", "GreenPlayerIV_UI");
				childNode.Add ("RandomNumber", selectDiceNumAnimation.ToString ());
				childNode.Add ("Wait", "2.5");

				temp = childNode.ToString ();
				this.MakeTurn (temp);
			}
		}
		void MovingBlueOrGreenPlayer(GameObject Player,Vector3[] path)
		{
			if (path.Length > 1) 
			{
				iTween.MoveTo (Player, iTween.Hash ("path", path, "speed", 600, "time", 0.4f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
			} 
			else
			{
				iTween.MoveTo (Player, iTween.Hash ("position", path [0], "speed", 600, "time", 0.4f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
			}
		}
		void MovingBlueOrGreenPlayerInSafeHouse(GameObject Player,Vector3 path)
		{
			iTween.MoveTo (Player, iTween.Hash ("position", path, "speed", 600, "time", 0.4f, "easetype", "elastic", "looptype", "none", "oncomplete", "InitializeDice", "oncompletetarget", this.gameObject));
		}
		//end


		//This method is executed When the Board scene is loaded 
		//In this method DisconnectTImeout is set
		//TurnManager is script is added to sending the turn
		void Start () 
		{
			PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 15000 ;
			GameObject OneOnOneConnectionManagerController = GameObject.Find ("SceneSWitchController");
			this.turnManager = this.gameObject.AddComponent<PunTurnManager> ();
			this.turnManager.TurnManagerListener = this;
			string name = null;
			if(PhotonNetwork.InRoom)
				name=PhotonNetwork.CurrentRoom.Name;
			ThisRoomName = name;
//			ThisLobbyName = PlayerPrefs.GetString ("amount");
//			print (PlayerPrefs.GetString ("amount"));
//			print (ThisRoomName);


			QualitySettings.vSyncCount = 1;
			Application.targetFrameRate = 30;
			randomNo = new System.Random ();

			TimerOnePosition = ImageOne.transform.position;
			TimerTwoPosition = ImageTwo.transform.position;

			//Player initial positions...........
			 


			if (PhotonNetwork.PlayerList.Length == 1) 
			{
				GreenPlayerI.SetActive (false);
				GreenPlayerII.SetActive (false);
				GreenPlayerIII.SetActive (false);
				GreenPlayerIV.SetActive (false);
				TurnImages [3].GetComponent<Image> ().enabled = false;
				TurnImages [4].GetComponent<Image> ().enabled = false;
				TurnImages [5].GetComponent<Image> ().enabled = false;
			}

			DisablingBordersOFBluePlayer ();

			DisablingBordersOFGreenPlayer ();

			playerTurn = "BLUE";
			BlueFrame.SetActive (false);
			GreenFrame.SetActive (false);

			if (PhotonNetwork.IsMasterClient) {
				diceRoll.position = BlueDiceRollPosition.position;
				TimerImage.transform.position = TimerOnePosition;
			} else {
				diceRoll.position = GreenDiceRollPosition.position;
				TimerImage.transform.position = TimerTwoPosition;
			}


			if (PhotonNetwork.PlayerList.Length == 2) {
				EnableFrameAndBorderForFirstTime ();
				ImageFillingCounter = 1;
				DisconnectText.text="Game will start in";

				MasterName.text = "" + PhotonNetwork.LocalPlayer.NickName;
				RemoteText.text = "" + PhotonNetwork.PlayerListOthers [0].NickName;

				if (!PhotonNetwork.IsMasterClient) {
					GameDiceBoard.transform.rotation = Quaternion.Euler (new Vector3(0,0,-180));
					ChatPanelParent.transform.rotation = GameDiceBoard.transform.rotation;
				}

				TurnImages [0].GetComponent<Image> ().enabled = false;
				TurnImages [1].GetComponent<Image> ().enabled = false;
				TurnImages [2].GetComponent<Image> ().enabled = false;
			} 
			else if (PhotonNetwork.PlayerList.Length == 1) {
				DisconnectPanel.SetActive (true);
				ReconnectButton.SetActive(false);
				DisconnectText.text="WAIT TILL THE COUNTDOWN TO CONNECT OTHER PLAYER ,IF THE COUNTDOWN IS OVER YOUR MONEY WILL BE REFUNDED";
			}

			//=====================Setting the initial position======================//

				BluePlayers_Pos [0] = BluePlayerI.transform.position;
				BluePlayers_Pos [1] = BluePlayerII.transform.position;
				BluePlayers_Pos [2] = BluePlayerIII.transform.position;
				BluePlayers_Pos [3] = BluePlayerIV.transform.position;

				GreenPlayers_Pos [0] = GreenPlayerI.transform.position;
				GreenPlayers_Pos [1] = GreenPlayerII.transform.position;
				GreenPlayers_Pos [2] = GreenPlayerIII.transform.position;
				GreenPlayers_Pos [3] = GreenPlayerIV.transform.position;



			if (PhotonNetwork.IsMasterClient) {
				
				int num = int.Parse (PlayerPrefs.GetString ("Avatar"));
//				print (num);
				if (num == 11) {
					num = 0;
				}
				dpMaster.sprite = ProfilePic [num];

			} else if (!PhotonNetwork.IsMasterClient) {
				int num = int.Parse (PlayerPrefs.GetString ("Avatar"));
//				print (num);
				if (num == 11) {
					num = 0;
				}
				dpMaster.sprite = ProfilePic [num];
				StartCoroutine (StartGame());
				isRemotePlayerConnected = true;

				if (this.turnManager.Turn == 0) {
					isMyTurn = true;
				}
					
				rootNode.Add ("WaitingTime", "Skiped");
				temp = rootNode.ToString ();
				this.MakeTurn (temp);

			}
			soundValue = PlayerPrefs.GetString ("soundValue");
			if (PlayerPrefs.GetString ("soundValue").Equals ("on")) {
				SoundOn.SetActive (true);
				SoundOff.SetActive (false);
			}else{
				SoundOn.SetActive (false);
				SoundOff.SetActive (true);
			}
			ActualAmount = int.Parse (PlayerPrefs.GetString ("amount"));

		}









		IEnumerator AmountCheckingAfterEntering()
		{
			print ("AmountCheckingAfterEntering");
			//			http://apienjoybtc.exioms.me/api/Balance/bidgamebalance?userid=2&gamesessionid=1&intWalletType=2&dblamt=10000&gametype=2&roomid=2PLDO1&date=27/02/2019
			string RandomRoomName = PhotonNetwork.CurrentRoom.Name;
			string tempRoomName = RandomRoomName;
			print (tempRoomName);
			print ("http://apienjoybtc.exioms.me/api/Balance/bidgamebalance?userid=" + PlayerPrefs.GetString ("userid") + "&gamesessionid=" + 1 + "&intWalletType=" + 2 + "&dblamt=" + PlayerPrefs.GetString("amount") + "&gametype=" + 4 + "&roomid=" + RandomRoomName + "&date=" + System.DateTime.Now.ToString ("yyyy-MM-dd hh-mm-ss"));
			UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Balance/bidgamebalance?userid="+PlayerPrefs.GetString("userid")+"&gamesessionid="+1+"&intWalletType="+2+"&dblamt="+PlayerPrefs.GetString("amount")+"&gametype="+2+"&roomid="+RandomRoomName+"&date="+System.DateTime.Now.ToString ("yyyy-MM-dd hh-mm-ss"));
			www.chunkedTransfer = false;
			www.downloadHandler = new DownloadHandlerBuffer ();
			yield return www.SendWebRequest ();
			if (www.error != null) {
				print ("Something Went wrong");
			} else {
				print (www.downloadHandler.text);
				RandomRoomName = www.downloadHandler.text;
				RandomRoomName = RandomRoomName.Substring (1, RandomRoomName.Length - 2);
				JSONNode jn = SimpleJSON.JSONData.Parse (RandomRoomName);
				RandomRoomName = null;
				RandomRoomName = jn [0];
				print (RandomRoomName);
				if (RandomRoomName.Equals ("Successfullybidforgame")) {
					isBothPlayerEnteredInRoom = true;
					print ("Amount Debited Sucessfully");

					lossAmount = ActualAmount;
					WinAmount = ActualAmount * 80;
					print (WinAmount);
					WinAmount = WinAmount / 100;
					WinAmount = ActualAmount + WinAmount;
					print ("lossAmount:" + lossAmount + "WinAmount:" + WinAmount);
				} else {
					
				}
			}
		}
		//end


		//This method is executed When any data is send to server 
		public void MakeTurn(string data)
		{
			string temp1 = null;

			temp = data;

			this.turnManager.SendMove (data as object, true);
		}

		#region IPunTurnManagerCallbacks
		public void OnTurnBegins(int turn)
		{
			print ("OnTurnBegins(int turn)");
			if ( PhotonNetwork.IsMasterClient && !turnManager.IsFinishedByMe && playerTurn.Equals("GREEN")) 
			{
				print ("Sending Blank Turn" + " isMyTurn" + isMyTurn);
				ActualPlayerCanPlayAgain = false;
				BlankTurn1.Add ("None", ""+0);
				temp = BlankTurn1.ToString ();
				this.MakeTurn (temp);
			}
		}
		public void OnTurnCompleted(int turn)
		{}
		public void OnPlayerMove(Player player, int turn, object move)
		{}
		public void OnPlayerFinished(Player player, int turn, object move)
		{

			print ("OnPlayerFinished(Player player, int turn, object move) PlayerName:"+player.NickName);
			temp = move as string;

//			print ("Temp" + temp);
			if (temp.Contains ("DiceNumber"))
			{
				JSONNode jn = SimpleJSON.JSONData.Parse (temp);
//				print (jn [0]);
				int Place = int.Parse (jn ["DiceNumber"].Value);
//				print ("Sending DiceNumber");
				StartCoroutine (DiceToss (Place));
			}

			//=====================for BluePlayer====================
//			TimerImage.fillAmount = 1;
			JSONNode jn1 = SimpleJSON.JSONData.Parse (temp);
			if (jn1 [0].Value.Equals ("Master") ) {

				if (PhotonNetwork.IsMasterClient) {
					diceRoll.position = GreenDiceRollPosition.position;
					TimerImage.transform.position =TimerTwoPosition ;
					TimerImage.fillAmount = 1;
					TriggerCounter = 0;
					TriggeredTime = 0;
					timer = 0;
					if (TriggerCounter < 1) 
					{
						TriggerCounter += 1;
						TriggeredTime = (int)Time.time;
					}
				} else {
					diceRoll.position = BlueDiceRollPosition.position;
					TimerImage.transform.position = TimerOnePosition;
					TimerImage.fillAmount = 1;
					TriggerCounter = 0;
					TriggeredTime = 0;
					timer = 0;
					if (TriggerCounter < 1) 
					{
						TriggerCounter += 1;
						TriggeredTime = (int)Time.time;
					}
				}
				DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [6];
				playerTurn = "GREEN";
				GreenFrame.SetActive (true);
				BlueFrame.SetActive (false);
				DisablingBordersOFBluePlayer ();
				if (!PhotonNetwork.IsMasterClient) {
					DiceRollButton.enabled = true;
					DiceRollButton.interactable = true;
				} else {
					DiceRollButton.interactable = false;
				}
				int counter =int.Parse( jn1 [1].Value);
				if (counter > 2) {
					if (PhotonNetwork.IsMasterClient) {
						DisconnectBySelf = true;
						StartCoroutine (AutoDisconnectFromRoom ("YOU ARE NOT PLAYING THE GAME ACTIVELY, SO YOU ARE DISCONNECTED FROM THE GAME"));
					} else {
						PlayerAutoDisconnect = true;
					}
				}
			}

			//=====================for GreenPlayer====================
			if (jn1 [0].Value.Equals ("Remote") ) {
				
				if (!PhotonNetwork.IsMasterClient) {
					diceRoll.position = GreenDiceRollPosition.position;
					TimerImage.transform.position = TimerTwoPosition;
					TimerImage.fillAmount = 1;
					TriggerCounter = 0;
					TriggeredTime = 0;
					timer = 0;
					if (TriggerCounter < 1) 
					{
						TriggerCounter += 1;
						TriggeredTime = (int)Time.time;
					}
				} else {
					diceRoll.position = BlueDiceRollPosition.position;
					TimerImage.transform.position = TimerOnePosition;
					TimerImage.fillAmount = 1;
					TriggerCounter = 0;
					TriggeredTime = 0;
					timer = 0;
					if (TriggerCounter < 1) 
					{
						TriggerCounter += 1;
						TriggeredTime = (int)Time.time;
					}
				}

				DiceRollButton.GetComponent<Image> ().sprite = DiceSprite [6];
				playerTurn = "BLUE";
				BlueFrame.SetActive (true);
				GreenFrame.SetActive (false);
				DisablingBordersOFGreenPlayer ();
				if (PhotonNetwork.IsMasterClient) {
					DiceRollButton.enabled = true;
					DiceRollButton.interactable = true;
				} else {
					DiceRollButton.interactable = false;
				}
				int counter =int.Parse( jn1 [1].Value);
				if (counter > 2) {
					if (!PhotonNetwork.IsMasterClient) {
						DisconnectBySelf = true;
						StartCoroutine (AutoDisconnectFromRoom ("YOU ARE NOT PLAYING THE GAME ACTIVELY, SO YOU ARE DISCONNECTED FROM THE GAME"));
					} else {
						PlayerAutoDisconnect = true;
					}
				}
			}
			string temp1 = temp;
			temp = null;
			int Place1=0;
//			print (jn1 [0]);
			switch (jn1 [0]) 
			{

			case "BluePlayerI_UI":
				Place1 = int.Parse (jn1 ["RandomNumber"].Value);
				selectDiceNumAnimation = Place1;
				BluePlayerI_UI ();
				break;
			case "BluePlayerII_UI":
				Place1 = int.Parse (jn1 ["RandomNumber"].Value);
				selectDiceNumAnimation = Place1;
				BluePlayerII_UI ();
				break;
			case "BluePlayerIII_UI":
				Place1 = int.Parse (jn1 ["RandomNumber"].Value);
				selectDiceNumAnimation = Place1;
				BluePlayerIII_UI ();
				break;
			case "BluePlayerIV_UI":
				Place1 = int.Parse (jn1 ["RandomNumber"].Value);
				selectDiceNumAnimation = Place1;
				BluePlayerIV_UI ();
				break;
			case "GreenPlayerI_UI":
				Place1 = int.Parse (jn1 ["RandomNumber"].Value);
				selectDiceNumAnimation = Place1;
				GreenPlayerI_UI ();
				break;
			case "GreenPlayerII_UI":
				Place1 = int.Parse (jn1 ["RandomNumber"].Value);
				selectDiceNumAnimation = Place1;
				GreenPlayerII_UI ();
				break;
			case "GreenPlayerIII_UI":
				Place1 = int.Parse (jn1 ["RandomNumber"].Value);
				selectDiceNumAnimation = Place1;
				GreenPlayerIII_UI ();
				break;
			case "GreenPlayerIV_UI":
				Place1 = int.Parse (jn1 ["RandomNumber"].Value);
				selectDiceNumAnimation = Place1;
				GreenPlayerIV_UI ();
				break;
			}
			JSONNode jn11 = SimpleJSON.JSONData.Parse (temp1);
			if (jn11 ["WaitingTime"].Value.Equals ("Skiped")) {
				StartCoroutine (AmountCheckingAfterEntering ());
			}
			if (player.IsLocal) 
			{
//				print ("(player.IsLocal) ");
				isMyTurn = false;
				PlayerCanPlayAgain = false;

			}
			else
			{
				StartCoroutine (WaitForSomeTime (temp1));

			}
		}

		IEnumerator WaitForSomeTime(string temp1)
		{
//			print ("StartCoroutine (WaitForSomeTime ())");
//			print ("PlayerCanPlayAgain:" + PlayerCanPlayAgain);
			JSONNode jn = SimpleJSON.JSONData.Parse (temp1);


			string WhatToDo = jn ["WaitingTime"].Value;
//			print(jn ["WaitingTime"].Value);
			float WaitingTime = 0;

			if (jn ["WaitingTime"].Value.Equals ("Skiped")) {
//				isBothPlayerEnteredInRoom = true;
				WaitingTime = 0;
				WhatToDo = null;
			}

			else if (jn ["WaitingTime"].Value.Equals("Skip")) {
//				print ("Skipping turn");
				ActualPlayerCanPlayAgain = true;
				WaitingTime = 0;
				WhatToDo = null;
			}
			else 
			{
				WaitingTime = .5f;
			}
			if (jn ["Wait"].Value.Equals ("2.5")) 
			{
				WaitingTime = 1.6f;
			}
			if (PlayerCanPlayAgain == true) 
			{
				ActualPlayerCanPlayAgain = true;
				PlayerCanPlayAgain = false;
			}
			yield return new WaitForSeconds (0.3f);
			isMyTurn = true;
			if (PlayerCanPlayAgain == true) 
			{
				ActualPlayerCanPlayAgain = true;
				PlayerCanPlayAgain = false;
			}
			if (PhotonNetwork.IsMasterClient)
			{
				StartTurn ();
			}
		}
		public void OnTurnTimeEnds(int turn)
		{

		}
		#endregion
		public void StartTurn()
		{
			print ("StartTurn000");

			//	isMyTurn = true;

			if (PhotonNetwork.IsMasterClient)
			{
				print ("StartTurn1111");

				this.turnManager.BeginTurn();

			}
		}
		void EnableFrameAndBorderForFirstTime()
		{
			GreenPlayerI.SetActive (true);
			GreenPlayerII.SetActive (true);
			GreenPlayerIII.SetActive (true);
			GreenPlayerIV.SetActive (true);

			BlueFrame.SetActive (true);
		}

		//This method is executed when the remote player is connected to this room or reconnected to this room 
		//when the remote player connect to this room 1 st time then the turn is assigned to master player 
		//In this method also the discard room variables are reset.
		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
			Debug.Log("Other player arrived turn = "+ this.turnManager.Turn );

//			print ("PlayerCounter:" + PhotonNetwork.CountOfPlayersInRooms);
			playerCounter += 1;
			if (PhotonNetwork.PlayerList.Length == 2) {
				isRemotePlayerConnected = true;
				ResettingRoomDiscardVariables ();
				ImageFillingCounter = 1;
				if (playerTurn.Equals ("BLUE") && PhotonNetwork.IsMasterClient) {
					isMyTurn = true;
				}
				StartCoroutine (StartGame());
				EnableFrameAndBorderForFirstTime ();
			}
		}

		IEnumerator StartGame()
		{
			ReconnectButton.SetActive (false);
			MasterName.text = "" + PhotonNetwork.LocalPlayer.NickName;
			RemoteText.text = "" + PhotonNetwork.PlayerListOthers [0].NickName;

			DisconnectText.text="Other Player is Connected, Game Start In";
			yield return new WaitForSeconds (3);
			DisconnectText.text=null;
			DisconnectPanel.SetActive (false);
		}

		//end

		void ResettingRoomDiscardVariables()
		{
			isQuitingRoom = false;
			TimePeriodTriggeredTime = 0;
			TimePeriod = 0;
			Countdown.text =null;
			isTimeOut = false;
		}


		//this method is used to Reconnect to the room from which it is disconnected
		public void OnClickReconnectAndReJoin()
		{
			if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) 
			{
//				print ("internet connection");
				PhotonNetwork.ReconnectAndRejoin ();
			}
			else if(Application.internetReachability == NetworkReachability.NotReachable)
			{
//				print ("no Internet connection is there");
				StartCoroutine (NoInternetConnectionWarning ());
			}
//			print ("Attempting to rejoin the room");
		}


		IEnumerator NoInternetConnectionWarning()
		{
			DisconnectText.text = "PLEASE CONNECT TO INTERNET AND THEN CLICK THE BUTTON:";
			yield return new WaitForSeconds (1);
			DisconnectText.text ="DISCONNECTED FROM THE ROOM CLICK THE BUTTON TO REENTER THE ROOM";
		}
		public override void OnLeftRoom ()
		{
			print ("Player left the room");
		}

		//This method is Executed when The Plyer is joined the room
		public override void OnJoinedRoom ()
		{
			print ("Joined Room in Board room");

			isRemotePlayerConnected=true;
			StartCoroutine (StartGame());
			playerCounter += 1;
			if (PhotonNetwork.CurrentRoom.PlayerCount == 2) {
				if (playerTurn.Equals ("GREEN") && !PhotonNetwork.IsMasterClient) {
					isMyTurn=true;
				} else if (playerTurn.Equals ("BLUE") && PhotonNetwork.IsMasterClient) {
					isMyTurn = true;
				}
			}
		}
		//end

		void OnServerConnect()
		{
			print ("Connected");
		}

		void OnServerDisconnect()
		{
			print ("disconnected");
		}


		//this method is executed when player disconnected from the room
		public override void OnPlayerLeftRoom (Photon.Realtime.Player otherPlayer)
		{	

			PlayerCount1 = 1;

			TimePeriodTriggeredTime = 0;

			isRemotePlayerConnected = false;
			FirstTimer = 0;
			SecondTimer = 0;
			Countdown.text = null;




			print ("Player Disconnected");
			if (!PlayerAutoDisconnect) {
				playerCounter -= 1;
				TriggeredTime = 0;
				TriggerCounter = 0;
				timer = 0;
				TimerImage.fillAmount = 1;
				DisconnectPanel.SetActive (true);
				ReconnectButton.SetActive (false);
				DisconnectText.text = null;
				DisconnectText.text = "OTHER PLAYER IS DISCONNECTED, YOU ARE THE WINNER";
				Countdown.GetComponent<Text> ().enabled = false;
				//be the winner
				if (!isMatchOver) {
					BeTheWinner ();
				}

			} else {
				DisconnectPanel.SetActive (true);
				ReconnectButton.SetActive (false);
				DisconnectText.text = null;
				DisconnectText.text = "OTHER PLAYER NOT PLAYING ACTIVELY ,SO YOU ARE THE WINNER";
				if (!isMatchOver) {
					BeTheWinner ();
				}
//				StartCoroutine (RoomDiscard ("OPPONENT IS NOT PLAYING GAME ACTIVELY, SO HE IS DISCONNECTED"));
			}
		}


		void BeTheWinner()
		{
			if (PhotonNetwork.IsMasterClient) {
				if (soundValue.Equals ("on")) {
					this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [3];
					this.gameObject.GetComponent<AudioSource> ().Play ();
				}
				WinPanel.SetActive (true);

				AmountDisplay.GetComponent<Text> ().text = "" + WinAmount;

				StartCoroutine(WinnerAPI("2","2"));

			}

			if (!PhotonNetwork.IsMasterClient) {
				if (soundValue.Equals ("on")) {
					this.gameObject.GetComponent<AudioSource> ().clip = AudioClips [3];
					this.gameObject.GetComponent<AudioSource> ().Play ();
				}
				WinPanel.SetActive (true);

				AmountDisplay.GetComponent<Text> ().text = "" + WinAmount;

				StartCoroutine(WinnerAPI("4","4"));

			}
		}


		public override void OnDisconnected (DisconnectCause cause)
		{
			FirstTimer = 0;
			SecondTimer = 0;
			Countdown.text = null;
			//Enable Reconnection Panel and Reset all the all the values of image and counters
			if (!DisconnectBySelf) {
				playerCounter -= 1;
				TriggeredTime = 0;
				TriggerCounter = 0;
				timer = 0;
				TimerImage.fillAmount = 1;
				DisconnectPanel.SetActive (true);
				StartCoroutine (AutoDisconnectFromRoom ("Disconnected from the room Quiting to main menu"));
			}
			//	base.OnDisconnected (cause);
		}


		//this method executed 60 times /sec
		void Update()
		{	
			if (Input.GetKeyDown (KeyCode.Escape)) {
				print ("Pressing Escape");
				print ("Player Will Logout");
				//			QuitPanel.SetActive (true);
				QuitPanel.GetComponent<Animator> ().SetInteger ("Counter", 1);
			}

			if (PhotonNetwork.InRoom) {
				if ((PhotonNetwork.IsMasterClient && playerTurn.Equals ("BLUE")) || (!PhotonNetwork.IsMasterClient && playerTurn.Equals ("GREEN")) && !turnManager.IsFinishedByMe) {
//				print ("Play");
					DiceRollButton.interactable = true;
					TurnStatusText.text = "PLAY";
				} else {
					DiceRollButton.interactable = false;
					TurnStatusText.text = "WAIT FOR YOUR TURN";
				}


				if ((playerTurn.Equals ("BLUE") && !PhotonNetwork.IsMasterClient && isMyTurn && !turnManager.IsFinishedByMe) ||
				    (playerTurn.Equals ("GREEN") && PhotonNetwork.IsMasterClient && isMyTurn && !turnManager.IsFinishedByMe)) {
					print ("It's not this platyers turn but still turn is coming");
//				ActualPlayerCanPlayAgain = false;
//				BlankTurn1.Add ("None", "" + 0);
//				temp = BlankTurn1.ToString ();
//				this.MakeTurn (temp);
				}
			
				RoomCanBeDiscard ();	

				SendBlankTurn ();

				TimerLogicWhenIsMyTurn ();

				DisableAllButtons ();
			}
		}

		//This method is executed when no player is connected to this room within time
		//this method calls RoomDiscard() coroutine, the player from this room and the bid amount is reverted to his wallet
		void RoomCanBeDiscard(){
			if ( !isQuitingRoom && !isTimeOut && PhotonNetwork.CurrentRoom.PlayerCount == 1 || PlayerCount1==1 ) {
//				print ("RoomCanBeDiscard()");
				if (TimePeriodTriggeredTime == 0) {
					
					TimePeriodTriggeredTime = (int)Time.time;
				}
				if (TimePeriod < 60 && !isTimeOut) {
					TimePeriod = (int)(Time.time - TimePeriodTriggeredTime);
					Countdown.text = "" + (60 - TimePeriod);
				}
				if (TimePeriod == 60 && !isTimeOut) {
					
					//call the room discard coroutine
					isTimeOut=true;
					if (isBothPlayerEnteredInRoom && !isMatchOver) {

						//be the winner

//						BeTheWinner ();

//						StartCoroutine (RoomDiscard ("OTHER PLAYER IS NOT CONNECTED WITHIN TIME SO, QUITING THIS GAME"));

					} else {
						StartCoroutine (AutoDisconnectFromRoom ("OTHER PLAYER IS NOT CONNECTED WITHIN TIME SO, QUITING THIS GAME"));
					}
				}
			}
		}

		//This coroutine is executed after the RoomDiscard() coroutine
		IEnumerator AutoDisconnectFromRoom( string msg ){
			print (msg);
			DisconnectBySelf = true;
			DisconnectPanel.SetActive (true);
			DisconnectText.text = msg;
			yield return new WaitForSeconds (2);
			ReconnectButton.GetComponent<Button> ().interactable = false;
			QuitTheRoom ();
		}



		public void EnableQuitTheRoom()
		{
			QuitPanel.GetComponent<Animator> ().SetInteger ("Counter", 1);
		}
		//=====================Disable the Quit Button=====================//
		public void CancelQuiting()
		{
			QuitPanel.GetComponent<Animator> ().SetInteger ("Counter", 2);
		}
		//=====================Quit the Room and direct to Dashboard=====================//
		public void QuitTheRoom()
		{
			DisconnectBySelf = true;
			isQuitingRoom = true;
			PhotonNetwork.LeaveRoom ();
			PhotonNetwork.LeaveLobby ();
			PhotonNetwork.Disconnect ();

			StartCoroutine (QuitTheRoom1 ());
		}

		IEnumerator QuitTheRoom1()
		{
			yield return new WaitForSeconds (.5f);
			SceneManager.LoadScene ("GameMenu");

			Destroy (GameObject.Find("SceneSwitchController"));

			Destroy (this.gameObject);
		}


		void DisableAllButtons()
		{
			if (!turnManager.IsFinishedByMe){
				TriggerCounter2 = 0;
				TriggeredTime2 = 0;
				timer2 = 0;
				DiceRollButton.interactable = true;
//				DiceRollButton.GetComponent<Image>().sprite=DiceSprite [6];
			}
			else {
				DiceRollButton.interactable = false;
			}
		}




		void SendBlankTurn()
		{
			if (!PhotonNetwork.IsMasterClient && !turnManager.IsFinishedByMe && playerTurn.Equals("BLUE")) {
				print ("Sending Blank Turn" + " isMyTurn" + isMyTurn);
				ActualPlayerCanPlayAgain = false;
				BlankTurn1.Add ("None", "" + 0);
				temp = BlankTurn1.ToString ();
				this.MakeTurn (temp);
			}
		}
		void TimerLogicWhenIsMyTurn()
		{
			if (isRemotePlayerConnected && isBothPlayerEnteredInRoom){
				if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.PlayerCount == 2 && FirstTimer < 1) {
				PlayerCount1 = 2;
					GettingSafePoints ();
				FirstTimer = (int)Time.time + 3;
				print ("Got the First Number" + FirstTimer);
			}
			if (FirstTimer > 0 && SecondTimer < 3) {
//				print ("Countdown:" + ((int)Time.time - (int)FirstTimer));
				SecondTimer = (int)Time.time - (int)FirstTimer;
				Countdown.text = "" + (FirstTimer - (int)Time.time);
			}
		}
//			print ("SecondTimer:"+SecondTimer);
			if (PhotonNetwork.InRoom && !turnManager.IsFinishedByMe && PhotonNetwork.CurrentRoom.PlayerCount == 2 && SecondTimer==3) {

				if (TriggerCounter3 < 1) {
						TriggerCounter3 += 1;
						TriggeredTime = (int)Time.time;
					TempNum = TriggeredTime;
					}
				
				if (TempNum==TriggeredTime && TriggerCounter3 == 1 && timer != 16) {
					TimerImage.fillAmount -= 1.0f / 15 * Time.deltaTime;
					timer = (int)Time.time - TriggeredTime;
				}
				if (TriggerCounter == 1 && timer!=16) 
				{
					TimerImage.fillAmount -= 1.0f / 15 * Time.deltaTime;
					timer = (int)Time.time - TriggeredTime;
				}
				if (TimerImage.fillAmount == 0 && timer==15) {
//					print ("Filling Image");
					string msg = null;
					if (PhotonNetwork.IsMasterClient) {
						msg = "Master";
						BluePlayerBlankTurnCounter += 1;
						if (BluePlayerBlankTurnCounter == 1) {
							TurnImages [2].GetComponent<Image> ().enabled = false;
						}
						else if (BluePlayerBlankTurnCounter == 2) {
							TurnImages [1].GetComponent<Image> ().enabled = false;
						}
						else if (BluePlayerBlankTurnCounter == 3) {
							TurnImages [0].GetComponent<Image> ().enabled = false;
						}
						BlankTurn2.Add ("None1", msg);
						BlankTurn2.Add ("BlankTurnCounter", "" + BluePlayerBlankTurnCounter);
					} else {
						msg = "Remote";
						GreenPlayerBlankTurnCounter += 1;
						if (GreenPlayerBlankTurnCounter == 1) {
							TurnImages [5].GetComponent<Image> ().enabled = false;
						}
						else if (GreenPlayerBlankTurnCounter == 2) {
							TurnImages [4].GetComponent<Image> ().enabled = false;
						}
						else if (GreenPlayerBlankTurnCounter == 3) {
							TurnImages [3].GetComponent<Image> ().enabled = false;
						}
						BlankTurn2.Add ("None1", msg);
						BlankTurn2.Add ("BlankTurnCounter", "" + GreenPlayerBlankTurnCounter);
					}
					print ("Blank Turn Sent");
					temp = BlankTurn2.ToString ();
					this.MakeTurn (temp);
				}
			}else if(PhotonNetwork.InRoom)
			{
				if (!DisconnectBySelf && PhotonNetwork.CurrentRoom.PlayerCount == 2 && turnManager.IsFinishedByMe  && SecondTimer==3) {
					if (TriggerCounter2 < 1 && ImageFillingCounter == 1) {
						ImageFillingCounter = 2;
						TriggerCounter2 += 1;
						TriggeredTime2 = (int)Time.time;
					}
					if (TriggerCounter2 == 1 && timer2 != 16) {
						TimerImage.fillAmount -= 1.0f / 15 * Time.deltaTime;
						timer2 = (int)Time.time - TriggeredTime2;
					}
				}
				if (turnManager.IsFinishedByMe && PhotonNetwork.CurrentRoom.PlayerCount == 2) {
					if (TriggerCounter2 < 1) {
						TriggerCounter2 += 1;
						TriggeredTime2 = (int)Time.time;
						TriggeredTime2 += 2;
					}
					if (timer2 != 3) {
						timer2 = (int)(Time.time - TriggeredTime2);
					} else if (TimerImage.fillAmount != 0) {
						TimerImage.fillAmount -= 1.0f / 15 * Time.deltaTime;
					}
				}
			}

		}
		void GettingSafePoints()
		{
			BluePiceSafeHouse [0] = BluePiceSafeHouseGO [0].transform.position;
			BluePiceSafeHouse [1] = BluePiceSafeHouseGO [1].transform.position;
			BluePiceSafeHouse [2] = BluePiceSafeHouseGO [2].transform.position;
			BluePiceSafeHouse [3] = BluePiceSafeHouseGO [3].transform.position;

			GreenPiceSafeHouse [0] = GreenPiceSafeHouseGO [0].transform.position;
			GreenPiceSafeHouse [1] = GreenPiceSafeHouseGO [1].transform.position;
			GreenPiceSafeHouse [2] = GreenPiceSafeHouseGO [2].transform.position;
			GreenPiceSafeHouse [3] = GreenPiceSafeHouseGO [3].transform.position;
		}
		public void SoundOnOff(){
			
			GameObject SoundToggle = GameObject.Find ("SoundToggle");
//			print (SoundToggle.GetComponent<Toggle> ().isOn);
			if (SoundToggle.GetComponent<Toggle> ().isOn) {
				PlayerPrefs.SetString ("soundValue", "on");
				soundValue = PlayerPrefs.GetString ("soundValue");
				SoundOn.SetActive (true);
				SoundOff.SetActive (false);
			} else if (!SoundToggle.GetComponent<Toggle> ().isOn) {
				PlayerPrefs.SetString ("soundValue", "off");
				soundValue = PlayerPrefs.GetString ("soundValue");
				SoundOn.SetActive (false);
				SoundOff.SetActive (true);
			}
			PlayerPrefs.GetString ("soundValue");
		}
	}
}
