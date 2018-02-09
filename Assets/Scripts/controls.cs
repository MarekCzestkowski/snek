using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controls : MonoBehaviour {

	public int maxSize;
	public int currentSize;

	public int xBound;
	public int yBound;

	public int score;

	public GameObject foodPrefab;
	public GameObject currentFood;

	public GameObject snakePrefab;
	public Snek head;
	public Snek tail;
	public int NESW; // north east south west
	public Vector2 nextPos;

	void OnEnable()
	{
		Snek.hit += hit;
	}

	// Use this for initialization
	void Start() {
		InvokeRepeating("TimerInvoke", 0, .5f);
		FoodFunction();
	}

	void OnDisable()
	{
		Snek.hit -= hit;
	}

	// Update is called once per frame
	void Update() {
		ChangeDirection();
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
	void ChangeDirection()
	{
		if (NESW != 2 && Input.GetKeyDown(KeyCode.W))
		{
			NESW = 0;
		}
		if (NESW != 3 && Input.GetKeyDown(KeyCode.D))
		{
			NESW = 1;
		}
		if (NESW != 0 && Input.GetKeyDown(KeyCode.S))
		{
			NESW = 2;
		}
		if (NESW != 1 && Input.GetKeyDown(KeyCode.A))
		{
			NESW = 3;
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
		//Randomizing value of food
		int xPos = Random.Range(-xBound, xBound);
		int yPos = Random.Range(-yBound, yBound);

		//Creation of food
		currentFood = (GameObject)Instantiate(foodPrefab, new Vector2(xPos, yPos), transform.rotation);
		StartCoroutine(CheckRender(currentFood));

	}

	IEnumerator CheckRender(GameObject IN) // checking if the object rendered within camera
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
		}
	}

}
