using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Cell
{
	public Sprite sprite;
	public Vector2 coords;

	public Cell (Sprite sprite, Vector2 coords)
	{
		this.sprite = sprite;
		this.coords = coords;
	}
		
}

