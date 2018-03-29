using UnityEngine;

public class InputKeyboardEditor : MonoBehaviour
{

    TouchScreenKeyboard keyboard;
    public string levelName;

    public void OpenKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    public void Save()
    {
        // Check that the level is doable
        // Count the number of elements
        // Serialize to a JSON file
    }

    // Update is called once per frame
    void Update()
    {   
        if (TouchScreenKeyboard.visible == false && keyboard != null)
        {
            if (keyboard.done)
            {
                levelName = keyboard.text;
                keyboard = null;
            }
        }
    }
}
