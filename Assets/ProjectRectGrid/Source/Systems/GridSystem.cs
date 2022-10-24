using UnityEngine;

namespace GameGrid.Source.Systems
{
    public class GridSystem : MonoBehaviour
    {
        public static GridSystem Instance;

        public Vector2 cellSize = new Vector2(1.0f, 1.0f);
        public Vector2 cellOffset = new Vector2(0.5f, 0.5f);

        private void Awake()
        {
            Instance = this;
        }

        public Vector3Int ConvertToGridCoordinate(Vector3 pos)
        {
            int coordX = Mathf.FloorToInt(pos.x);
            int coordY = Mathf.FloorToInt(pos.y);
            int coordZ = Mathf.FloorToInt(pos.z);
            
            return new Vector3Int(coordX, coordY, coordZ);
        }
    }
}