using UnityEngine;

namespace Assets.Scripts.Map
{
    public class SnapToGrid : MonoBehaviour
    {
        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            var gridObjects = GetComponents<GameObject>();
            foreach (var gridObject in gridObjects)
            {
            }
        }
    }
}