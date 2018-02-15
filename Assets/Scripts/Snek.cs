using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Snek : MonoBehaviour {

	private Snek next;
	static public Action<String> hit;

	//setter
	public void Setnext(Snek IN)
	{
		next = IN;
	}

	//getter
	public Snek GetNext()
	{
		return next;
	}

	public void RemoveTail()
	{
		Destroy(this.gameObject);
	}

	// sends tagt to GameController
	void OnTriggerEnter(Collider other)
	{
		if (hit != null)
		{
			hit(other.tag);
		}
		if (other.tag == "Food")
		{
			Destroy(other.gameObject);
		}
		if (other.tag == "SuperFood")
		{
			Destroy(other.gameObject);
		}
	}
}
