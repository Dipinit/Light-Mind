using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpriteUtility
{
	
	public static Sprite LoadSprite (string FilePath, int width, int height, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
	{
		Texture2D Tex2D;
		byte[] FileData;
		Sprite NewSprite = new Sprite ();

		if (File.Exists (FilePath)) {
			FileData = File.ReadAllBytes (FilePath);
			Tex2D = new Texture2D (width, height);       // Create new "empty" texture
			if (!Tex2D.LoadImage (FileData)) {           // Load the imagedata into the texture (size is set automatically)
				Debug.Log ("Error while loading image"); // If data = readable -> return texture
			} else {
				return Sprite.Create (Tex2D, new Rect (0, 0, width, height), new Vector2 (0, 0), PixelsPerUnit, 0, spriteType);
			}
		}
		Debug.Log ("Invalid Path");
		return null;
	}
}
