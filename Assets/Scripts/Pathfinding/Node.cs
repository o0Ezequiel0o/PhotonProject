using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Node parent;

    public Vector2 position;
    public Vector2Int gridPosition;

    public int gCost;
    public int hCost;

    private int heapIndex;

    public Node(bool walkable, Vector3 position, Vector2Int gridPosition)
    {
        this.walkable = walkable;
        this.position = position;

        this.gridPosition = gridPosition;
    }

    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);

        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }
}