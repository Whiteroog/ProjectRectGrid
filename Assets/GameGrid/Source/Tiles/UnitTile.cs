using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameGrid.Source.Tiles
{
    public class UnitTile : BaseSquareTile
    {
        public event Action<UnitTile, bool> OnAnimating;

        [SerializeField] private float movementSpeed = 2.0f;
        [SerializeField] private Animator unitAnimator;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void MoveUnit(Vector3Int newCoordinate)
        {
            OnAnimating?.Invoke(this, true);
            StartCoroutine(MovementUnit(newCoordinate));
        }

        public IEnumerator MovementUnit(Vector3Int targetCoordinate)
        {
            Vector3 startPosition = Coordinate;
            Vector3 endPosition = targetCoordinate;

            Vector3 direction = endPosition - startPosition;
            float scaledSpeed = direction.magnitude / movementSpeed;
            
            print("Start animation");
            
            float side = Vector3.Dot(direction.normalized, Vector3.right);
            if(!Mathf.Approximately(side, 0.0f))
                spriteRenderer.flipX = side < 0.0f;
            
            unitAnimator.SetBool("IsMove", true);
            
            for (float t = 0; t < 1; t += Time.deltaTime / scaledSpeed)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                yield return null;
            }
            Coordinate = targetCoordinate;
            unitAnimator.SetBool("IsMove", false);
            
            OnAnimating?.Invoke(this, false);
        }
    }
}