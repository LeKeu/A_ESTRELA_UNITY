using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public Transform seeker, target;
    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }
    void FindPath(Vector3 startPos, Vector3 endPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(endPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                RetraceStep(startNode, targetNode);
                return; 
            }

            foreach (Node vizinho in grid.GetVizinhos(currentNode))
            {
                if (!vizinho.walkable || closedSet.Contains(vizinho)) { continue; }

                int newMovCostToVizinho = currentNode.gCost + GetDistance(currentNode, vizinho); // custo atual + custo indo do atual p vizinho
                if (newMovCostToVizinho < vizinho.gCost || !(openSet.Contains(vizinho)))
                {
                    vizinho.gCost = newMovCostToVizinho;
                    vizinho.hCost = GetDistance(vizinho, targetNode);
                    vizinho.parent = currentNode;

                    if (!(openSet.Contains(vizinho))) { openSet.Add(vizinho); }
                }

            }

        }
    }

    void RetraceStep(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        grid.path = path;

    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY) {  return 14 * dstY + 10 * (dstX - dstY); }
        return 14 * dstX + 10 * (dstY - dstX);

    }
}
