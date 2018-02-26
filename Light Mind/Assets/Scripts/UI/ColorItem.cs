using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class ColorItem : MonoBehaviour
{
    public RayColor Color;

    public void OnMouseDown()
    {
        Debug.LogWarning(" ******************** Hello you ********************");
        GameManager.Instance.SetSelectedItemColor(Color);
        GameManager.Instance.HideColorPanel();
    }

    public void SetColor(RayColor color)
    {
        Color = color;
        var _image = this.GetComponentInChildren<Image>();
        var _text = this.GetComponentInChildren<Text>();
        _text.text = Color.Name;
        _image.color = Color.GetColor();
    }
}
