using System;
using UnityEngine;

namespace Behaviors
{
    [RequireComponent(typeof(AudioSource))]
    public class DragAndDrop : MonoBehaviour
    {
        [Header("Snap")] public float SnapRange = 1.5f;

        private BoardManager _board;
        private Vector3 _screenPoint;
        private Vector4 _lastPosition;

        // Closest tile from this object
        private GameObject _closestCell;

        private AudioSource[] _audioSources;

        private void Awake()
        {
            _board = FindObjectOfType<BoardManager>();
            _audioSources = gameObject.GetComponents<AudioSource>();
        }

        private void OnMouseDown()
        {
            UpdateDraggedPosition();
        }

        private void OnMouseDrag()
        {
            UpdateDraggedPosition();

            // Illuminate nearest tile for board placement
            HighlightNearestCell();
        }

        private void OnMouseUp()
        {
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
            if (_closestCell) _closestCell.GetComponent<SpriteRenderer>().color = _board.CellDefaultColor;

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
                if (!hitCollider is BoxCollider2D) continue;

                var distanceFromCell = Vector3.Distance(transform.position, hitCollider.gameObject.transform.position);
                if (!(distanceFromCell < closestDistance)) continue;

                if (_board.IsOccupied(hitCollider.gameObject.transform.position)) continue;

                closestDistance = distanceFromCell;
                _closestCell = hitCollider.gameObject;
            }

            if (null == _closestCell) return;

            Debug.Log(string.Format("Closest {0} is at position {1}", _closestCell.gameObject.name,
                _closestCell.transform.position));
            _closestCell.GetComponent<SpriteRenderer>().color = _board.CellHighlightColor;
        }
        
        public void DropItem()
        {
            // If object is not over board, replace it in inventory or destroy it
            if (_closestCell == null)
            {
                Debug.Log(string.Format("Destroying {0}", this.gameObject.name));
                
                // TODO: Replace item in inventory
                Destroy(gameObject);

                GameObject.Find("Inventory").GetComponent<AudioSource>().Play();
            }
            else
            {
                // Else, place on board
                var cellPosition = _closestCell.transform.position;
                transform.position = cellPosition;

                _audioSources[0].Play();
            }

            // Reset all cells color
            _board.ResetCells();
            _closestCell = null;
        }
    }
}