using System.IO;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public static class SpriteUtility
    {
        /// <summary>
        /// Load a sprite.
        /// </summary>
        /// <param name="filePath">Path of the sprite.</param>
        /// <param name="width">Width of the sprite.</param>
        /// <param name="height">Height of the sprite.</param>
        /// <param name="pixelsPerUnit">Pixels per unit.</param>
        /// <param name="spriteType">Type of sprite.</param>
        /// <returns></returns>
        public static Sprite LoadSprite(string filePath, int width, int height, float pixelsPerUnit = 100.0f,
            SpriteMeshType spriteType = SpriteMeshType.Tight)
        {
            if (File.Exists(filePath))
            {
                var fileData = File.ReadAllBytes(filePath);
                var tex2D = new Texture2D(width, height);
                if (!tex2D.LoadImage(fileData))
                {
                    // Load the imagedata into the texture (size is set automatically)
                    Debug.Log("Error while loading image"); // If data = readable -> return texture
                }
                else
                {
                    return Sprite.Create(tex2D, new Rect(0, 0, width, height), new Vector2(0, 0), pixelsPerUnit, 0,
                        spriteType);
                }
            }

            Debug.Log("Invalid Path");
            return null;
        }
    }
}