using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    // Ray source
    public class Laser : MonoBehaviour
    {
        // Public ray color settings
        public bool Red;
        public bool Green;
        public bool Blue;
        public float Alpha;
        
        // Public long click setting. Used to change the color of the source when holding click
        public float LongClickTime = 3f;

        // The ray emitter
        private RayEmitter _rayEmitter;
        
        // The Orientable component. Used to change the filter direction 
        private Orientable _orientable;
        
        // Last time the laser was clicked. Used to change the color of the source when holding click
        private float _lastClickTime;
        
        // The current generated color
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
            // Update the RayColor if a color filter setting was changed
            if (_color.R != Red || _color.G != Green || _color.B != Blue)
            {
                SetColor();
            }

            // Draw the ray
            _rayEmitter.Emit(_orientable.Orientation);
        }

        private void OnMouseUpAsButton()
        {
            // Detect double click
            if (Time.time - _lastClickTime > LongClickTime)
            {
                // Change laser color
                RandomColor();
            }

            _lastClickTime = Time.time;
        }

        // Update the current filter color
        void SetColor()
        {
            // Calculate the color based on filter color settings
            _color = new RayColor(Red, Green, Blue, 0.9f);
            
            // Apply it to the emitter
            _rayEmitter.SetRayColor(_color);
        }

        // Update the ray color to a random color
        void RandomColor()
        {
            Red = Random.value > 0.5f;
            Blue = Random.value > 0.5f;
            Green = Random.value > 0.5f;
            SetColor();
        }
    }
}