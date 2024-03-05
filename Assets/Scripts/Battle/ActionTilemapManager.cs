using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Battle
{
    [RequireComponent(typeof(Tilemap))]
    public class ActionTilemapManager : MonoBehaviour
    {
        private static readonly Vector3Int LeftTop = new(-6, -5); // origin
        private static readonly Vector3Int RightTop = new(5, -5);
        private static readonly Vector3Int LeftBottom = new(-6, 4);
        private static readonly Vector3Int RightBottom = new(5, 4);

        private const int RightDirectionX = 1; // to the right
        private const int DownDirectionY = 1; // to the down

        [SerializeField] private TileBase placeholderTile;
        [SerializeField] private TileBase greenTile;
        [SerializeField] private TileBase redTile;

        private Tilemap _tilemap;

        private bool _isPlacingUnits;
        private bool _isHost;

        private void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
        }

        public void StartPlacingUnits(bool isHost)
        {
            _isPlacingUnits = true;
            _isHost = isHost;
            HighlightAvailablePlacingSpots(isHost);
        }

        private void HighlightAvailablePlacingSpots(bool isHost)
        {
            Vector3Int leftTop;
            if (isHost)
                leftTop = LeftTop;
            else
            {
                leftTop = RightTop;
                leftTop.x -= RightDirectionX;
            }
            Vector3Int size = new(2, 10, 1);
            BoundsInt bounds = new BoundsInt(leftTop, size);
            TileBase[] newTiles = _tilemap
                .GetTilesBlock(bounds)
                .Select(tile => tile == placeholderTile ? placeholderTile : greenTile)
                .ToArray();
            _tilemap.SetTilesBlock(bounds, newTiles);
        }

        private void Update()
        {
            if (_isPlacingUnits) // and mouse button
            {
                
            }
        }
    }
}