using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOver : MonoBehaviour {

	public Text scoreTxt;
	controls s;

	// Use this for initialization
	void Start () {
		s = GameObject.Find("GameController").GetComponent<controls>();
		scoreTxt.text = s.score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void GoToScene(int x)
	{
		s.score = 0;
		SceneManager.LoadScene(x);
	}
}
