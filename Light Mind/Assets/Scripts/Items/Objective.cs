using Assets.Scripts.Utilities;
using UnityEngine;

namespace Items
{
    public class Objective : ItemBase
    {
        public bool Completed;

		private GameManager _gameManager;

        // The Mesh Renderer of the GameObject. Used to change its color based on the filter colors.
        private MeshRenderer _meshRenderer;

        // Use this for initialization
        public override void Awake()
        {
            base.Awake();
            IsColorable = false;
            IsOrientable = false;
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            CheckCompletion();
        }

        /// <summary>
        /// Update the current objective color.
        /// </summary>
        /// <param name="color"></param>
        public override void SetColor(RayColor color)
        {
            base.SetColor(color);
            
            // Changed the color of the object
            _meshRenderer.material.color = color.GetColor();
        }

        /// <summary>
        /// Check if the objective is completed.
        /// </summary>
        private void CheckCompletion()
        {
            bool redCompletion = !Color.R;
            bool greenCompletion = !Color.G;
            bool blueCompletion = !Color.B;
            
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

        /// <summary>
        /// Launched when a ray hits the filter.
        /// </summary>
        /// <param name="ray"></param>
        public override void HitEnter(Ray ray)
        {
            base.HitEnter(ray);
            HandleReceivedRay(ray);
        }
    }
}