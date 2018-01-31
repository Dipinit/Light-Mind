using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayColor {
	
	public bool r;
	public bool g;
	public bool b;
	public float alpha;
	
	public RayColor(bool r, bool g, bool b, float alpha)
	{
		this.r = r;
		this.g = g;
		this.b = b;
		this.alpha = alpha;
	}

	public Color GetColor()
	{
		return new Color(r ? 1F : 0F, g ? 1F : 0F, b ? 1F : 0F);
	}
}
