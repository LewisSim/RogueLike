using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pathfind : MonoBehaviour
{

    Grid GridReference;
    public Transform StartPosition;//starting position to pathfind from
    public Transform TargetPosition;//starting position to pathfind to
    public List<Node> FinalPath;

    private void Awake()
    {
        GridReference = GetComponent<Grid>();
        StartPosition = GameObject.FindGameObjectWithTag("Enemy").transform;
        TargetPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        FindPath(StartPosition.position, TargetPosition.position);

        StartPosition.transform.position += StartPosition.forward * Time.deltaTime;
    }

    public void FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        Node StartNode = GridReference.NodeFromWorldPoint(a_StartPos);//gets the node closest to the starting position
        Node TargetNode = GridReference.NodeFromWorldPoint(a_TargetPos);//gets the node closest to the target position

        List<Node> OpenList = new List<Node>();//list of nodes
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(StartNode);//add the starting node to the open list

        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].ihCost < CurrentNode.ihCost)//If the f cost of that object is less than or equal to the f cost of the current node
                {
                    CurrentNode = OpenList[i];//set the current node to that object
                }
            }
            OpenList.Remove(CurrentNode);//remove that from the open list
            ClosedList.Add(CurrentNode);//and add it to the closed list

            if (CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);//calculate the final path
            }

            foreach (Node NeighborNode in GridReference.GetNeighboringNodes(CurrentNode))
            {
                if (!NeighborNode.bIsWall || ClosedList.Contains(NeighborNode))
                {
                    continue;
                }
                int MoveCost = CurrentNode.igCost + GetManhattenDistance(CurrentNode, NeighborNode);

                if (MoveCost < NeighborNode.igCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.igCost = MoveCost;
                    NeighborNode.ihCost = GetManhattenDistance(NeighborNode, TargetNode);
                    NeighborNode.ParentNode = CurrentNode;

                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
            
        }
    }

    

    public void GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        FinalPath = new List<Node>();//list to hold the path sequentially 
        Node CurrentNode = a_EndNode;//node to store the current node being checked

        while (CurrentNode != a_StartingNode)
        { 

            FinalPath.Add(CurrentNode);//add that node to the final path
            CurrentNode = CurrentNode.ParentNode;//move onto its parent node
        }

        FinalPath.Reverse();//reverse the path

        GridReference.FinalPath = FinalPath;//set the final path

        LookTo();

    }

    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.iGridX - a_nodeB.iGridX);//x1-x2
        int iy = Mathf.Abs(a_nodeA.iGridY - a_nodeB.iGridY);//y1-y2

        return ix + iy;
    }

    void LookTo()
    {
        Node NextNode = FinalPath.First();

        StartPosition.LookAt(NextNode.vPosition);
    }
}