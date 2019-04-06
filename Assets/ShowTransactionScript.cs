using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;
public class ShowTransactionScript : MonoBehaviour {
	public GameObject RowData;
	public GameObject AllParentObj;
	public GameObject AllParentObj1;

	public GameObject WinRowData;
	public GameObject WinParentObject;
	public GameObject WinParentObject1;

	public GameObject LossRowData;
	public GameObject LossParentObject;
	public GameObject LossParentObject1;

	public int matchCount,winCount,lossCount;

	public Text TotalMatchText;
	public Text TotalWinText;
	public Text TotalLossCount;

	public GameObject AllTransaction,WinTransaction,LossTransaction;
	// Use this for initialization
	void Start () {
		StartCoroutine (HitTransactionApi ());
	}

	public void WinTransactionMethod()
	{
		TotalWinText.GetComponent<Text> ().enabled = true;
		TotalLossCount.GetComponent<Text> ().enabled = false;
		TotalMatchText.GetComponent<Text> ().enabled = false;

		AllTransaction.SetActive (false);
		WinTransaction.SetActive (true);
		LossTransaction.SetActive (false);
	}

	public void LossTransactionMethod()
	{
		TotalWinText.GetComponent<Text> ().enabled = false;
		TotalLossCount.GetComponent<Text> ().enabled = true;
		TotalMatchText.GetComponent<Text> ().enabled = false;
		AllTransaction.SetActive (false);
		WinTransaction.SetActive (false);
		LossTransaction.SetActive (true);		
	}


	IEnumerator HitTransactionApi(){
		print("http://apienjoybtc.exioms.me/api/Balance/gametransaction?userid=" + PlayerPrefs.GetString ("userid") + "&gamesessionid=1");
		UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Balance/gametransaction?userid=" + PlayerPrefs.GetString ("userid") + "&gamesessionid=1&inttranstype=0"); 

		www.chunkedTransfer = false;
		www.downloadHandler = new DownloadHandlerBuffer ();
		yield return www.SendWebRequest ();
		if (www.error != null) {
			print ("Something went wrong");
		} else {
			string msg = www.downloadHandler.text;
			print (msg);
			msg = msg.Substring (1, msg.Length - 2);
			print (msg);
			if (!msg.Contains ("ul")) {
				msg = msg.Insert (0, "[");
				msg = msg.Insert (msg.Length, "]");
				print (msg);
				JSONNode jn = SimpleJSON.JSONData.Parse (msg);

				print (jn);

				int num1 = 0, num2 = 1, num3 = 2, num4 = 3;
				foreach (JSONNode jn1 in jn.Childs) {
					print (jn1);
					if (jn1 [num1].Value.Equals ("SessionisLogout")) {
						SceneManager.LoadScene ("Home");
					}
					print (jn1 [num1] + " " + jn1 [num2] + " " + jn1 [num3] + " " + jn1 [num4]);

					GameObject data = Instantiate (RowData, AllParentObj.transform.position, AllParentObj.transform.rotation, AllParentObj1.transform);
					data.transform.localScale = AllParentObj.transform.localScale;
					if (jn1 [num1].Value.Equals ("1")) {
						data.transform.GetComponent<Text> ().text = " " + jn1 [num2] + " " + "win" + " " + jn1 [num3] + " " + jn1 [num4];
					} else if (jn1 [num1].Value.Equals ("2")) {
						data.transform.GetComponent<Text> ().text = "  " + jn1 [num2] + "  " + "loss" + "  " + jn1 [num3]+ " " + jn1 [num4];
					} else if (jn1 [num1].Value.Equals ("3")) {
						data.transform.GetComponent<Text> ().text = "  " + jn1 [num2] + "  " + "Returned" + "  " + jn1 [num3]+ " " + jn1 [num4];
					}
					matchCount += 1;
					TotalMatchText.text="TOTAL MATCHES:"+matchCount;
				}
			}
			TotalLossCount.GetComponent<Text> ().enabled = false;
			StartCoroutine (WinTransactioAPI());
		}
	}



	IEnumerator WinTransactioAPI()
	{
		print("http://apienjoybtc.exioms.me/api/Balance/gametransaction?userid=" + PlayerPrefs.GetString ("userid") + "&gamesessionid=1");
		UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Balance/gametransaction?userid=" + PlayerPrefs.GetString ("userid") + "&gamesessionid=1&inttranstype=1"); 

		www.chunkedTransfer = false;
		www.downloadHandler = new DownloadHandlerBuffer ();
		yield return www.SendWebRequest ();
		if (www.error != null) {
			print ("Something went wrong");
		} else {
			string msg = www.downloadHandler.text;
			print (msg);
			msg = msg.Substring (1, msg.Length - 2);
			print (msg);
			if (!msg.Contains ("ul")) {
				msg = msg.Insert (0, "[");
				msg = msg.Insert (msg.Length, "]");
				print (msg);
				JSONNode jn = SimpleJSON.JSONData.Parse (msg);

				print (jn);

				int num1 = 0, num2 = 1, num3 = 2, num4 = 3;
				foreach (JSONNode jn1 in jn.Childs) {
					print (jn1);
					if (jn1 [num1].Value.Equals ("SessionisLogout")) {
						SceneManager.LoadScene ("Home");
					}
					print (jn1 [num1] + " " + jn1 [num2] + " " + jn1 [num3] + " " + jn1 [num4]);

					GameObject data = Instantiate (WinRowData, WinParentObject.transform.position, WinParentObject.transform.rotation, WinParentObject1.transform);
					data.transform.localScale = WinParentObject.transform.localScale;
					if (jn1 [num1].Value.Equals ("1")) {
						data.transform.GetComponent<Text> ().text = " " + jn1 [num2] + " " + "win" + " " + jn1 [num3] + " " + jn1 [num4];
					}
					winCount += 1;
					TotalWinText.text = "TOTAL WIN'S:" + winCount;
					TotalWinText.GetComponent<Text> ().enabled = false;
				}
			}
			WinTransaction.SetActive (false);
			StartCoroutine (LossTransactionAPI());
		}
	}

	IEnumerator LossTransactionAPI()
	{
		print("http://apienjoybtc.exioms.me/api/Balance/gametransaction?userid=" + PlayerPrefs.GetString ("userid") + "&gamesessionid=1");
		UnityWebRequest www = new UnityWebRequest ("http://apienjoybtc.exioms.me/api/Balance/gametransaction?userid=" + PlayerPrefs.GetString ("userid") + "&gamesessionid=1&inttranstype=2"); 

		www.chunkedTransfer = false;
		www.downloadHandler = new DownloadHandlerBuffer ();
		yield return www.SendWebRequest ();
		if (www.error != null) {
			print ("Something went wrong");
		} else {
			string msg = www.downloadHandler.text;
			print (msg);
			msg = msg.Substring (1, msg.Length - 2);
			print (msg);
			if (!msg.Contains ("ul")) {
				msg = msg.Insert (0, "[");
				msg = msg.Insert (msg.Length, "]");
				print (msg);
				JSONNode jn = SimpleJSON.JSONData.Parse (msg);

				print (jn);

				int num1 = 0, num2 = 1, num3 = 2, num4 = 3;
				foreach (JSONNode jn1 in jn.Childs) {
					print (jn1);
					if (jn1 [num1].Value.Equals ("SessionisLogout")) {
						SceneManager.LoadScene ("Home");
					}
					print (jn1 [num1] + " " + jn1 [num2] + " " + jn1 [num3] + " " + jn1 [num4]);

					GameObject data = Instantiate (LossRowData, LossParentObject.transform.position, LossParentObject.transform.rotation, LossParentObject1.transform);
					data.transform.localScale = LossParentObject.transform.localScale;
					if (jn1 [num1].Value.Equals ("2")) {
						data.transform.GetComponent<Text> ().text = "  " + jn1 [num2] + "  " + "loss" + "  " + jn1 [num3]+ " " + jn1 [num4];
					}
					lossCount += 1;
					TotalLossCount.text = "TOTAL LOSS:" + lossCount;
				}
			}

			LossTransaction.SetActive (false);
			TotalMatchText.GetComponent<Text> ().enabled = true;
		}
	}
	// Update is called once per frame
}
