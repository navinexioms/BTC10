using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPlayerIII_Script : MonoBehaviour {

	// Use this for initialization
	public static string RedPlayerIII_ColName = null;

	void OnTriggerEnter2D(Collider2D col)
	{

		if (col.gameObject.tag == "blocks") 
		{

			RedPlayerIII_ColName = col.gameObject.name;

			if (col.gameObject.name.Contains ("Safe House")) 
			{

				print ("Entered PlayerI YellowI in safe house");

			}
		}
	}
	// Use this for initialization
	void Start () {
		RedPlayerIII_ColName = "none";	
	}
}
