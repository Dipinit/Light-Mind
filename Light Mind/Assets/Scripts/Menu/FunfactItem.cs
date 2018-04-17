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

    public void SetName(string name)
    {
        var _text = this.GetComponentInChildren<Text>();
        _text.text = name;
    }

    public void SetFilename(string filename)
    {
        this.filename = filename;
    }

    public void SetInteractable(bool interactable)
    {
        var _button = this.GetComponentInChildren<Button>();
        this.isInteractable = interactable;
        _button.interactable = this.isInteractable;
    }
}
