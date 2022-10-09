using System;
using System.Collections;
using GameGrid.Source.Managers;
using UnityEngine;

namespace GameGrid.Source.Tiles
{
    public class UnitTile : BaseRectTile
    {
        [SerializeField] private int defaultMovementPoints = 5;
        
        [SerializeField] private float movementSpeed = 2.0f;
        
        private Animator _unitAnimator;
        private SpriteRenderer _spriteRenderer;

        private int _movementPoints = 0;

        private void Awake()
        {
            _unitAnimator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        private void Start()
        {
            ResetMovementPoints();
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

        public void ResetMovementPoints() => _movementPoints = defaultMovementPoints;
        public int GetMovementPoints() => _movementPoints;

        public void Move(GroundTile[] pathway, int spentCost)
        {
            StartCoroutine(MoveAnimation(pathway, () => MoveTo(pathway[^1], spentCost)));
        }
        
        private void MoveTo(GroundTile targetTile, int spentCost)
        {
            _movementPoints -= spentCost;
            Coordinate = targetTile.Coordinate;
            
            SelectManager.Instance.IsProcessing = false;
        }

        private IEnumerator MoveAnimation(GroundTile[] pathway, Action endAnimation)
        {
            _unitAnimator.SetBool("IsMoving", true);

            for (int i = 1; i < pathway.Length; i++)
            {
                Vector3 start = pathway[i - 1].transform.localPosition;
                Vector3 end = pathway[i].transform.localPosition;

                float side = Vector3.Dot((end - start).normalized, Vector3.right);
                
                if (!Mathf.Approximately(side, 0.0f))
                    _spriteRenderer.flipX = side < 0.0f;

                for (float t = 0; t < 1; t += Time.deltaTime * movementSpeed)
                {
                    transform.localPosition = Vector3.Lerp(start, end, t);
                    yield return null;
                }

                transform.localPosition = end;
            }

            _unitAnimator.SetBool("IsMoving", false);

            endAnimation?.Invoke();
        }
    }
}