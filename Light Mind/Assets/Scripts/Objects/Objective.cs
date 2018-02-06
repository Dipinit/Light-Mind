using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Objective : RaySensitive
    {
        // Public ray color settings
        public bool Red;
        public bool Green;
        public bool Blue;
        public bool Completed;

        // The current objective color
        private RayColor _color;

        // The Mesh Renderer of the GameObject. Used to change its color based on the filter colors.
        private MeshRenderer _meshRenderer;

        // Use this for initialization
        public override void Start()
        {
            base.Start();
            _meshRenderer = GetComponent<MeshRenderer>();
            SetColor();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            // Update the RayColor if a color filter setting was changed
            if (_color.R != Red || _color.G != Green || _color.B != Blue)
            {
                SetColor();
            }

            CheckCompletion();
        }

        // Update the current objective color
        private void SetColor()
        {
            // Calculate the color based on filter color settings
            _color = new RayColor(Red, Green, Blue, 0.9f);
            
            // Changed the color of the object
            _meshRenderer.material.color = _color.GetColor();

            CheckCompletion();
        }

        // Check if the objective is completed
        private void CheckCompletion()
        {
            bool redCompletion = false;
            bool greenCompletion = false;
            bool blueCompletion = false;
            
            foreach (var ray in ReceveidRays)
            {
                redCompletion = redCompletion || ray.Color.R;
                greenCompletion = greenCompletion || ray.Color.G;
                blueCompletion = blueCompletion || ray.Color.B;
            }
            
            if (redCompletion && greenCompletion && blueCompletion)
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
        
        public override void HandleReceivedRay(Ray ray)
        {
        }
        
        // Launched when a ray hits the filter
        public override void HitEnter(Ray ray)
        {
            base.HitEnter(ray);
            Debug.Log("Hit enter in Objective");
            HandleReceivedRay(ray);
        }
    }
}