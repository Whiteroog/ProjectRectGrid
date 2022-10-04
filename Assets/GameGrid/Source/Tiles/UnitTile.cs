using System;
using System.Collections;
using GameGrid.Source.Managers;
using UnityEngine;

namespace GameGrid.Source.Tiles
{
    public class UnitTile : BaseRectTile
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

        public override Vector3Int Coordinate
        {
            set
            {
                GroundTilesManager groundTilesManager = GroundTilesManager.Instance;
            
                groundTilesManager.FindTile(base.Coordinate).OccupiedUnit = null;
                base.Coordinate = value;
                groundTilesManager.FindTile(value).OccupiedUnit = this;
            }
        }

        public int GetMovementPoints() => movementPoints;

        public IEnumerator Move(Vector3Int[] pathway, int spentCost, Action onEndMove)
        {
            movementPoints -= spentCost;

            _unitAnimator.SetBool("IsMove", true);

            for (int i = 1; i < pathway.Length; i++)
            {
                Vector3 start = pathway[i - 1];
                Vector3 end = pathway[i];

                DirectionRenderSprite(start, end);

                for (float t = 0; t < 1; t += Time.deltaTime * movementSpeed)
                {
                    transform.localPosition = Vector3.Lerp(start, end, t);
                    yield return null;
                }
            }

            _unitAnimator.SetBool("IsMove", false);
            
            Coordinate = pathway[^1]; // end array
            
            onEndMove?.Invoke();

            void DirectionRenderSprite(Vector3 start, Vector3 end)
            {              
                float side = Vector3.Dot((end - start).normalized, Vector3.right);
                
                if (!Mathf.Approximately(side, 0.0f))
                    _spriteRenderer.flipX = side < 0.0f;
            }
        }

    }
}