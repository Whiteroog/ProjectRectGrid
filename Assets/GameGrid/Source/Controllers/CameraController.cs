using System;
using GameGrid.Source.Tiles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameGrid.Source.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 5.0f;
        [SerializeField] private float scrollWheelSpeed = 50.0f;
        [SerializeField] private float scrollKeysSpeed = 5.0f;
        [SerializeField] private float edgeScrollSpeed = 7.0f;
        [SerializeField] private float dragPanSpeed = 2.0f;

        [SerializeField] private float scrollSizeMin = 3.0f;
        [SerializeField] private float scrollSizeMax = 8.0f;
        
        
        [SerializeField] private float edgeScrollSize = 200.0f;

        public UnityEvent<Vector3> onClickTile;

        private Camera _playerCamera;
        
        private bool _isDragPanMoveActive = false;
        private Vector3 _lastMousePosition = Vector3.zero;

        private void Awake()
        {
            _playerCamera = GetComponent<Camera>();
        }

        private void Start()
        {
            _playerCamera.orthographicSize = Mathf.Clamp(_playerCamera.orthographicSize, scrollSizeMin, scrollSizeMax);
        }

        private void Update()
        {
            CameraMovement();
            CameraScrolling();
            OnClicked();
        }

        private void CameraMovement()
        {
            Vector3 deltaMovement = Vector3.zero;
            
            deltaMovement = CameraPanMovement();
            if(!_isDragPanMoveActive)
            {
                deltaMovement = deltaMovement == Vector3.zero ? CameraMovementByEdges() : deltaMovement;
                deltaMovement = deltaMovement == Vector3.zero ? CameraMovementByKeys() : deltaMovement;
            }

            transform.position += deltaMovement;
        }
        
        private Vector3 CameraPanMovement()
        {
            Vector3 mouseMovementDelta = Vector3.zero;
            
            if (Input.GetMouseButtonDown(2))
            {
                _isDragPanMoveActive = true;
                _lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(2))
                _isDragPanMoveActive = false;

            if (_isDragPanMoveActive)
            {
                mouseMovementDelta = (Input.mousePosition - _lastMousePosition) * ((-1.0f) * dragPanSpeed * Time.deltaTime);
                _lastMousePosition = Input.mousePosition;
            }

            return mouseMovementDelta;
        }

        private Vector3 CameraMovementByEdges()
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 inputDir = Vector3.zero;

            if (mousePosition.x < edgeScrollSize)
                inputDir += Vector3.Lerp(Vector3.zero, Vector3.left,  Mathf.Abs(mousePosition.x - edgeScrollSize) / edgeScrollSize);

            if (mousePosition.y < edgeScrollSize)
                inputDir += Vector3.Lerp(Vector3.zero, Vector3.down, Mathf.Abs(mousePosition.y - edgeScrollSize) / edgeScrollSize);
            
            if (mousePosition.x > Screen.width - edgeScrollSize)
                inputDir += Vector3.Lerp(Vector3.zero, Vector3.right, Mathf.Abs(mousePosition.x - (Screen.width - edgeScrollSize)) / edgeScrollSize);

            if (mousePosition.y > Screen.height - edgeScrollSize)
                inputDir += Vector3.Lerp(Vector3.zero, Vector3.up, Mathf.Abs(mousePosition.y - (Screen.height - edgeScrollSize)) / edgeScrollSize);

            return inputDir * (edgeScrollSpeed * Time.deltaTime);
        }

        private Vector3 CameraMovementByKeys()
        {
            float horizontalMove = Input.GetAxis("Horizontal Camera");
            float verticalMove = Input.GetAxis("Vertical Camera");
            
            return new Vector3(horizontalMove, verticalMove, 0.0f) * (movementSpeed * Time.deltaTime);;
        }

        private void CameraScrolling()
        {
             float deltaScroll = 0.0f;

            // Because not conflicting
            if (Input.GetAxis("ScrollKeys Camera") != 0.0f)
            {
                float scroll = Input.GetAxis("ScrollKeys Camera");
                deltaScroll = scroll * scrollKeysSpeed * Time.deltaTime;
            }
            else if (Input.GetAxis("ScrollWheel Camera") != 0.0f)
            {
                float scroll = Input.GetAxis("ScrollWheel Camera");
                deltaScroll = scroll * scrollWheelSpeed * Time.deltaTime;
            }

            float orthographicSize = _playerCamera.orthographicSize;
            
            orthographicSize += deltaScroll;

            _playerCamera.orthographicSize = orthographicSize;
            _playerCamera.orthographicSize = Mathf.Clamp(orthographicSize, scrollSizeMin, scrollSizeMax);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void OnClicked()
        {
            if (Input.GetMouseButtonDown(0))
            {
                onClickTile.Invoke(_playerCamera.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }
}
