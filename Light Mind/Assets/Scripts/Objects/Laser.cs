using UnityEngine;
using Random = System.Random;

namespace Objects
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


        // Use this for initialization
        void Start()
        {
            _rayEmitter = new RayEmitter(GetComponent<LineRenderer>(), new RayColor(Red, Green, Blue, Alpha));
            _orientable = GetComponent<Orientable>();
        }

        // Update is called once per frame
        void Update()
        {            
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

        void RandomColor()
        {
            Red = UnityEngine.Random.value > 0.5f;
            Blue = UnityEngine.Random.value > 0.5f;
            Green = UnityEngine.Random.value > 0.5f;
            _rayEmitter.rayColor = new RayColor(Red, Green, Blue, Alpha);
        }
    }
}