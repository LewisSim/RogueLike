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
        //Get list of all tiles
        List<TileGenerator> allTiles = new List<TileGenerator>();
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                allTiles.Add(grid[i][j].GetComponent<TileGenerator>());
            }
        }

        //Get list of occupied cells
        List<TileGenerator> occupied = new List<TileGenerator>();
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                if (grid[i][j].GetComponent<TileGenerator>().isAssigned)
                {
                    occupied.Add(grid[i][j].GetComponent<TileGenerator>());
                }
            }
        }
        Debug.Log(occupied.Count);

        //Get initial list of frontier cells
        List<TileGenerator> frontier = new List<TileGenerator>();
        for (int i = 0; i < occupied.Count; i++)
        {
            Debug.Log("start of loop");
            if (occupied[i].connectionPoints[0])
            {
                var newFrontierMember = GetAdjacentTile(occupied[i], 0);
                newFrontierMember.parent = occupied[i];
                newFrontierMember.connectionPoints[2] = true;
                frontier.Add(newFrontierMember);
            }
            if (occupied[i].connectionPoints[1])
            {
                var newFrontierMember = GetAdjacentTile(occupied[i], 1);
                newFrontierMember.parent = occupied[i];
                newFrontierMember.connectionPoints[3] = true;
                frontier.Add(newFrontierMember);
            }
            if (occupied[i].connectionPoints[2])
            {
                var newFrontierMember = GetAdjacentTile(occupied[i], 2);
                newFrontierMember.parent = occupied[i];
                newFrontierMember.connectionPoints[0] = true;
                frontier.Add(newFrontierMember);
            }
            if (occupied[i].connectionPoints[3])
            {
                var newFrontierMember = GetAdjacentTile(occupied[i], 3);
                newFrontierMember.parent = occupied[i];
                newFrontierMember.connectionPoints[1] = true;
                frontier.Add(newFrontierMember);
            }
            Debug.Log("end of loop");
        }
        for (int i = 0; i < frontier.Count; i++)
        {
            Debug.Log("Frontier cell " + i + "= cell id: " + frontier[i].iD);
        }

        //Step through randomly selecting frontier cells to connect to
        FrontierPop(frontier, occupied);

        while (occupied.Count < nOfCells)
        {
            //Add to frontier new adjacent cells
            for (int i = 0; i < occupied.Count; i++)
            {
                var currentOCDirections = occupied[i].GetNonConnectedDirections();
                for (int j = 0; j < currentOCDirections.Count; j++)
                {
                    switch (currentOCDirections[j])
                    {
                        case 0:
                            var newFrontierMemberR = GetAdjacentTile(occupied[i], 0);
                            if (newFrontierMemberR != null && !newFrontierMemberR.isAssigned)
                            {
                                newFrontierMemberR.parent = occupied[i];
                                //newFrontierMemberR.connectionPoints[2] = true;
                                frontier.Add(newFrontierMemberR);
                            }
                            break;
                        case 1:
                            var newFrontierMemberD = GetAdjacentTile(occupied[i], 1);
                            if (newFrontierMemberD != null && !newFrontierMemberD.isAssigned)
                            {
                                newFrontierMemberD.parent = occupied[i];
                                //newFrontierMemberD.connectionPoints[3] = true;
                                frontier.Add(newFrontierMemberD);
                            }
                            break;
                        case 2:
                            var newFrontierMemberL = GetAdjacentTile(occupied[i], 2);
                            if (newFrontierMemberL != null && !newFrontierMemberL.isAssigned)
                            {
                                newFrontierMemberL.parent = occupied[i];
                                //newFrontierMemberL.connectionPoints[0] = true;
                                frontier.Add(newFrontierMemberL);
                            }
                            break;
                        case 3:
                            var newFrontierMemberU = GetAdjacentTile(occupied[i], 3);
                            if (newFrontierMemberU != null && !newFrontierMemberU.isAssigned)
                            {
                                newFrontierMemberU.parent = occupied[i];
                                //newFrontierMemberU.connectionPoints[1] = true;
                                frontier.Add(newFrontierMemberU);
                            }
                            break;
                    }
                }
                //if (currentOCDirections.Count == 0)
                //{
                //    break;
                //}
            }
            //Remove any frontier members that are already claimed
            frontier.RemoveAll(item => item.isAssigned);

            Debug.Log("Number of frontier cells: " + frontier.Count);
            if (frontier.Count != 0)
            {
                FrontierPop(frontier, occupied);
            }
        }

        //Consolidate all tiles
        for (int i = 0; i < allTiles.Count; i++)
        {
            if (allTiles[i].unchangable == false)
            {
                allTiles[i].ConsolidateTile();
            }
        }
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
                targetN.SetRotation(90);
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
                targetE.SetRotation(180);
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
                targetS.SetRotation(270);
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


    private void FrontierPop(List<TileGenerator> f, List<TileGenerator> o)
    {
        //Pick random frontier cell to pop
        int ran = Random.Range(0, f.Count);
        //Add to Occupied list
        f[ran].SetRoomType(TileGenerator.RoomType.XConnect);
        o.Add(f[ran]);
        //Check adjacent spots for existing cells and add connection to one
        for (int i = 0; i < 4; i++)
        {
            var nextTo = GetAdjacentTile(f[ran], i);
            if (nextTo != null)
            {
                switch (i)
                {
                    case 0:
                        if (nextTo == f[ran].parent)
                        {
                            //Create mutual connection between parent and child
                            f[ran].connectionPoints[0] = true;
                            f[ran].parent.connectionPoints[2] = true;
                        }
                        break;
                    case 1:
                        if (nextTo == f[ran].parent)
                        {
                            //Create mutual connection between parent and child
                            f[ran].connectionPoints[1] = true;
                            f[ran].parent.connectionPoints[3] = true;
                        }
                        break;
                    case 2:
                        if (nextTo == f[ran].parent)
                        {
                            //Create mutual connection between parent and child
                            f[ran].connectionPoints[2] = true;
                            f[ran].parent.connectionPoints[0] = true;
                        }
                        break;
                    case 3:
                        if (nextTo == f[ran].parent)
                        {
                            //Create mutual connection between parent and child
                            f[ran].connectionPoints[3] = true;
                            f[ran].parent.connectionPoints[1] = true;
                        }
                        break;
                }
            }

        }

        //Remove from frontier list
        f.Remove(f[ran]);

    }

    private TileGenerator GetAdjacentTile(TileGenerator oc, int direction)
    {
        TileGenerator foundTile = new TileGenerator();
        switch (direction)
        {
            case 0:
                //If tile is on edge don't do anything
                if (oc.arrayPosX != grid.Length - 1)
                {
                    foundTile = grid[oc.arrayPosX + 1][oc.arrayPosY].GetComponent<TileGenerator>();
                }
                else
                {
                    foundTile = null;
                }
                break;
            case 1:
                if (oc.arrayPosY != 0)
                {
                    foundTile = grid[oc.arrayPosX][oc.arrayPosY - 1].GetComponent<TileGenerator>();
                }
                else
                {
                    foundTile = null;
                }
                break;
            case 2:
                if (oc.arrayPosX != 0)
                {
                    foundTile = grid[oc.arrayPosX - 1][oc.arrayPosY].GetComponent<TileGenerator>();
                }
                else
                {
                    foundTile = null;
                }
                break;
            case 3:
                if (oc.arrayPosY != grid[0].Length - 1)
                {
                    foundTile = grid[oc.arrayPosX][oc.arrayPosY + 1].GetComponent<TileGenerator>();
                }
                else
                {
                    foundTile = null;
                }
                break;
        }
        return foundTile;
    }
}
