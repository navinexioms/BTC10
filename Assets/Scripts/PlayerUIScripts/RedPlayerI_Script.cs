using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPlayerI_Script : MonoBehaviour {

	public static string RedPlayerI_ColName = null;

	void OnTriggerEnter2D(Collider2D col)
	{

		if (col.gameObject.tag == "blocks") 
		{

			RedPlayerI_ColName = col.gameObject.name;

			if (col.gameObject.name.Contains ("Safe House")) 
			{

				print ("Entered PlayerI YellowI in safe house");

			}
		}
	}
	// Use this for initialization
	void Start () {
		RedPlayerI_ColName = "none";	
	}
}
