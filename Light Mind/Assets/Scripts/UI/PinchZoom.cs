using UnityEngine;
using System.Collections;

public class PinchZoom : MonoBehaviour {

    public float PanSpeed = 20f;
    public float ZoomSpeedTouch = 5f;
    public float ZoomSpeedMouse = 10f;
    public float PanFingersDistance = 1f;
    public float XMin = -5f;
    public float XMax = 10f;
    public float YMin = -5f;
    public float YMax = 10f;
    public float ZoomMin = 2f;
    public float ZoomMax = 12f;
    public float OverlapRadius = 1f;
    
    private Camera _camera;
    
    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only
    
    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only

    void Awake() {
        _camera = GetComponent<Camera>();
    }
    
    void Update() {
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer) {
            HandleTouch();
        } else {
            HandleMouse();
        }
    }
    
    void HandleTouch()
    {
        // TODO: Handle panning (code below is never called)
        if (Input.touchCount == 12) // Pan
        {
            wasZoomingLastFrame = false;

            // If the touch began, capture its position and its finger ID.
            // Otherwise, if the finger ID of the touch doesn't match, skip it.
            Touch touch = Input.GetTouch(0);

            Collider[] colliders = Physics.OverlapSphere(new Vector3(touch.position.x, touch.position.y, 0),
                OverlapRadius, LayerMask.NameToLayer("Items"));
            if (colliders.Length > 0) return;

            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = touch.position;
                panFingerId = touch.fingerId;
            }
            else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
            {
                PanCamera(touch.position);
            }
        }
        else if (Input.touchCount == 2)
        {
            Vector2[] newPositions = new Vector2[]{Input.GetTouch(0).position, Input.GetTouch(1).position};
            if (!wasZoomingLastFrame) {
                lastZoomPositions = newPositions;
                wasZoomingLastFrame = true;
            } else {
                // Zoom based on the distance between the new positions compared to the 
                // distance between the previous positions.
                float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                float offset = oldDistance - newDistance;
    
                ZoomCamera(offset, ZoomSpeedTouch / 1000f);
    
                lastZoomPositions = newPositions;
            }
        }
        else
        {
            wasZoomingLastFrame = false;
        }
    }
    
    void HandleMouse() {
        Collider[] colliders = Physics.OverlapSphere(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0), OverlapRadius);
        if (colliders.Length > 0) return;
        
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(2)) {
            lastPanPosition = Input.mousePosition;
        } else if (Input.GetMouseButton(2)) {
            PanCamera(Input.mousePosition);
        }
    
        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);
    }
    
    void PanCamera(Vector3 newPanPosition) {
        // Determine how much to move the camera
        Vector3 offset = _camera.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, offset.y * PanSpeed, 0);
       
        
        // Perform the movement
        transform.Translate(move, Space.World);  
        
        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, XMin, XMax);
        pos.y = Mathf.Clamp(transform.position.y, YMin, YMax);
        transform.position = pos;
    
        // Cache the position
        lastPanPosition = newPanPosition;
    }
    
    void ZoomCamera(float offset, float speed) {
        if (offset == 0) {
            return;
        }

        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + offset * speed, ZoomMin, ZoomMax);
    }
}