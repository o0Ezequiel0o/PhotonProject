using System.Collections.Generic;
using UnityEngine;

public class Graph
{	
	private LayerMask unwalkableLayer;
	private int erosionIterations;
	
	private float nodeRadius;
	private float nodeDiameter;

	private float entityRadius;

	private Vector3 localPosition;

	private Vector2 gridWorldSize;
	private Vector2Int gridSize;

	private Node[,] grid;

	public int MaxSize
	{
		get
		{
			return gridSize.x * gridSize.y;
		}
	}

	public enum Name
    {
        Graph1,
        Graph2,
        Graph3,
        Graph4,
        Graph6,
        Graph7,
        Graph8
    }

	public Graph(Vector3 localPosition, LayerMask unwalkableLayer, int erosionIterations, Vector2 gridWorldSize, float nodeRadius, float entityRadius)
	{
		this.localPosition = localPosition;

		this.unwalkableLayer = unwalkableLayer;
		this.erosionIterations = erosionIterations;

		this.gridWorldSize = gridWorldSize;
		this.nodeRadius = nodeRadius;

		this.entityRadius = entityRadius;

		nodeDiameter = nodeRadius * 2;

		gridSize.x = Mathf.Max(0, Mathf.RoundToInt(gridWorldSize.x / nodeDiameter));
		gridSize.y = Mathf.Max(0, Mathf.RoundToInt(gridWorldSize.y / nodeDiameter));

		Initialize();
	}

	public static Graph GetEmptyGraph()
	{
		return new Graph(Vector3.zero, LayerMask.NameToLayer("Default"), 0, Vector2.zero, 0f, 0f);
	}

	public List<Node> GetNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (TryGetNeighbour(node, x, y, out Node neighbour))
				{
					neighbours.Add(neighbour);
				}
			}
		}

		return neighbours;
	}

	public bool TryGetNeighbour(Node node, int directionX, int directionY, out Node neighbour)
	{
		neighbour = GetNeighbour(node, directionX, directionY);

		return neighbour != null;
	}

	public bool TryGetNeighbour(Node node, Vector2Int direction, out Node neighbour)
	{
		return TryGetNeighbour(node, direction.x, direction.y, out neighbour);
	}

	public Node GetNeighbour(Node node, int directionX, int directionY)
	{
		if (directionX != 0 || directionY != 0)
		{
			int checkX = node.gridPosition.x + directionX;
			int checkY = node.gridPosition.y + directionY;

			if (NodeInBounds(checkX, checkY))
			{
				return grid[checkX, checkY];
			}
		}

		return null;
	}

	public Node GetNeighbour(Node node, Vector2Int direction)
	{
		return GetNeighbour(node, direction.x, direction.y);
	}

	public Node WorldPositionToNode(Vector3 nodeWorldPosition)
	{
		if (MaxSize == 0) return null;

		Vector2 rawGridPosition = Vector2.zero;
		Vector2Int gridPosition = Vector2Int.zero;

		rawGridPosition.x = (nodeWorldPosition.x - localPosition.x + gridWorldSize.x * 0.5f) / nodeDiameter;
		rawGridPosition.y = (nodeWorldPosition.y - localPosition.y + gridWorldSize.y * 0.5f) / nodeDiameter;

		gridPosition.x = Mathf.FloorToInt(Mathf.Clamp(rawGridPosition.x, 0, gridSize.x - 1));
		gridPosition.y = Mathf.FloorToInt(Mathf.Clamp(rawGridPosition.y, 0, gridSize.y - 1));

		return grid[gridPosition.x, gridPosition.y];
	}

	public void DrawGizmos()
	{
		Gizmos.DrawWireCube(localPosition, new Vector3(gridWorldSize.x, gridWorldSize.y, 0f));

		foreach (Node node in grid)
		{
			Gizmos.color = Color.white;
			Gizmos.DrawWireCube(node.position, Vector3.one * nodeDiameter);

			if (!node.walkable)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(node.position, 0.2f);
			}
		}
	}

	void Initialize()
	{
		CreateGrid();
		CalculateErosion();
	}

	void CreateGrid()
	{
		grid = new Node[gridSize.x, gridSize.y];

		Vector3 worldBottomLeft = Vector3.zero;
		Vector3 worldPoint = Vector3.zero;

		worldBottomLeft.x = localPosition.x - gridWorldSize.x / 2;
		worldBottomLeft.y = localPosition.y - gridWorldSize.y / 2;

		for (int x = 0; x < gridSize.x; x++)
		{
			worldPoint.x = worldBottomLeft.x + x * nodeDiameter + nodeRadius;

			for (int y = 0; y < gridSize.y; y++)
			{
				worldPoint.y = worldBottomLeft.y + y * nodeDiameter + nodeRadius;
				bool walkable = !Physics2D.OverlapCircle(worldPoint, entityRadius, unwalkableLayer);

				grid[x, y] = new Node(walkable, worldPoint, new Vector2Int(x, y));
			}
		}
	}

	void CalculateErosion()
	{
		for (int i = 0; i < erosionIterations; i++)
		{
			List<Node> nodesToUpdate = new List<Node>();

			foreach (Node node in grid)
			{
				if (node.walkable)
				{
					continue;
				}

				foreach (Node neighbour in GetNeighbours(grid[node.gridPosition.x, node.gridPosition.y]))
				{
					if (neighbour.walkable)
					{
						nodesToUpdate.Add(neighbour);
					}
				}
			}

			foreach (Node nodeToUpdate in nodesToUpdate)
			{
				nodeToUpdate.walkable = false;
			}
		}
	}

	bool NodeInBounds(int x, int y)
	{
		if (x >= 0 && x < gridSize.x && y >= 0 && y < gridSize.y)
		{
			return true;
		}

		return false;
	}
}