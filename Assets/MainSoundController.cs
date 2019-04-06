using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainSoundController : MonoBehaviour {
	public GameObject MusicOnImage;
	public GameObject MusicOffImage;
	public GameObject SoundOnImage;
	public GameObject SoundOffImage;
	public GameObject MusicToggle;
	public GameObject SoundToggle;
	public Text MusicText;
	public Text SoundText;
	// Use this for initialization
	void Start () {
		string audValue = PlayerPrefs.GetString ("audiovalue");
		print (audValue);
		if (audValue.Equals ("on")) {
			MusicOnImage.GetComponent<Image> ().enabled = true;
			MusicOffImage.GetComponent<Image>().enabled=false;
			MusicToggle.GetComponent<Toggle> ().isOn = true;
			MusicText.text = "ON";
		} else {
			MusicOnImage.GetComponent<Image> ().enabled = false;
			MusicOffImage.GetComponent<Image>().enabled=true;
			MusicToggle.GetComponent<Toggle> ().isOn = false;
			MusicText.text = "MUTE";
		}
		string audValue2 = PlayerPrefs.GetString ("soundValue");
		print (audValue2);
		if (audValue2.Equals ("on")) {
			SoundOnImage.GetComponent<Image> ().enabled = true;
			SoundOffImage.GetComponent<Image>().enabled=false;
			SoundToggle.GetComponent<Toggle> ().isOn = true;
			SoundText.text = "ON";
		} else {
			SoundOnImage.GetComponent<Image> ().enabled = false;
			SoundOffImage.GetComponent<Image>().enabled=true;
			SoundToggle.GetComponent<Toggle> ().isOn = false;
			SoundText.text = "MUTE";
		}
	}
	
	// Update is called once per frame
	public void MusicOnOffMethod()
	{
		print ("Hello");
		if (MusicToggle.GetComponent<Toggle> ().isOn) {
			MusicText.text = "ON";
			PlayerPrefs.SetString ("audiovalue", "on");
			MusicOnImage.GetComponent<Image> ().enabled = true;
			MusicOffImage.GetComponent<Image>().enabled=false;
			GameObject.FindGameObjectWithTag ("music").GetComponent<AudioSource> ().Play ();
			print ("Hello");
		} else if(!MusicToggle.GetComponent<Toggle> ().isOn){
			MusicText.text = "MUTE";
			PlayerPrefs.SetString ("audiovalue", "off");
			MusicOnImage.GetComponent<Image> ().enabled = false;
			MusicOffImage.GetComponent<Image>().enabled=true;
			GameObject.FindGameObjectWithTag ("music").GetComponent<AudioSource> ().Stop ();
			print ("Hello");
		}
	}
	public void SoundOnOffMethod()
	{
		print ("Hello");
		if (SoundToggle.GetComponent<Toggle> ().isOn) {
			SoundText.text = "ON";
			PlayerPrefs.SetString ("soundValue", "on");
			SoundOnImage.GetComponent<Image> ().enabled = true;
			SoundOffImage.GetComponent<Image>().enabled=false;

		} else if(!SoundToggle.GetComponent<Toggle> ().isOn){
			SoundText.text = "MUTE";
			PlayerPrefs.SetString ("soundValue", "off");
			SoundOnImage.GetComponent<Image> ().enabled = false;
			SoundOffImage.GetComponent<Image>().enabled=true;
		}
	}
}
