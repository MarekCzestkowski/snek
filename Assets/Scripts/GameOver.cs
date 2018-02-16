using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOver : MonoBehaviour {

	public Text scoreTxt;
	GameController s;
	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.Portrait;
		s = GameObject.Find("GameController").GetComponent<GameController>();
		scoreTxt.text = "Your Score: " + s.score.ToString();
		Destroy(GameObject.Find("GameController"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void GoToScene(int x)
	{
		SceneManager.LoadScene(x);
	}
}
