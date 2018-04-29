using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class FunfactItem : MonoBehaviour
{
    public string name;
    public string filename;
    public bool isInteractable = false;
    public GameObject ScrollFunFact;
  	public GameObject ScrollFunFactDetail;

    public void Select()
    {
      string jsonText = GameManager.LoadFile(filename);
      JSONObject dataAsJson = new JSONObject(jsonText);
      PlayerPrefs.SetString("FunFact", dataAsJson["FunFact"].str);
      Debug.Log(dataAsJson["FunFact"].str);
      Debug.Log(filename);
      ScrollFunFact.SetActive(false);
      ScrollFunFactDetail.SetActive(true);
    }
    /// <summary>
    /// Set the text of the funfact.
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        var _text = this.GetComponentInChildren<Text>();
        _text.text = name;
    }

    /// <summary>
    /// Set the filename for the funfact.
    /// </summary>
    /// <param name="filename"></param>
    public void SetFilename(string filename)
    {
        this.filename = filename;
    }

    /// <summary>
    /// Set the funfact as active and loadable.
    /// </summary>
    /// <param name="interactable"></param>
    public void SetInteractable(bool interactable)
    {
        var _button = this.GetComponentInChildren<Button>();
        this.isInteractable = interactable;
        _button.interactable = this.isInteractable;
    }
}
