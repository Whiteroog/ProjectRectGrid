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

        public IEnumerator MoveAnimate(Vector3Int start, Vector3Int end)
        {
            _unitAnimator.SetBool("IsMove", true);

            Vector3 direction = end - start;
            float side = Vector3.Dot(direction.normalized, Vector3.right);
            if (!Mathf.Approximately(side, 0.0f))
                _spriteRenderer.flipX = side < 0.0f;

            for (float t = 0; t < 1; t += Time.deltaTime * movementSpeed)
            {
                transform.position = Vector3.Lerp(start, end, t);
                yield return null;
            }

            _unitAnimator.SetBool("IsMove", false);
        }

        public int GetMovementPoints() => movementPoints;
    }
}