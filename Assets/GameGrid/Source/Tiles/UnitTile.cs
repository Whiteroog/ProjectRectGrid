using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameGrid.Source.Tiles
{
    public class UnitTile : BaseSquareTile
    {
        [SerializeField] private int movementPoints = 5;
        [SerializeField] private float movementSpeed = 2.0f;
        
        private Animator _unitAnimator;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _unitAnimator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public IEnumerator MovementUnitNotTile(Vector3Int targetCoordinate, Action onEndingMovement)
        {
            Vector3 startPosition = Coordinate;
            Vector3 endPosition = targetCoordinate;

            Vector3 direction = endPosition - startPosition;
            float scaledSpeed = direction.magnitude / movementSpeed;

            float side = Vector3.Dot(direction.normalized, Vector3.right);
            if(!Mathf.Approximately(side, 0.0f))
                _spriteRenderer.flipX = side < 0.0f;
            
            _unitAnimator.SetBool("IsMove", true);
            
            for (float t = 0; t < 1; t += Time.deltaTime / scaledSpeed)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }
            Coordinate = targetCoordinate;
            _unitAnimator.SetBool("IsMove", false);
            
            onEndingMovement?.Invoke();
        }
        
        public IEnumerator MovementUnit(List<Vector3Int> way , Action onEndingMovement)
        {
            _unitAnimator.SetBool("IsMove", true);

            for (int i = 1; i < way.Count; i++)
            {
                Vector3Int start = way[i - 1];
                Vector3Int end = way[i];

                DirectionRenderSprite(start, end);
                
                for (float t = 0; t < 1; t += Time.deltaTime / movementSpeed)
                {
                    transform.position = Vector3.Lerp(start, end, t);
                    yield return null;
                }

                Coordinate = end;
            }
            
            _unitAnimator.SetBool("IsMove", false);
            onEndingMovement?.Invoke();

            void DirectionRenderSprite(Vector3Int start, Vector3Int end)
            {
                Vector3 direction = end - start;
                float side = Vector3.Dot(direction.normalized, Vector3.right);
                if(!Mathf.Approximately(side, 0.0f))
                    _spriteRenderer.flipX = side < 0.0f;
            }
        }

        public int GetMovementPoints() => movementPoints;
    }
}