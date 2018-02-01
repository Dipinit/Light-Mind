using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Filter : MonoBehaviour, IHitObject
    {
        private RayEmitter _rayEmitter;
        private Direction _hitDirection;
        private MeshRenderer _meshRenderer;
        private RayColor _color;
        private RayColor _hitColor;

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
            if (_color.R != Red || _color.G != Green || _color.B != Blue)
            {
                SetColor();
            }

            _rayEmitter.Emit(_hitDirection);
        }

        private void SetColor()
        {
            _color = new RayColor(Red, Green, Blue, 0.9f);
            _meshRenderer.material.color = _color.GetColor();
            FilterColor();
        }

        private void FilterColor()
        {
            var filteredColor = new RayColor(Red && _hitColor.R, Green && _hitColor.G, Blue && _hitColor.B, 0.9f);
            _rayEmitter.SetRayColor(filteredColor);
        }

        public void HitEnter(Direction hitDirection, RayColor rayColor)
        {
            _rayEmitter.Enable(true);
            _hitColor = rayColor;
            FilterColor();
            _hitDirection = hitDirection;
        }

        public void HitExit()
        {
            _rayEmitter.Enable(false);
        }
    }
}