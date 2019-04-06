using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPlayerIV_Script : MonoBehaviour {

	public static string YellowPlayerIV_ColName = null;

	void OnTriggerEnter2D(Collider2D col)
	{

		if (col.gameObject.tag == "blocks") 
		{

			YellowPlayerIV_ColName = col.gameObject.name;

			if (col.gameObject.name.Contains ("Safe House")) 
			{

				print ("Entered PlayerI YellowI in safe house");

			}
		}
	}
	// Use this for initialization
	void Start () {
		YellowPlayerIV_ColName = "none";	
	}

}
