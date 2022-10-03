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
        [SerializeField] private float scrollSpeed = 10.0f;

        [SerializeField] private float scrollSizeMin = 3.0f;
        [SerializeField] private float scrollSizeMax = 8.0f;

        private bool _isDrag = false;

        public UnityEvent<Vector3> onClickTile;

        private Camera _playerCamera;

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
            CameraMovementByMouse();
            CameraMovementByKeyboard();

            CameraScrolling();
            OnClicked();
        }

        private void CameraMovementByMouse()
        {

        }

        private void CameraMovementByKeyboard()
        {
            float horizontalMove = Input.GetAxis("Horizontal Camera");
            float verticalMove = Input.GetAxis("Vertical Camera");

            Vector3 deltaMovement = new Vector3(horizontalMove, verticalMove, 0.0f) * (movementSpeed * Time.deltaTime);

            transform.position += deltaMovement;
        }

        private void CameraScrolling()
        {
            float scroll = Input.GetAxis("ScrollWheel Camera");
            float deltaScroll = scroll * scrollSpeed * Time.deltaTime;

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
