using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    GameObject masterNode;
    public GameObject nodeRef, bossRoomRef;
    GameObject[][] grid;
    public int seed;
    public int gridSizeX, gridSizeY, cellSizeX, cellSizeY;
    public int numberOfRI;
    int nOfCells;

    public enum BorderPos { North, East, South, West };
    public BorderPos entrancePos, exitPos;

    public float progress = 0f;
    bool isGenerating = false;
    public string loadingText = "";

    public bool loadBossRoom;

    public GameObject gm_manager;

    private void Awake()
    {
        StartCoroutine(Begin());
    }

    private void Update()
    {
        if (isGenerating)
        {
            print("PROGRESS: " + progress);
            print(loadingText);
        }
        if (progress == 1f)
        {
            isGenerating = false;
        }
    }

    public IEnumerator Begin()
    {
        isGenerating = true;
        Random.InitState(seed);
        //Initialise grid
        grid = new GameObject[gridSizeX][];
        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = new GameObject[gridSizeY];
        }
        progress = 0.1f;
        yield return null;
        //Generate();
        loadingText = "Initialising Level Generation...";
        GenInit();
        progress = 0.2f;
        yield return null;
        //GenPrim();
        yield return StartCoroutine(GenPrim());
        progress = 1f;

        var pSpawner = GameObject.FindGameObjectWithTag("PlayerSpawner");
        pSpawner.GetComponent<PlayerSpawner>().SpawnPlayer();

        yield break;
    }

    private void Generate()
    {
        GenInit();
        GenPrim();
    }

    private void GenInit()
    {
        nOfCells = gridSizeX * gridSizeY;
        //Debug.Log("Gen Initialising");
        //Debug.Log("Number of Cells: " + nOfCells);

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
        //Place important rooms
        for (int i = 0; i < numberOfRI; i++)
        {
            AnywhereGenerate(TileGenerator.RoomType.Room);
        }
    }

    private IEnumerator GenPrim()
    {

        loadingText = "Listing tiles...";

        //Get list of all tiles
        List<TileGenerator> allTiles = new List<TileGenerator>();
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                allTiles.Add(grid[i][j].GetComponent<TileGenerator>());
            }
        }

        progress = 0.22f;
        yield return null;


        loadingText = "Getting List of Occupied Cells...";

        //Get list of occupied cells
        List<TileGenerator> occupied = new List<TileGenerator>();
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                if (grid[i][j].GetComponent<TileGenerator>().isAssigned && grid[i][j].GetComponent<TileGenerator>().type == TileGenerator.RoomType.Entrance)
                {
                    occupied.Add(grid[i][j].GetComponent<TileGenerator>());
                }
            }
        }
        //Debug.Log(occupied.Count);

        progress = 0.23f;
        yield return null;

        loadingText = "Generating list of frontier cells...";

        //Get initial list of frontier cells
        List<TileGenerator> frontier = new List<TileGenerator>();
        for (int i = 0; i < occupied.Count; i++)
        {
            //Debug.Log("start of loop");
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
            //Debug.Log("end of loop");
        }
        for (int i = 0; i < frontier.Count; i++)
        {
            //Debug.Log("Frontier cell " + i + "= cell id: " + frontier[i].iD);
        }

        progress = 0.25f;
        yield return null;

        loadingText = "Connecting Cells...";

        //Step through randomly selecting frontier cells to connect to
        FrontierPop(frontier, occupied);

        while (occupied.Count + 1 + numberOfRI < nOfCells)
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

            //Debug.Log("Number of frontier cells: " + frontier.Count);
            if (frontier.Count != 0)
            {
                FrontierPop(frontier, occupied);
            }


        }

        progress = 0.40f;
        yield return null;

        loadingText = "Connecting Rooms of Importance...";

        //Connect extra rooms i.e. exit, rooms of importance
        List<TileGenerator> nonEntrance = new List<TileGenerator>();
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                if (grid[i][j].GetComponent<TileGenerator>().isAssigned && grid[i][j].GetComponent<TileGenerator>().type != TileGenerator.RoomType.Entrance)
                {
                    if (grid[i][j].GetComponent<TileGenerator>().type == TileGenerator.RoomType.Exit || grid[i][j].GetComponent<TileGenerator>().type == TileGenerator.RoomType.Room)
                    {
                        nonEntrance.Add(grid[i][j].GetComponent<TileGenerator>());
                    }
                }
            }
        }
        //Debug.Log(nonEntrance.Count + "NONENTRANCES");

        progress = 0.46f;
        yield return null;

        for (int i = 0; i < nonEntrance.Count; i++)
        {
            //Debug.Log("Iterating nonentrances");
            //Grab connection points
            var cPoints = nonEntrance[i].connectionPoints;
            //Loop through each index: 0 = connection on right, 1 = connection below etc.
            for (int j = 0; j < cPoints.Length; j++)
            {
                //Debug.Log("Iterating connections" + " " + nonEntrance[i].GetComponent<TileGenerator>().iD);
                switch (j)
                {
                    case 0:
                        if (cPoints[j])
                        {
                            //cell on right?- add connection on left
                            //Debug.Log(grid[nonEntrance[i].arrayPosX + 1][nonEntrance[i].arrayPosY].GetComponent<TileGenerator>().connectionPoints[0]);
                            grid[nonEntrance[i].arrayPosX + 1][nonEntrance[i].arrayPosY].GetComponent<TileGenerator>().connectionPoints[2] = true;
                            //Debug.Log("Connected on right");
                        }
                        break;
                    case 1:
                        if (cPoints[j])
                        {
                            //cell on south?- add connection on top
                            grid[nonEntrance[i].arrayPosX][nonEntrance[i].arrayPosY - 1].GetComponent<TileGenerator>().connectionPoints[3] = true;
                            //Debug.Log("Connected on south");
                        }
                        break;
                    case 2:
                        if (cPoints[j])
                        {
                            //cell on left?- add connection on right
                            grid[nonEntrance[i].arrayPosX - 1][nonEntrance[i].arrayPosY].GetComponent<TileGenerator>().connectionPoints[0] = true;
                            //Debug.Log("Connected on left");
                        }
                        break;
                    case 3:
                        if (cPoints[j])
                        {
                            //cell on north?- add connection on south
                            grid[nonEntrance[i].arrayPosX][nonEntrance[i].arrayPosY + 1].GetComponent<TileGenerator>().connectionPoints[1] = true;
                            //Debug.Log("Connected on north");
                        }
                        break;
                }

            }
        }

        progress = 0.55f;
        yield return null;

        loadingText = "Consolidating Tiles...";

        //Consolidate all tiles
        for (int i = 0; i < allTiles.Count; i++)
        {
            if (allTiles[i].unchangable == false)
            {
                allTiles[i].ConsolidateTile();
            }
        }

        progress = 0.70f;
        yield return null;

        loadingText = "Spawning Entities...";

        //Generate from spawn points
        var sPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        if (sPoints.Length > 0)
        {
            for (int i = 0; i < sPoints.Length; i++)
            {
                sPoints[i].GetComponent<SpawnPoint>().Spawn();
            }
        }


        gm_manager.SetActive(true);

        progress = 0.96f;
        yield break;

    }


    private void PositionBorder(BorderPos b, TileGenerator.RoomType rType)
    {
        var rnd = Random.Range(1, grid.Length - 2);
        switch (b)
        {
            case BorderPos.North:
                //Get Target Node randomly from appropriate axis
                rnd = Random.Range(1, grid.Length - 2);
                var targetN = grid[rnd][grid[0].Length - 1].GetComponent<TileGenerator>();
                if (!targetN.isAssigned)
                {
                    targetN.SetRoomType(rType, 1);
                }
                else
                {
                    while (targetN.isAssigned == true)
                    {
                        rnd = Random.Range(1, grid.Length - 2);
                        targetN = grid[rnd][grid[0].Length - 1].GetComponent<TileGenerator>();
                    }
                    targetN.SetRoomType(rType, 1);
                }
                targetN.SetRotation(90);
                break;

            case BorderPos.East:
                rnd = Random.Range(1, grid.Length - 2);
                var targetE = grid[grid[0].Length - 1][rnd].GetComponent<TileGenerator>();
                if (!targetE.isAssigned)
                {
                    targetE.SetRoomType(rType, 2);
                }
                else
                {
                    while (targetE.isAssigned == true)
                    {
                        rnd = Random.Range(1, grid.Length - 2);
                        targetE = grid[grid[0].Length - 1][rnd].GetComponent<TileGenerator>();
                    }
                    targetE.SetRoomType(rType, 2);
                }
                targetE.SetRotation(180);
                break;

            case BorderPos.South:
                rnd = Random.Range(1, grid.Length - 2);
                var targetS = grid[rnd][0].GetComponent<TileGenerator>();
                if (!targetS.isAssigned)
                {
                    targetS.SetRoomType(rType, 3);
                }
                else
                {
                    while (targetS.isAssigned == true)
                    {
                        rnd = Random.Range(1, grid.Length - 2);
                        targetS = grid[rnd][0].GetComponent<TileGenerator>();
                    }
                    targetS.SetRoomType(rType, 3);
                }
                targetS.SetRotation(270);
                break;

            case BorderPos.West:
                rnd = Random.Range(1, grid.Length - 2);
                var targetW = grid[0][rnd].GetComponent<TileGenerator>();
                if (!targetW.isAssigned)
                {
                    targetW.SetRoomType(rType, 0);
                }
                else
                {
                    while (targetW.isAssigned == true)
                    {
                        rnd = Random.Range(1, grid.Length - 2);
                        targetW = grid[0][rnd].GetComponent<TileGenerator>();
                    }
                    targetW.SetRoomType(rType, 0);
                }
                targetW.SetRotation(0);
                break;
        }
    }


    private void AnywhereGenerate(TileGenerator.RoomType rType)
    {
        var rnd1 = Random.Range(1, grid.Length - 2);
        var rnd2 = Random.Range(1, grid[0].Length - 2);
        var rotrnd = Random.Range(0, 3);
        var target = grid[rnd1][rnd2].GetComponent<TileGenerator>();
        var tAdjacentT = grid[rnd1][rnd2+1].GetComponent<TileGenerator>();
        var tAdjacentR = grid[rnd1 + 1][rnd2].GetComponent<TileGenerator>();
        var tAdjacentB = grid[rnd1][rnd2 - 1].GetComponent<TileGenerator>();
        var tAdjacentL = grid[rnd1 - 1][rnd2].GetComponent<TileGenerator>();
        if (!target.isAssigned && !tAdjacentT.isAssigned && !tAdjacentR.isAssigned && !tAdjacentB.isAssigned && !tAdjacentL.isAssigned)
        {
            target.SetRoomType(rType, 0);
        }
        //Regenerate until empty space with enough clearance is found
        else
        {
            var usable = false;
            int maxIterations = 10, counter = 0;
            while (!usable)
            {
                if(counter >= maxIterations)
                {
                    break;
                }
                rnd1 = Random.Range(1, grid.Length - 2);
                rnd2 = Random.Range(1, grid[0].Length - 2);
                tAdjacentT = grid[rnd1][rnd2 + 1].GetComponent<TileGenerator>();
                tAdjacentR = grid[rnd1 + 1][rnd2].GetComponent<TileGenerator>();
                tAdjacentB = grid[rnd1][rnd2 - 1].GetComponent<TileGenerator>();
                tAdjacentL = grid[rnd1 - 1][rnd2].GetComponent<TileGenerator>();
                target = grid[rnd1][rnd2].GetComponent<TileGenerator>();
                //Debug.Log("reassigning: place already taken = " + target.iD);
                if(!target.isAssigned && !tAdjacentT.isAssigned && !tAdjacentR.isAssigned && !tAdjacentB.isAssigned && !tAdjacentL.isAssigned)
                {
                    usable = true;
                }
                counter += 1;
            }
            if (usable)
            {
                target.SetRoomType(rType, 0);
            }
            else
            {
                numberOfRI -= 1;
            }
        }

        //Randomly rotate
        switch (rotrnd)
        {
            case 0:
                target.SetRotation(0);
                target.SetRoomType(rType, 0);
                break;
            case 1:
                target.SetRotation(90);
                target.SetRoomType(rType, 1);
                break;
            case 2:
                target.SetRotation(180);
                target.SetRoomType(rType, 2);
                break;
            case 3:
                target.SetRotation(270);
                target.SetRoomType(rType, 3);
                break;
        }
        //Debug.Log(rotrnd + "rotrnd");
        //BorderCheck(target);
    }


    private void FrontierPop(List<TileGenerator> f, List<TileGenerator> o)
    {
        //Pick random frontier cell to pop
        int ran = Random.Range(0, f.Count);
        //Add to Occupied list
        f[ran].SetRoomType(TileGenerator.RoomType.XConnect, 0);
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


    public void LoadBossRoom()
    {
        loadBossRoom = false;
        GameObject.FindGameObjectWithTag("floor").SetActive(false);
        Instantiate(bossRoomRef);
        var pSpawner = GameObject.FindGameObjectWithTag("PlayerSpawner");
        pSpawner.GetComponent<PlayerSpawner>().SpawnPlayer();
    }
}
