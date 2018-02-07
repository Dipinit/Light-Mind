using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Utilities.Json
{
    [System.Serializable]
    public class Entity
    {
        public string Type;
        public int X;
        public int Y;
        public string Data;

        public Entity(string type, int x, int y, string Data)
        {
            Type = type;
            X = x;
            Y = y;
            this.Data = Data;
        }

        public override string ToString()
        {
            return string.Format("{0} (x: {1}, y: {2})", Type, X, Y);
        }
    }
}