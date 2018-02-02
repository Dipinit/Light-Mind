using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Objective : MonoBehaviour, IHitObject
    {
        // Public ray color settings
        public bool Red;
        public bool Green;
        public bool Blue;
        public bool Completed;

        // The current objective color
        private RayColor _color;
        
        // The current hitting color
        private RayColor _hitColor;
        
        // The Mesh Renderer of the GameObject. Used to change its color based on the filter colors.
        private MeshRenderer _meshRenderer;

        // Use this for initialization
        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            SetColor();
        }

        // Update is called once per frame
        private void Update()
        {
            // Update the RayColor if a color filter setting was changed
            if (_color.R != Red || _color.G != Green || _color.B != Blue)
            {
                SetColor();
            }

            // Check if the objective is completed
            CheckCompletion();
        }

        // Launched when a ray hits the objective
        public void HitEnter(Direction hitDirection, RayColor rayColor)
        {
            // Update the hitting ray color
            _hitColor = rayColor;
        }

        // Launched when a ray stops hitting the objective
        public void HitExit()
        {
            // Update the hitting ray color to null
            _hitColor = null;
        }

        // Update the current objective color
        private void SetColor()
        {
            // Calculate the color based on filter color settings
            _color = new RayColor(Red, Green, Blue, 0.9f);
            
            // Changed the color of the object
            _meshRenderer.material.color = _color.GetColor();
        }

        // Check if the objective is completed
        private void CheckCompletion()
        {
            // If the hitting ray colors is equal to the objective color...
            if (_hitColor != null && _hitColor.R == _color.R && _hitColor.G == _color.G && _hitColor.B == _color.B)
            {
                // ... if already completed, return
                if (Completed) return;
                
                // ... if not already completed, log the completion and update the completion
                Debug.Log(string.Format("Objective {0} is completed", transform.gameObject.GetInstanceID()));
                Completed = true;
            }
            else
            {
                // ... if already uncompleted, return
                if (!Completed) return;
                
                // ... if not already uncompleted, log the uncompletion and update the completion
                Debug.Log(string.Format("Objective {0} is not completed anymore",
                    transform.gameObject.GetInstanceID()));
                Completed = false;
            }
        }
    }
}