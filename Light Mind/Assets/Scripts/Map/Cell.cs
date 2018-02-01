using System;
using UnityEngine;

namespace Assets.Scripts.Map
{
    [Serializable]
    public class Cell
    {
        public Sprite Sprite;
        public Vector2 Coords;

        public Cell(Sprite sprite, Vector2 coords)
        {
            Sprite = sprite;
            Coords = coords;
        }
    }
}