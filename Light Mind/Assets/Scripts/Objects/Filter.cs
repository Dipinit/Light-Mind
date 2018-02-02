using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Filter : MonoBehaviour, IHitObject
    {
        // The emitter of the filtered ray
        private RayEmitter _rayEmitter;
        
        // The direction of the currently hitting ray
        private Direction _hitDirection;
        
        // The Mesh Renderer of the GameObject. Used to change its color based on the filter colors.
        private MeshRenderer _meshRenderer;
        
        // The current filter color
        private RayColor _color;
        
        // The hitting ray color
        private RayColor _hitColor;

        // Public filter color settings
        public bool Red;
        public bool Green;
        public bool Blue;

        // Use this for initialization
        void Start()
        {
            _rayEmitter = new RayEmitter(GetComponent<LineRenderer>());
            _rayEmitter.Enable(false);
            _hitDirection = Direction.East;
            _meshRenderer = GetComponent<MeshRenderer>();
            _hitColor = new RayColor(true, true, true, 0.9f);
            SetColor();
        }

        // Update is called once per frame
        void Update()
        {
            // Update the RayColor if a color filter setting was changed
            if (_color.R != Red || _color.G != Green || _color.B != Blue)
            {
                SetColor();
            }

            // Draw the filtered ray
            _rayEmitter.Emit(_hitDirection);
        }

        // Update the current filter color
        private void SetColor()
        {
            // Calculate the color based on filter color settings
            _color = new RayColor(Red, Green, Blue, 0.9f);
            
            // Changed the color of the object
            _meshRenderer.material.color = _color.GetColor();
            FilterColor();
        }

        // Calculate the filtered color
        private void FilterColor()
        {
            // Calculate the filtered color
            var filteredColor = new RayColor(Red && _hitColor.R, Green && _hitColor.G, Blue && _hitColor.B, 0.9f);
            
            // Apply it to the emitter
            _rayEmitter.SetRayColor(filteredColor);
        }

        // Launched when a ray hits the filter
        public void HitEnter(Direction hitDirection, RayColor rayColor)
        {
            // Enable the emitter
            _rayEmitter.Enable(true);
            
            // Save hitting ray color
            _hitColor = rayColor;
            
            // Calculate the filtered color
            FilterColor();
            
            // Save the hitting ray direction
            _hitDirection = hitDirection;
        }

        // Launched when a ray stops hitting the filter
        public void HitExit()
        {
            // Disable the emitter
            _rayEmitter.Enable(false);
        }
    }
}