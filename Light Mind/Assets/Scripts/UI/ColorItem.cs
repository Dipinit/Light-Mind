using Assets.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class ColorItem : MonoBehaviour
{
    public RayColor Color;

    /// <summary>
    /// Change the color of an item.
    /// </summary>
    public void ChangeColor()
    {
        Debug.LogWarning(Color.GetName());
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetSelectedItemColor(Color);
            GameManager.Instance.HideColorPanel();
        }
        else if (LevelEditorTD.Instance != null)
        {
            LevelEditorTD.Instance.SetSelectedItemColor(Color);
            LevelEditorTD.Instance.HideColorPanel();
        }
    }

    /// <summary>
    /// Set hthe color of an item.
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(RayColor color)
    {
        Color = color;
        var _image = this.GetComponentInChildren<Image>();
        var _text = this.GetComponentInChildren<Text>();
        _text.text = Color.GetName();
        _image.color = Color.GetColor();
    }
}
