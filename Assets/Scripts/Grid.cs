using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform seeker0;
    public Transform target0;
    public LayerMask UnwalkableMask;
    public Vector2 GridWorldSize;
    public float NodeRadius;    // ver quanto de espaço cada node vai ter
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {   // começar calculando, a partir do espaço do node e tamanho do grid, quandos nodes cabem nesse grid
        nodeDiameter = NodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(GridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(GridWorldSize.y / nodeDiameter);
    }

    private void Update()
    {
        CalculateGrid();
    }

    void CalculateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldLeftBottom = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y/2; // var com a posição do canto esuqerdo embaixo

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {   // pegando cada ponto ocupavel no mundo
                Vector3 worldPoint = worldLeftBottom + Vector3.right * (x * nodeDiameter + NodeRadius) + Vector3.forward * (y * nodeDiameter + NodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, NodeRadius, UnwalkableMask)); // checar se no ponto atual tem algo colidindo ou pode ser "andavel".
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            
            }
        }
    }

    public List<Node> GetVizinhos(Node node)
    {
        List<Node> vizinhos = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x==0 && y == 0) { continue; }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    vizinhos.Add(grid[checkX, checkY]);
                }
            }
        }
        return vizinhos;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {   // descobrir onde o node específico está na grid
        float percentX = (worldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x;
        float percentY = (worldPosition.z + GridWorldSize.y / 2) / GridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];

    }

    public List<Node> path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));

        if (grid != null)
        {
            Node seekerNode = NodeFromWorldPoint(seeker0.position);
            Node targetNode = NodeFromWorldPoint(target0.position);
            foreach (Node node in grid)
            {
                
                Gizmos.color = (node.walkable)? Color.white : Color.red;

                if (path != null)
                {
                    if (path.Contains(node)) { Gizmos.color = Color.black; }
                }
                if (seekerNode == node) { Gizmos.color = Color.cyan; } // se o player estiver neste node específico, pinta de azul

                if (targetNode == node) { Gizmos.color = Color.green; }
                Gizmos.DrawCube(node.WorldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
