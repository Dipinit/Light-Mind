using System;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public class JsonArrayUtility
    {
        //Usage:
        //YouObject[] objects = JsonArrayUtility.getJsonArray<YouObject> (jsonString);
        public static T[] GetJsonArray<T>(string json)
        {
            var wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Array;
        }

        //Usage:
        //string jsonString = JsonArrayUtility.arrayToJson<YouObject>(objects);
        public static string ArrayToJson<T>(T[] array)
        {
            var wrapper = new Wrapper<T> {Array = array};
            return JsonUtility.ToJson(wrapper);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Array;
        }
    }
}