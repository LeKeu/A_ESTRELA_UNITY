using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public Transform seeker0;
    public Transform target0;
    public LayerMask UnwalkableMask;
    public Vector2 GridWorldSize;
    public float NodeRadius;    // ver quanto de espaço cada node vai ter
    Node[,] grid;

    [SerializeField] Canvas canvas;

    public Vector3 pos;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    [SerializeField] DragObject PdragObject;
    [SerializeField] DragObject TdragObject;

    private void Start()
    {   // começar calculando, a partir do espaço do node e tamanho do grid, quandos nodes cabem nesse grid
        nodeDiameter = NodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(GridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(GridWorldSize.y / nodeDiameter);

        CalculateGrid();
        DesenharGrid();
    }

    private void Update()
    {
        //DesenharGrid();
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
    public List<Node> vizinhos;

    public void DesenharGrid()
    {

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Area");
        if (gos != null)
        {
            foreach (GameObject go in gos)
                Destroy(go);
        }

        if (grid != null)
        {
            Node seekerNode = NodeFromWorldPoint(seeker0.position);
            Node targetNode = NodeFromWorldPoint(target0.position);

            foreach (Node node in grid)
            {
                Canvas c = Instantiate(canvas, node.WorldPosition, Quaternion.Euler(90, 0, 0));
                c.GetComponentInChildren<Image>().color = (node.walkable) ? Color.white : Color.red;
                //c.GetComponent<RectTransform>().localScale = new Vector3(nodeDiameter/10, nodeDiameter / 10, nodeDiameter / 10);
                //c.transform.localScale = new Vector3(nodeDiameter / 10, nodeDiameter / 10, nodeDiameter / 10);

                if (path != null)
                {
                    //Debug.Log("primeiro");
                    if (vizinhos.Contains(node)) { c.GetComponentInChildren<Image>().color = Color.yellow; }
                    if (path.Contains(node))
                    {
                        Debug.Log("preto");
                        c.GetComponentInChildren<Image>().color = Color.black;
                        //Instantiate(canvas, transform.position, Quaternion.identity);
                    }

                }
                if (seekerNode == node) { c.GetComponentInChildren<Image>().color = Color.cyan; } // se o player estiver neste node específico, pinta de azul

                if (targetNode == node) { c.GetComponentInChildren<Image>().color = Color.green; }

                int aux = c.transform.childCount;
                List<string> costList = new List<string>() { node.fCost.ToString(), node.gCost.ToString(), node.hCost.ToString() };

                for (int i = 1; i < aux; i++)
                {
                    c.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = costList[i - 1];
                }
            }
        }
    }


}
