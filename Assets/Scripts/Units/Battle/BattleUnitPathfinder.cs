using System;
using System.Collections.Generic;
using System.Linq;
using Battle;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Units.Battle
{
    public static class BattleUnitPathfinder
    {
        public static IDictionary<Vector3Int, List<Vector3Int>> GetReachablePositions(BattleUnit battleUnit)
        {
            // by speed only
            Vector3Int startPosition = battleUnit.CellPosition;
            
            ISet<Vector3Int> occupiedPositions =
                Object.FindObjectsOfType<BattleUnit>().Select(unit => unit.CellPosition).ToHashSet();
            
            ISet<Vector3Int> positionsReachableBySpeed = new HashSet<Vector3Int>();
            positionsReachableBySpeed.Add(startPosition);
            for (int i = -battleUnit.Speed; i <= battleUnit.Speed; i++)
            {
                for (int j = -(battleUnit.Speed - Math.Abs(i)); j <= battleUnit.Speed - Math.Abs(i); j++)
                {
                    Vector3Int relativePosition = new(i, j);
                    Vector3Int worldPosition = battleUnit.CellPosition + relativePosition;
                    if (GridManager.BattlefieldBounds.Contains(worldPosition) && !occupiedPositions.Contains(worldPosition))
                        positionsReachableBySpeed.Add(worldPosition);
                }
            }
            
            // Dijkstra to filter too long routes
            // Setup graph
            Dictionary<Vector3Int, List<Vector3Int>> graph = new();
            foreach (Vector3Int position in positionsReachableBySpeed)
            {
                Vector3Int[] possibleNeighbours =
                {
                    new(position.x-1, position.y),
                    new(position.x+1, position.y),
                    new(position.x, position.y-1),
                    new(position.x, position.y+1)
                };
                List<Vector3Int> neighbours = new();
                foreach (Vector3Int possibleNeighbour in possibleNeighbours)
                {
                    if (positionsReachableBySpeed.Contains(possibleNeighbour))
                        neighbours.Add(possibleNeighbour);
                }

                graph[position] = neighbours;
            }
            
            // Necessary data structures
            IDictionary<Vector3Int, int> shortestDistanceFromStart = new Dictionary<Vector3Int, int>(graph.Count);
            foreach (Vector3Int position in positionsReachableBySpeed)
                shortestDistanceFromStart[position] = int.MaxValue;
            shortestDistanceFromStart[startPosition] = 0;
            
            PriorityQueue<Vector3Int, int> unvisitedPositions = new PriorityQueue<Vector3Int, int>();
            unvisitedPositions.Enqueue(startPosition, shortestDistanceFromStart[startPosition]);
            
            ISet<Vector3Int> visitedPositions = new HashSet<Vector3Int>();

            IDictionary<Vector3Int, Vector3Int> previousPositions = new Dictionary<Vector3Int, Vector3Int>(graph.Count);

            // Actual loop
            do
            {
                Vector3Int position = unvisitedPositions.Dequeue();
                foreach (Vector3Int neighbour in graph[position])
                {
                    if (visitedPositions.Contains(neighbour)) continue;

                    int newDistance = shortestDistanceFromStart[position] + 1;
                    if (newDistance < shortestDistanceFromStart[neighbour])
                    {
                        shortestDistanceFromStart[neighbour] = newDistance;
                        previousPositions[neighbour] = position;
                    }
                    
                    unvisitedPositions.Enqueue(neighbour, shortestDistanceFromStart[neighbour]);
                }

                visitedPositions.Add(position);
            } while (unvisitedPositions.Count != 0);

            // Path construction
            IDictionary<Vector3Int, List<Vector3Int>> pathToPosition = new Dictionary<Vector3Int, List<Vector3Int>>();
            foreach (KeyValuePair<Vector3Int, Vector3Int> positions in previousPositions)
            {
                Vector3Int position = positions.Key;
                List<Vector3Int> path = new();
                void SearchUntilStartPosition(Vector3Int prev)
                {
                    if (prev == startPosition) return;
                    path.Add(prev);
                    SearchUntilStartPosition(previousPositions[prev]);
                }
                SearchUntilStartPosition(position);
                path.Reverse();
                pathToPosition[position] = path;
            }

            // Filtering of too long paths
            return pathToPosition.Where(position => position.Value.Count <= battleUnit.Speed)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}