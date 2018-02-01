using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Objective : MonoBehaviour, IHitObject
    {
        public bool Red;
        public bool Green;
        public bool Blue;
        public bool Completed;

        private RayColor _color;
        private RayColor _hitColor;
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
            if (_color.R != Red || _color.G != Green || _color.B != Blue)
            {
                SetColor();
            }

            CheckCompletion();
        }

        public void HitEnter(Direction hitDirection, RayColor rayColor)
        {
            _hitColor = rayColor;
        }

        public void HitExit()
        {
            _hitColor = null;
        }

        private void SetColor()
        {
            _color = new RayColor(Red, Green, Blue, 0.9f);
            _meshRenderer.material.color = _color.GetColor();
        }

        private void CheckCompletion()
        {
            if (_hitColor != null && _hitColor.R == _color.R && _hitColor.G == _color.G && _hitColor.B == _color.B)
            {
                if (Completed) return;
                Debug.Log(string.Format("Objective {0} is completed", transform.gameObject.GetInstanceID()));
                Completed = true;
            }
            else
            {
                if (!Completed) return;
                Debug.Log(string.Format("Objective {0} is not completed anymore",
                    transform.gameObject.GetInstanceID()));
                Completed = false;
            }
        }
    }
}