using UnityEngine;

namespace Behaviors
{
    [RequireComponent(typeof(AudioSource))]
    public class DragAndDrop : MonoBehaviour
    {
        [Header("General")]
        public bool IsDraggable = true;
        
        [Header("Snap")]
        public float SnapRange = 1.5f;

        private BoardManager _board;
        private Vector3 _screenPoint;
        private Vector4 _lastPosition;
        private RaySensitive _raySensitive;


        // Closest tile from this object
        private GameObject _closestCell;

        private AudioSource[] _audioSources;

        private void Awake()
        {
            _board = FindObjectOfType<BoardManager>();
            _audioSources = gameObject.GetComponents<AudioSource>();
            _raySensitive = gameObject.GetComponent<RaySensitive>();
        }

        private void OnMouseDown()
        {
            if (!IsDraggable) return;
            
            _lastPosition = transform.position;
            GameManager.Instance.BoardManager.RemoveItemPosition(_lastPosition);
            
            UpdateDraggedPosition();
            if (_raySensitive != null)
                _raySensitive.Disable();
        }

        private void OnMouseDrag()
        {
            if (!IsDraggable) return;
            
            UpdateDraggedPosition();

            // Illuminate nearest tile for board placement
            HighlightNearestCell();
        }

        private void OnMouseUp()
        {
            if (!IsDraggable) return;
            
            DropItem();
        }

        public void UpdateDraggedPosition()
        {
            // Get World Point using the mouse position
            _screenPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            _screenPoint.z = 0;

            // Change GameObject position
            transform.position = _screenPoint;
        }

        public void HighlightNearestCell()
        {
            if (_closestCell) _closestCell.GetComponent<Renderer>().material = _board.CellDefaultMaterial;

            float closestDistance = SnapRange;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, SnapRange);

            if (hitColliders.Length == 0)
            {
                _closestCell = null;
                return;
            }

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject == this) continue;

                var distanceFromCell = Vector3.Distance(transform.position, hitCollider.gameObject.transform.position);
                if (!(distanceFromCell < closestDistance)) continue;

                if (_board.IsOccupied(hitCollider.gameObject.transform.position)) continue;

                closestDistance = distanceFromCell;
                _closestCell = hitCollider.gameObject;
            }

            if (null == _closestCell) return;

            //Debug.Log(string.Format("Closest {0} is at position {1}", _closestCell.gameObject.name,
            //    _closestCell.transform.position));
            _closestCell.GetComponent<Renderer>().material = _board.CellHighlightMaterial;
        }

        public void DropItem()
        {
            // If object is not over board, replace it in inventory or destroy it
            if (_closestCell == null)
            {
                Debug.Log(string.Format("Destroying {0}", gameObject.name));

                // TODO: Replace item in inventory
                Destroy(gameObject);
                RaySensitive raySensitive = gameObject.GetComponent<RaySensitive>();
                if (raySensitive)
                {
                    string itemType = raySensitive.getItemType();
                    Debug.LogWarning(itemType);
                }
                

                GameObject.Find("Inventory").GetComponent<AudioSource>().Play();
            }
            else
            {
                // Else, place on board
                var cellPosition = _closestCell.transform.position;
                transform.position = cellPosition;
                
                GameManager.Instance.BoardManager.AddItemPosition(cellPosition);

                _audioSources[0].Play();
            }

            // Reset all cells color
            _board.ResetCells();
            _closestCell = null;
            
            if (_raySensitive != null)
                _raySensitive.Enable();
        }
    }
}