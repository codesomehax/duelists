using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Battle
{
    [RequireComponent(typeof(Grid))]
    public class GridManager : MonoBehaviour
    {
        private const int BattlefieldWidth = 10;
        private const int BattlefieldLength = 12;
        private static readonly Vector3Int BattlefieldSize = new(BattlefieldLength, BattlefieldWidth, 1); 
        
        private static readonly Vector3Int LeftTop = new(0, 0); // origin
        private static readonly Vector3Int RightTop = new(11, 0);
        private static readonly Vector3Int LeftBottom = new(0, 9);
        private static readonly Vector3Int RightBottom = new(11, 9);
        private const int RightDirectionX = 1; // to the right
        private const int DownDirectionY = 1; // to the down

        [Header("Tiles")] 
        [SerializeField] private ActionTile3D placeholderTile;
        [SerializeField] private ActionTile3D greenTile;
        [SerializeField] private ActionTile3D redTile;

        private Grid _grid;

        private readonly Dictionary<Vector3Int, ActionTile3D> _actionTilemap = new(BattlefieldLength*BattlefieldWidth);

        private void Awake()
        {
            _grid = GetComponent<Grid>();
            
            PlacePlaceholderTiles();
            HighlightAvailablePlacingSpots(false);
        }

        private void PlacePlaceholderTiles()
        {
            BoundsInt bounds = new(LeftTop, BattlefieldSize);
            foreach (Vector3Int cellPosition in bounds.allPositionsWithin)
                SetTile(cellPosition, placeholderTile);
        }

        private void SetTile(Vector3Int cellPosition, ActionTile3D tile)
        {
            if (_actionTilemap.TryGetValue(cellPosition, out ActionTile3D existingTile))
            {
                Destroy(existingTile.gameObject);
                _actionTilemap[cellPosition] = null;
            }
            
            Vector3 worldPosition = _grid.CellToWorld(cellPosition);
            worldPosition.x += 0.5f;
            worldPosition.z += 0.5f;
            ActionTile3D newTile = Instantiate(tile, worldPosition, tile.transform.rotation);
            _actionTilemap[cellPosition] = newTile;
        }

        private void HighlightAvailablePlacingSpots(bool isHost)
        {
            BoundsInt availablePlacingSpots = GetAvailablePlacingSpots(isHost);
            foreach (Vector3Int cellPosition in availablePlacingSpots.allPositionsWithin)
            {
                ActionTile3D currentTile = _actionTilemap[cellPosition];
                if (currentTile.Equals(placeholderTile)) 
                    SetTile(cellPosition, greenTile);
            }
        }

        private static BoundsInt GetAvailablePlacingSpots(bool isHost)
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
            return new BoundsInt(leftTop, size);
        }
    }
}