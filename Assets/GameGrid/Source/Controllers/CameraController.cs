using GameGrid.Source.Tiles;
using UnityEngine;
using UnityEngine.Events;

namespace GameGrid.Source.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float movementSpeed = 5.0f;
        [SerializeField] private float scrollSpeed = 10.0f;
        
        [SerializeField] private float scrollSizeMin = 3.0f;
        [SerializeField] private float scrollSizeMax = 8.0f;

        public UnityEvent<BaseSquareTile> onClickTile; 

        private void Start()
        {
            playerCamera.orthographicSize = Mathf.Clamp(playerCamera.orthographicSize, scrollSizeMin, scrollSizeMax);
        }

        private void Update()
        {
            CameraMovement();
            CameraScrolling();
            OnClicked();
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void OnClicked()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 clickWorldPosition = playerCamera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(clickWorldPosition, Vector2.zero);

                BaseSquareTile selectedTile = hit.collider?.gameObject.GetComponent<BaseSquareTile>();
                if (selectedTile is not null)
                {
                    onClickTile.Invoke(selectedTile);
                }
            }
        }

        private void CameraScrolling()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            float deltaScroll = scroll * scrollSpeed * Time.deltaTime;

            float orthographicSize = playerCamera.orthographicSize;
            orthographicSize += deltaScroll;
            
            playerCamera.orthographicSize = orthographicSize;
            playerCamera.orthographicSize = Mathf.Clamp(orthographicSize, scrollSizeMin, scrollSizeMax);
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
