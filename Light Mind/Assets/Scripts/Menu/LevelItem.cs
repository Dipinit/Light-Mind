using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    public string name;
    public string filename;

    public void Select()
    {
        Debug.LogWarning(this.filename);
  			PlayerPrefs.SetString("currentLevel", this.filename);
  			Application.LoadLevel("Scenes/GameTD3D");
    }

    public void SetName(string name)
    {
        var _text = this.GetComponentInChildren<Text>();
        _text.text = name;
    }

    public void SetFilename(string filename)
    {
        this.filename = filename;
    }
}
