using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameGrid.Source.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private float movementSpeed = 5.0f;
        [SerializeField] private float scrollSpeed = 10.0f;
        
        [SerializeField] private float scrollSizeMin = 3.0f;
        [SerializeField] private float scrollSizeMax = 8.0f;

        //public UnityEvent onSelectedTile; 

        private void Start()
        {
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, scrollSizeMin, scrollSizeMax);
        }

        private void Update()
        {
            CameraMovement();
            CameraScrolling();
            OnClicked();
        }

        private void OnClicked()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 clickWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(clickWorldPosition, Vector2.zero);
            }
        }

        private void CameraScrolling()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            float deltaScroll = scroll * scrollSpeed * Time.deltaTime;

            float orthographicSize = camera.orthographicSize;
            orthographicSize += deltaScroll;
            
            camera.orthographicSize = orthographicSize;
            camera.orthographicSize = Mathf.Clamp(orthographicSize, scrollSizeMin, scrollSizeMax);
        }

        private void CameraMovement()
        {
            float horizontalMove = Input.GetAxis("Camera Horizontal");
            float verticalMove = Input.GetAxis("Camera Vertical");

            Vector3 deltaMovement = new Vector3(horizontalMove, verticalMove, 0.0f) * (movementSpeed * Time.deltaTime);

            transform.position += deltaMovement;
        }
    }
}
