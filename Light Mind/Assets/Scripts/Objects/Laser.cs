using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Laser : MonoBehaviour
    {
        public bool Red;
        public bool Green;
        public bool Blue;
        public float Alpha;
        public float LongClickTime = 3f;

        private RayEmitter _rayEmitter;
        private Orientable _orientable;
        private float _lastClickTime;
        private RayColor _color;


        // Use this for initialization
        private void Start()
        {
            _rayEmitter = new RayEmitter(GetComponent<LineRenderer>());
            SetColor();

            _orientable = GetComponent<Orientable>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (_color.R != Red || _color.G != Green || _color.B != Blue)
            {
                SetColor();
            }

            _rayEmitter.Emit(_orientable.Orientation);
        }

        private void OnMouseUpAsButton()
        {
            // Detect double click
            if (Time.time - _lastClickTime > LongClickTime)
            {
                RandomColor();
            }

            _lastClickTime = Time.time;
        }

        void SetColor()
        {
            _color = new RayColor(Red, Green, Blue, 0.9f);
            _rayEmitter.SetRayColor(_color);
        }

        void RandomColor()
        {
            Red = Random.value > 0.5f;
            Blue = Random.value > 0.5f;
            Green = Random.value > 0.5f;
            SetColor();
        }
    }
}