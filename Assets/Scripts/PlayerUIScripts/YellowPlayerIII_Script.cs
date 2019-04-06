﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPlayerIII_Script : MonoBehaviour {

	public static string YellowPlayerIII_ColName = null;

	void OnTriggerEnter2D(Collider2D col)
	{

		if (col.gameObject.tag == "blocks") 
		{

			YellowPlayerIII_ColName = col.gameObject.name;

			if (col.gameObject.name.Contains ("Safe House")) 
			{

				print ("Entered PlayerI YellowI in safe house");

			}
		}
	}
	// Use this for initialization
	void Start () {
		YellowPlayerIII_ColName = "none";	
	}

}