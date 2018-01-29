using UnityEngine;

namespace Objects
{
    public class Laser : MonoBehaviour
    {
        private LineRenderer _lineRenderer;

        // Use this for initialization
        void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            // Set initial position
            _lineRenderer.SetPosition(0, transform.position);

            // Check if laser hit an object
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.up, out hit))
            {
                if (hit.collider)
                {
                    _lineRenderer.SetPosition(1, hit.point);
                }
            }
            else _lineRenderer.SetPosition(1, transform.up * 5000);
        }
    }
}