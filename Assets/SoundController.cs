using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SoundController : MonoBehaviour {
	// Use this for initialization
	void Awake () {
		
		GameObject[] obj = GameObject.FindGameObjectsWithTag ("music");
		if (obj.Length > 1)
			Destroy (this.gameObject);
		
		DontDestroyOnLoad (this);

		string AudStatus = PlayerPrefs.GetString ("audiovalue");
		print (AudStatus);
		if (AudStatus.Equals ("on")) {
			this.GetComponent<AudioSource> ().Play ();
		} else {
			this.GetComponent<AudioSource> ().Stop ();
		}
		Scene currScene = SceneManager.GetActiveScene ();
		string sceneName = currScene.name;
	}
	void Update()
	{
		Scene CurrScene = SceneManager.GetActiveScene ();
		string scenename = CurrScene.name;
//		print (scenename); 
		string soundValue = PlayerPrefs.GetString ("audiovalue");
		if (soundValue.Equals ("off")) {
			this.GetComponent<AudioSource> ().Stop ();
		}
		if((scenename.Equals("PlayerVSAI") || scenename.Equals("OneOnOneGameBoard")) ){
			this.GetComponent<AudioSource> ().Stop ();
		}
	}
}
