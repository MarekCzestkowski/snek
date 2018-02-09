using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class controls : MonoBehaviour {

	public int maxSize;
	public int currentSize;

	public int xBound;
	public int yBound;

	public int score = 0;

	public GameObject foodPrefab;
	public GameObject currentFood;

	public GameObject snakePrefab;
	public Snek head;
	public Snek tail;
	public GameObject boundaries;
	public int NESW; // north east south west
	public Vector2 nextPos;

	public Text scoreText;

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	void OnEnable()
	{
		Snek.hit += hit;
	}

	// Use this for initialization
	void Start() {
		InvokeRepeating("TimerInvoke", 0, .4f);
		// here you can modify the SNEK speed - the lower the value the faster the speed
		FoodFunction();
		BoundaryFunction();
	}

	private void BoundaryFunction()
	{
		int x = xBound+2;
		int y = yBound;
		for (int i= 0; i<12; i++ )
		{
			boundaries = (GameObject)Instantiate(snakePrefab, new Vector2(x--, y), transform.rotation);
		}
		for (int i = 0; i < 17; i++)
		{
			boundaries = (GameObject)Instantiate(snakePrefab, new Vector2(x, y--), transform.rotation);
		}
		for (int i = 0; i < 12; i++)
		{
			boundaries = (GameObject)Instantiate(snakePrefab, new Vector2(x++, y), transform.rotation);
		}
		for (int i = 0; i < 17; i++)
		{
			boundaries = (GameObject)Instantiate(snakePrefab, new Vector2(x, y++), transform.rotation);
		}
	}

	void OnDisable()
	{
		Snek.hit -= hit;
	}

	// Update is called once per frame
	void Update() {
		//ChangeDirection();
	}

	void TimerInvoke()
	{
		Controlls();
		if (currentSize >= maxSize)
		{
			TailFunction();
		}
		else
		{
			currentSize++;
		}
	}

	void Controlls()
	{
		GameObject temp;
		nextPos = head.transform.position;
		//Snake movements
		switch (NESW)
		{
			case 0:
				nextPos = new Vector2(nextPos.x, nextPos.y + 1);
				break;
			case 1:
				nextPos = new Vector2(nextPos.x + 1, nextPos.y);
				break;
			case 2:
				nextPos = new Vector2(nextPos.x, nextPos.y - 1);
				break;
			case 3:
				nextPos = new Vector2(nextPos.x - 1, nextPos.y);
				break;
		}
		temp = (GameObject)Instantiate(snakePrefab, nextPos, transform.rotation);
		head.Setnext(temp.GetComponent<Snek>());
		head = temp.GetComponent<Snek>();

		return;
	}
	//Handles the directon where snake goes
	public void ChangeDirection(int x) //   Right - 0    Left - 1
	{
		if (x==0)
		{
			if (NESW == 3)
			{
				NESW = 0;
			}
			else
			{
				NESW++;
			}
		}
		if (x==1)
		{
			if (NESW == 0)
			{
				NESW = 3;
			}
			else
			{
				NESW--;
			}
		}
	}

	void TailFunction()
	{
		Snek tempSnake = tail;
		tail = tail.GetNext();
		tempSnake.RemoveTail();
	}

	void FoodFunction()
	{
		//Randomizing spawn coordinates of food
		int xPos = UnityEngine.Random.Range(-xBound, xBound);
		int yPos = UnityEngine.Random.Range(-yBound, yBound);

		//Creation of food
		currentFood = (GameObject)Instantiate(foodPrefab, new Vector2(xPos, yPos), transform.rotation);
		StartCoroutine(CheckRender(currentFood));


	}

	IEnumerator CheckRender(GameObject IN) // checking if the object rendered within boundaries
	{
		yield return new WaitForEndOfFrame();
		if (IN.GetComponent<Renderer>().isVisible == false)
		{
			if (IN.tag == "Food")
			{
				Destroy(IN);
				FoodFunction();
			}

		}

	}

	void hit(string WhatWasSent)
	{
		//Collision handle
		if (WhatWasSent == "Food")
		{
			FoodFunction();
			maxSize++;
			score++;
			scoreText.text = score.ToString();
		}
		if (WhatWasSent == "Snek")
		{
			CancelInvoke("TimerInvoke");
			Exit();
		}
	}

	public void Exit()
	{
		SceneManager.LoadScene(2);
	}

}
