using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect : MonoBehaviour {
	public GameObject Cool;
	// Use this for initialization
	void OnTriggerEnter2D(Collider2D col)
	{
		Cool.SetActive (true);
		StartCoroutine (DisableCool());
	}
	IEnumerator DisableCool()
	{
		yield return new WaitForSeconds (1);
		Cool.SetActive (false);
	}
}
