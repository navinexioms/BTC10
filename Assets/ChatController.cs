using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.EventSystems;
namespace Photon.Chat
{
	using ExitGames.Client.Photon;
	public class ChatController : MonoBehaviour,IChatClientListener {
		public ChatClient chatClient;
		public GameObject ChatPanel;
		public string username;
		public Text masterText;
		public Text remoteText;

		// Use this for initialization
		void Start()
		{
			Application.runInBackground = true;
			this.chatClient = new ChatClient (this);
			username = PlayerPrefs.GetString ("userid");
			this.chatClient.Connect (Photon.Pun.PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, Photon.Pun.PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion, new AuthenticationValues(username));
		}

		void Update(){
			this.chatClient.Service ();
		}

		void SendMessage1()
		{
			string message = null;

			switch(EventSystem.current.currentSelectedGameObject.name)
			{
			case "1":
				message = "hi";
				break;
			case "2":
				message = "Hello";
				break;
			case "3":
				message = "play fast";
				break;
			case "4":
				message = "keep quite";
				break;
			case "5":
				message = "nice move";
				break;
			case "6":
				message = "i was so lucky";
				break;
			case "7":
				message = "play like a pro";
				break;
			case "8":
				message = "you can win";
				break;
			case "9":
				message = "you are awesome";
				break;
			case "10":
				message = "i will let you win";
				break;
			case "11":
				message = "how did i miss this";
				break;
			case "12":
				message = "i don't seethat coming";
				break;
			case "13":
				message = "are you talking to me";
				break;
			case "14":
				message = "i am ludo king";
				break;
			}
			ChatPanel.SetActive (false);
			this.chatClient.PublishMessage (PlayerPrefs.GetString ("userid"), message);
		}



		public  void OnConnected(){
			print ("*******************************************Connected to chat****************************************************");
			this.chatClient.Subscribe (new string[]{ PlayerPrefs.GetString("roomname") });
			this.chatClient.SetOnlineStatus (ChatUserStatus.Online);
		}



		public void OnGetMessages(string channelName, string[] senders, object[] messages){
			print ("OnGetMessages");
			if (Photon.Pun.PhotonNetwork.IsMasterClient) {
				print ("OnGetMessagesMaster");
				int num = messages.Length - 1;
				masterText.text =""+ messages [num];
			} else {
				print ("OnGetMessagesMaster");
				int num = messages.Length - 1;
				remoteText.text =""+ messages [num];
			}
		}



		public void OnSubscribed(string[] channels, bool[] results){}

		public void OnUnsubscribed(string[] channels){}
		public void OnStatusUpdate(string user, int status, bool gotMessage, object message){}
		public void OnPrivateMessage(string sender, object message, string channelName){}
		public void OnChatStateChange(ChatState state){}
		public void DebugReturn(DebugLevel level, string message){}
		public void OnDisconnected(){}

		public void EnableChatPanel(){
			ChatPanel.SetActive (true);
		}
		public void DisableChatPanel(){
			ChatPanel.SetActive (false);
		}
	}
}
