using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Snek : MonoBehaviour {

	private Snek next;
	static public Action<String> hit;

	public void Setnext(Snek IN)
	{
		next = IN;
	}

	public Snek GetNext()
	{
		return next;
	}

	public void RemoveTail()
	{
		Destroy(this.gameObject);
	}

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
	}

}
