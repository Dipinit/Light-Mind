using UnityEngine;
using Random = System.Random;

namespace Objects
{
    public class Laser : MonoBehaviour
    {
        public bool red;
        public bool green;
        public bool blue;
        public float alpha;
        public float LongClickTime = 3f;

        private RayEmitter _rayEmitter;
        private Orientable _orientable;
        private float _lastClickTime;
        private RayColor _color;


        // Use this for initialization
        void Start()
        {
            _rayEmitter = new RayEmitter(GetComponent<LineRenderer>());
            SetColor();

            _orientable = GetComponent<Orientable>();
        }

        // Update is called once per frame
        void Update()
        {            
            if (_color.r != red || _color.g != green || _color.b != blue)
            {
                SetColor();
            }
            _rayEmitter.Emit(_orientable.Orientation);
        }
        
        private void OnMouseUpAsButton()
        {
            // Detect double click
            if(Time.time - _lastClickTime > LongClickTime)
            {
                RandomColor();
            }
            _lastClickTime = Time.time;
        }
        
        void SetColor()
        {
            _color = new RayColor(red, green, blue, 0.9f);
            _rayEmitter.SetRayColor(_color);
        }

        void RandomColor()
        {
            red = UnityEngine.Random.value > 0.5f;
            blue = UnityEngine.Random.value > 0.5f;
            green = UnityEngine.Random.value > 0.5f;
            SetColor();
        }
    }
}