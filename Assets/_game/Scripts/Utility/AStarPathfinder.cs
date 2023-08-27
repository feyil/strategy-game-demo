using System.Collections.Generic;
using _game.Scripts.GridComponents;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts.Utility
{
    // NOTE: It is generated using Chat GPT and modified to suit to the usecase.
    public class AStarPathfinder
    {
        private Node[,] _grid;
        private Vector2Int _gridSize;
        private List<Node> _openList;
        private HashSet<Node> _closedSet;

        private readonly GridManager _gridManager;
        private readonly Vector2 _startPoint;
        private readonly Vector2 _endPoint;

        public static GridCell[] GetPath(GridManager gridManager, Vector2 startPoint, Vector2 endPoint)
        {
            var finder = new AStarPathfinder(gridManager, startPoint, endPoint);
            return finder.Search();
        }

        private AStarPathfinder(GridManager gridManager, Vector2 startPoint, Vector2 endPoint)
        {
            _gridManager = gridManager;
            _startPoint = startPoint;
            _endPoint = endPoint;
            _gridSize = gridManager.GetDimension();
        }

        [Button]
        private GridCell[] Search()
        {
            _grid = new Node[_gridSize.x, _gridSize.y];
            InitializeGrid();

            var path = FindPath();
            var gridCellPath = new GridCell[path.Count];
            for (var index = 0; index < path.Count; index++)
            {
                var node = path[index];
                var gridCell = _gridManager.GetCell(new Vector2(node.gridX, node.gridY));
                gridCellPath[index] = gridCell;
            }

            return gridCellPath;
        }

        private void InitializeGrid()
        {
            // Create nodes for each cell in the grid
            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    Vector3 cellPosition = new Vector3(x, 0, y);
                    var isObstacle = _gridManager.GetCell(new Vector2(x, y)).IsFilled();
                    _grid[x, y] = new Node(isObstacle, cellPosition, x, y);
                }
            }
        }

        private List<Node> FindPath()
        {
            _openList = new List<Node>();
            _closedSet = new HashSet<Node>();

            Node startNode = _grid[(int)_startPoint.x, (int)_startPoint.y];
            Node endNode = _grid[(int)_endPoint.x, (int)_endPoint.y];

            _openList.Add(startNode);

            while (_openList.Count > 0)
            {
                Node currentNode = _openList[0];
                for (int i = 1; i < _openList.Count; i++)
                {
                    if (_openList[i].fCost < currentNode.fCost ||
                        (_openList[i].fCost == currentNode.fCost && _openList[i].hCost < currentNode.hCost))
                    {
                        currentNode = _openList[i];
                    }
                }

                _openList.Remove(currentNode);
                _closedSet.Add(currentNode);

                if (currentNode == endNode)
                {
                    // Found the path, reconstruct and show it
                    return RetracePath(startNode, endNode);
                }

                foreach (Node neighbor in GetNeighboringNodes(currentNode))
                {
                    if (neighbor.isObstacle || _closedSet.Contains(neighbor))
                        continue;

                    int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                    if (newMovementCostToNeighbor < neighbor.gCost || !_openList.Contains(neighbor))
                    {
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, endNode);
                        neighbor.parent = currentNode;

                        if (!_openList.Contains(neighbor))
                            _openList.Add(neighbor);
                    }
                }
            }

            return new List<Node>();
        }

        private List<Node> GetNeighboringNodes(Node node)
        {
            List<Node> neighbors = new List<Node>();

            for (int xOffset = -1; xOffset <= 1; xOffset++)
            {
                for (int yOffset = -1; yOffset <= 1; yOffset++)
                {
                    if (xOffset == 0 && yOffset == 0)
                        continue;

                    int checkX = node.gridX + xOffset;
                    int checkY = node.gridY + yOffset;

                    if (checkX >= 0 && checkX < _gridSize.x && checkY >= 0 && checkY < _gridSize.y)
                    {
                        neighbors.Add(_grid[checkX, checkY]);
                    }
                }
            }

            return neighbors;
        }

        private List<Node> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();

            // Now 'path' contains the shortest path from start to end
            // You can do whatever you want with the path, like moving a character along it
            return path;
        }

        private Node GetNodeFromPosition(Vector3 position)
        {
            int x = Mathf.FloorToInt(position.x);
            int y = Mathf.FloorToInt(position.z);
            return _grid[x, y];
        }

        private int GetDistance(Node nodeA, Node nodeB)
        {
            int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
            return distX + distY;
        }

        private class Node
        {
            public bool isObstacle;
            public Vector3 position;
            public int gridX;
            public int gridY;
            public int gCost;
            public int hCost;
            public Node parent;

            public int fCost => gCost + hCost;

            public Node(bool isObstacle, Vector3 position, int gridX, int gridY)
            {
                this.isObstacle = isObstacle;
                this.position = position;
                this.gridX = gridX;
                this.gridY = gridY;
            }
        }
    }
}