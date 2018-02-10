using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class controls : MonoBehaviour
{
	public bool updateOn = true;

	public int maxSize; // maximum size of the snake
	public int currentSize; // current size of the snake

	public int xBound; // boundaries
	public int yBound;

	public int score = 0;

	public GameObject foodPrefab;
	public GameObject currentFood;
	public GameObject superFood;
	public GameObject superFoodPrefab;
	public GameObject boundaryPrefab;
	public GameObject snakePrefab;
	public Snek head; // snake's head
	public Snek tail; // rest of the snake
	public GameObject boundaries;
	public int NESW; // north east south west- the direction where the snake goes
	public Vector2 nextPos; // next position of the snake

	public Text scoreText;

	public float interval = .5f; // how quick superfood flashes before it vanishes

	void Awake() //used to collect the score at the game over screen
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	void OnEnable()
	{
		Snek.hit += hit;
	}

	void Start()
	{
		InvokeRepeating("TimerInvoke", 0, .4f);
		// here you can modify the SNEK speed - the lower the value the faster the speed
		FoodFunction();
		SuperFoodFunction();
		BoundaryFunction();

	}

	void Flash()
	{
		if (superFood.activeSelf)
			superFood.SetActive(false);
		else
			superFood.SetActive(true);
	}

	private void BoundaryFunction() // setting the real boundaries to the game
	{
		int x = xBound + 2;
		int y = yBound;
		for (int i = 0; i < 12; i++)
		{
			boundaries = (GameObject)Instantiate(boundaryPrefab, new Vector2(x--, y), transform.rotation);
		}
		for (int i = 0; i < 17; i++)
		{
			boundaries = (GameObject)Instantiate(boundaryPrefab, new Vector2(x, y--), transform.rotation);
		}
		for (int i = 0; i < 12; i++)
		{
			boundaries = (GameObject)Instantiate(boundaryPrefab, new Vector2(x++, y), transform.rotation);
		}
		for (int i = 0; i < 17; i++)
		{
			boundaries = (GameObject)Instantiate(boundaryPrefab, new Vector2(x, y++), transform.rotation);
		}
	}

	void OnDisable()
	{
		Snek.hit -= hit;
	}

	// Update is called once per frame
	void Update()
	{
		if (updateOn == true)
		{
			//here is defined percent chance for every frame that superfood will spawn
			int percent=15;
			System.Random r = new System.Random();
			if (r.Next(1, 101) < percent)
			{
				SuperFoodFunction();
			}
		}

		// if you want certain parts of update to work at all times write them here.

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

	void Controlls() // controlls for the snake
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
		if (x == 0)
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
		if (x == 1)
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

	void TailFunction() //deleting the tail
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

		//Creating food
		currentFood = (GameObject)Instantiate(foodPrefab, new Vector2(xPos, yPos), transform.rotation);
		StartCoroutine(CheckRender(currentFood));
	}

	void SuperFoodFunction()
	{

		int xPos = UnityEngine.Random.Range(-xBound, xBound);
		int yPos = UnityEngine.Random.Range(-yBound, yBound);

		superFood = (GameObject)Instantiate(superFoodPrefab, new Vector2(xPos, yPos), transform.rotation);
		updateOn = false;  //stopping the chance to spawn superfood
		InvokeRepeating("Flash", 7, interval); //starting flash for 7 seconds
		StartCoroutine(Wait());
		
		StartCoroutine(CheckRender(superFood));

	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(10);
		Destroy(superFood);
		CancelInvoke(methodName: "Flash"); // ending flash

		
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
			if (IN.tag == "SuperFood")
			{
				Destroy(IN);
				updateOn = true;
			}
		}

	}

	void hit(string WhatWasSent) // differencies between objects that snake approaches
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
		if (WhatWasSent == "SuperFood")
		{
			score = score + 10;
			maxSize++;
			scoreText.text = score.ToString();
			Destroy(superFood);
			updateOn = true; // enabling the chance to spawn superfood
		}
	}

	public void Exit()
	{
		SceneManager.LoadScene(2);
	}


}