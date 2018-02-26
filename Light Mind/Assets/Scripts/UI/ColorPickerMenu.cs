using Assets.Scripts.Utilities;
using UnityEngine;

public class ColorPickerMenu : MonoBehaviour {

    public GameObject ColorPrefab;
    public GameManager GameManager;

    // Use this for initialization
    void Start () {
		foreach(RayColor color in RayColor.COLORS)
        {
            var currentColor = Instantiate(ColorPrefab, transform);
            var _colorItem = currentColor.GetComponent<ColorItem>();
            _colorItem.SetColor(color);
        }
    }
}
