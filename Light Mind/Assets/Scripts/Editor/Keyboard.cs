using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardInput : MonoBehaviour
{

    TouchScreenKeyboard keyboard;
    public Text levelName;

    // Use this for initialization
    public void OpenKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
