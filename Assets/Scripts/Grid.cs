using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform enemy;
    public Vector2 gridSize;
    public float nodeSize;
    public LayerMask obstacleMask;
    Node[,] aGrid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    bool displayGizmos = true;






    // Start is called before the first frame update
    void Awake()
    {
        nodeDiameter = nodeSize * 2;//current 1
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
        Create();
    }


    void Create()//create nodes for grid
    {
        aGrid = new Node[gridSizeX, gridSizeY];

        Vector3 swPos = transform.position - (Vector3.right * gridSize.x / 2) - (Vector3.forward * gridSize.y / 2);  //X + Z

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = swPos + Vector3.right * (x * nodeDiameter + nodeSize) + Vector3.forward * (y * nodeDiameter + nodeSize);// recorde pos for each node starting bottom left of grid
                bool traversable = !(Physics.CheckSphere(worldPoint, nodeSize, obstacleMask));//check if pos is already occupied
                aGrid[x, y] = new Node(traversable, worldPoint, x, y);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }



    public Node nodeFromWorldPos(Vector3 worldPos)//find current node
    {
        float precentX = (worldPos.x + gridSize.x / 2) / gridSize.x;
        Vector3 swPos = transform.position - (Vector3.right * (gridSizeX / 2)) - (Vector3.forward * (gridSizeY / 2));  //finds bottom left pos of grid



        float precentY = (worldPos.z + gridSize.y / 2) / gridSize.y;



        precentX = Mathf.Clamp01(precentX);//Prevent issues if grid is left
        precentY = Mathf.Clamp01(precentY);

        int x = Mathf.FloorToInt((worldPos - swPos).x/* nodeSize*/);
        int y = Mathf.FloorToInt((worldPos - swPos).z /* nodeSize*/);


        return aGrid[x, y];
    }


    public List<Node> path;//only for visually test pathfinding

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));//X == Z in 3D


        if (aGrid != null)//testing
        {
            //node enemyNode = nodeFromPos(enemy.position);// passes enemy pos 
            //Node enemyNode = nodeFromWorldPos(enemy.position);

            if (displayGizmos == true)
            {
                foreach (Node n in aGrid)
                {
                    Gizmos.color = (n.traversable) ? Color.white : Color.red;


                    if (path != null && path.Contains(n))
                    {
                        Gizmos.color = Color.yellow;
                    }


                    /*if (n == enemyNode)
                    {
                        //Gizmos.color = Color.blue;
                    }*/

                    Gizmos.DrawCube(n.location, Vector3.one * (nodeDiameter - 0.1f));
                    Gizmos.color = (n.traversable) ? Color.white : Color.red;
                }
            }
        }


    }


    public List<Node> getBorderNodes(Node Node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)//3 by 3 grid
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)//ignores curren node/centre
                {
                    continue;
                }
                int checkX = Node.gridX + x;
                int checkY = Node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)//if neibouring
                {
                    neighbours.Add(aGrid[checkX, checkY]);


                }
            }
        }
        //Debug.Log("working");
        return neighbours;
    }

}

