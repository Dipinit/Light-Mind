using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JsonArrayUtility
{
	//Usage:
	//YouObject[] objects = JsonArrayUtility.getJsonArray<YouObject> (jsonString);
	public static T[] getJsonArray<T> (string json)
	{
		Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (json);
		return wrapper.array;
	}
	//Usage:
	//string jsonString = JsonArrayUtility.arrayToJson<YouObject>(objects);
	public static string arrayToJson<T> (T[] array)
	{
		Wrapper<T> wrapper = new Wrapper<T> ();
		wrapper.array = array;
		return JsonUtility.ToJson (wrapper);
	}

	[Serializable]
	private class Wrapper<T>
	{
		public T[] array;
	}
}
