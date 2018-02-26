using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class ColorItem : MonoBehaviour {
    public RayColor Color;
    private void OnMouseDown()
    {
        Debug.Log("Hello you");
        GameManager.Instance.HideColorPanel();
    }

    public void SetColor(RayColor color)
    {
        var _image = this.GetComponentInChildren<Image>();
        var _text = this.GetComponentInChildren<Text>();
        _text.text = color.Name;
        _image.color = color.GetColor();
    }

}
