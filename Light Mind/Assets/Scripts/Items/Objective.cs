using Assets.Scripts.Utilities;
using UnityEngine;

namespace Items
{
    public class Objective : ItemBase
    {
        // Public ray color settings
        public bool Red = true;
        public bool Green = true;
        public bool Blue = true;
        public bool Completed;

		private GameManager _gameManager;

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
        }

        // Check if the objective is completed
        private void CheckCompletion()
        {
            bool redCompletion = !Red;
            bool greenCompletion = !Green;
            bool blueCompletion = !Blue;
            
            foreach (var ray in ReceveidRays)
            {
                redCompletion = redCompletion || ray.Color.R;
                greenCompletion = greenCompletion || ray.Color.G;
                blueCompletion = blueCompletion || ray.Color.B;
            }

            bool completion = redCompletion && greenCompletion && blueCompletion;
            
            if (completion)
            {
                if (!Completed)
                {
                    Debug.LogWarning(string.Format("Objective {0} is completed", transform.gameObject.GetInstanceID()));
                    Completed = true;
                    GameManager.Instance.CheckWinCondition ();

                }

            }
            else
            {
                if (Completed)
                {
                    Debug.LogWarning(string.Format("Objective {0} is not completed anymore",
                        transform.gameObject.GetInstanceID()));
                    Completed = false;
                    GameManager.Instance.CheckWinCondition ();
                }
            }
        }
        
        public override void HandleReceivedRay(Ray ray)
        {
        }
        
        // Launched when a ray hits the filter
        public override void HitEnter(Ray ray)
        {
            base.HitEnter(ray);
            HandleReceivedRay(ray);
        }
    }
}