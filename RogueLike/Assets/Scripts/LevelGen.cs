using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    GameObject masterNode;
    public GameObject nodeRef;
    GameObject[][] grid;
    public int seed;
    public int gridSizeX, gridSizeY, cellSizeX, cellSizeY;
    int nOfCells;

    public enum BorderPos { North, East, South, West };
    public BorderPos entrancePos, exitPos;

    private void Awake()
    {
        Random.InitState(seed);
        //Initialise grid
        grid = new GameObject[gridSizeX][];
        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = new GameObject[gridSizeY];
        }
        Generate();
    }

    private void Generate()
    {
        GenInit();
        GenPrim();
    }

    private void GenInit()
    {
        nOfCells = gridSizeX * gridSizeY;
        Debug.Log("Gen Initialising");
        Debug.Log("Number of Cells: " + nOfCells);

        //Create master node
        masterNode = new GameObject("Master Node");

        //Generate Grid
        int offsetX = 0;
        int idOffset = 0;
        for (int i = 0; i < grid.Length; i++)
        {
            int offsetY = 0;
            for (int j = 0; j < grid[i].Length; j++)
            {
                grid[i][j] = Instantiate(nodeRef, masterNode.transform);
                grid[i][j].transform.position = new Vector3((i + offsetX) * 10, 0, (j + offsetY) * 10);
                var tg = grid[i][j].GetComponent<TileGenerator>();
                tg.SetWidthHeight(cellSizeX, cellSizeY);
                tg.iD = idOffset;
                tg.arrayPosX = i;
                tg.arrayPosY = j;
                idOffset += 1;
                offsetY += cellSizeY - 1;
            }
            offsetX += cellSizeX - 1;
        }

        //Place Entrance and Exit
        PositionBorder(entrancePos, TileGenerator.RoomType.Entrance);
        PositionBorder(exitPos, TileGenerator.RoomType.Exit);
    }

    private void GenPrim()
    {

    }


    private void PositionBorder(BorderPos b, TileGenerator.RoomType rType)
    {
        switch (b)
        {
            case BorderPos.North:
                //Get Target Node randomly from appropriate axis
                var targetN = grid[Random.Range(1, grid.Length - 2)][grid[0].Length - 1].GetComponent<TileGenerator>();
                if (!targetN.isAssigned)
                {
                    targetN.SetRoomType(rType);
                }
                else
                {
                    while (targetN.isAssigned == true)
                    {
                        targetN = grid[Random.Range(1, grid.Length - 2)][grid[0].Length - 1].GetComponent<TileGenerator>();
                    }
                }
                targetN.SetRotation(-90);
                break;

            case BorderPos.East:
                var targetE = grid[grid[0].Length - 1][Random.Range(1, grid[0].Length - 2)].GetComponent<TileGenerator>();
                if (!targetE.isAssigned)
                {
                    targetE.SetRoomType(rType);
                }
                else
                {
                    while (targetE.isAssigned == true)
                    {
                        targetE = grid[grid[0].Length - 1][Random.Range(1, grid[0].Length - 2)].GetComponent<TileGenerator>();
                    }
                }
                targetE.SetRotation(-180);
                break;

            case BorderPos.South:
                var targetS = grid[Random.Range(1, grid.Length - 2)][0].GetComponent<TileGenerator>();
                if (!targetS.isAssigned)
                {
                    targetS.SetRoomType(rType);
                }
                else
                {
                    while (targetS.isAssigned == true)
                    {
                        targetS = grid[Random.Range(1, grid.Length - 2)][0].GetComponent<TileGenerator>();
                    }
                }
                targetS.SetRotation(-270);
                break;

            case BorderPos.West:
                var targetW = grid[0][Random.Range(1, grid[0].Length - 2)].GetComponent<TileGenerator>();
                if (!targetW.isAssigned)
                {
                    targetW.SetRoomType(rType);
                }
                else
                {
                    while (targetW.isAssigned == true)
                    {
                        targetW = grid[0][Random.Range(1, grid[0].Length - 2)].GetComponent<TileGenerator>();
                    }
                }
                targetW.SetRotation(0);
                break;
        }
    }

}
