﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    //NOTE: Current use of public variables is for debugging only, refactor at a later date
    public GameObject[] entrance, exit, lConnect, tConnect, xConnect, sConnect, room, deadEnd;
    public enum RoomType { Entrance, Exit, LConnect, TConnect, XConnect, SConnect, Room, DeadEnd };
    public bool[] connectionPoints;
    public RoomType type;
    int width, height;
    public bool isAssigned = false;
    public int iD;
    public int arrayPosX, arrayPosY;
    public bool unchangable;
    public TileGenerator parent;


    private void Awake()
    {
        //Four connection points: 0 = right, 1 = down, 2 = left, 3 = up
        connectionPoints = new bool[4];

        //Assign all to false by default
        for (int i = 0; i < connectionPoints.Length; i++)
        {
            connectionPoints[i] = false;
        }
        unchangable = false;
    }


    public void SetWidthHeight(int w, int h)
    {
        width = w;
        height = h;
        int depth = width;
        transform.localScale = new Vector3(width, height, depth);
    }

    public void SetRoomType(RoomType t, int rotation)
    {
        type = t;
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        switch (type)
        {
            case RoomType.Entrance:
                Instantiate(VariantPick(entrance, true, rotation), transform);
                unchangable = true;
                //Debug.Log("Entrance spawned");
                gameObject.name = "Entrance";
                break;

            case RoomType.Exit:
                Instantiate(VariantPick(exit, true, rotation), transform);
                unchangable = true;
                //Debug.Log("Exit spawned");
                gameObject.name = "Exit";
                break;

            case RoomType.LConnect:
                Instantiate(VariantPick(lConnect, true, rotation), transform);
                gameObject.name = "L connect";
                break;

            case RoomType.TConnect:
                Instantiate(VariantPick(tConnect, true, rotation), transform);
                gameObject.name = "T connect";
                break;

            case RoomType.XConnect:
                Instantiate(VariantPick(xConnect, false, rotation), transform);
                gameObject.name = "X connect";
                break;

            case RoomType.SConnect:
                Instantiate(VariantPick(sConnect, true, rotation), transform);
                gameObject.name = "Straight connect";
                break;

            case RoomType.Room:
                Instantiate(VariantPick(room, true, rotation), transform);
                gameObject.name = "Room";
                unchangable = true;
                break;

            case RoomType.DeadEnd:
                Instantiate(VariantPick(deadEnd, true, rotation), transform);
                gameObject.name = "Dead End";
                break;

            default:
                break;
        }
        isAssigned = true;
    }

    public void SetRotation(float r)
    {
        //Debug.Log("WAS " + transform.rotation.y);
        //transform.rotation = Quaternion.Euler(0f, r, 0f);
        //Debug.Log(transform.rotation.eulerAngles.y);
        //Assign connection point if room is pre placed type e.g. Entrance, Exit, Important room
        if (type == RoomType.Entrance || type == RoomType.Exit || type == RoomType.Room)
        {
            switch (r)
            {
                case 0:
                    connectionPoints[0] = true;
                    connectionPoints[1] = false;
                    connectionPoints[2] = false;
                    connectionPoints[3] = false;
                    break;
                case 90:
                    connectionPoints[0] = false;
                    connectionPoints[1] = true;
                    connectionPoints[2] = false;
                    connectionPoints[3] = false;
                    break;
                case 180:
                    connectionPoints[0] = false;
                    connectionPoints[1] = false;
                    connectionPoints[2] = true;
                    connectionPoints[3] = false;
                    break;
                case 270:
                    connectionPoints[0] = false;
                    connectionPoints[1] = false;
                    connectionPoints[2] = false;
                    connectionPoints[3] = true;
                    break;

            }
        }
        //Debug.Log(iD + " Got Rotated: " + r);
    }

    public List<int> GetNonConnectedDirections()
    {
        if (type == RoomType.Entrance || type == RoomType.Exit || type == RoomType.Room)
        {
            //Tell it it's fully connected
            List<int> full = new List<int>();
            //for (int i = 0; i < 4; i++)
            //{
            //    full.Add(i);
            //}
            return full;
        }
        else
        {
            List<int> nonConnected = new List<int>();
            for (int i = 0; i < connectionPoints.Length; i++)
            {
                if (connectionPoints[i] == false)
                {
                    nonConnected.Add(i);
                }
            }
            return nonConnected;
        }
    }

    public void ConsolidateTile()
    {
        if (type != RoomType.Entrance && type != RoomType.Exit && type != RoomType.Room)
        {
            if (connectionPoints[0] && connectionPoints[1] && !connectionPoints[2] && !connectionPoints[3])
            {
                SetRoomType(RoomType.LConnect, 0);
            }
            if (connectionPoints[1] && connectionPoints[2] && !connectionPoints[3] && !connectionPoints[0])
            {
                SetRoomType(RoomType.LConnect, 1);
                //SetRotation(90);
            }
            if (connectionPoints[2] && connectionPoints[3] && !connectionPoints[0] && !connectionPoints[1])
            {
                SetRoomType(RoomType.LConnect, 2);
                //SetRotation(180);
            }
            if (connectionPoints[3] && connectionPoints[0] && !connectionPoints[1] && !connectionPoints[2])
            {
                SetRoomType(RoomType.LConnect, 3);
                //SetRotation(270);
            }
            if (connectionPoints[0] && connectionPoints[2] && !connectionPoints[1] && !connectionPoints[3])
            {
                SetRoomType(RoomType.SConnect, 0);
            }
            if (connectionPoints[1] && connectionPoints[3] && !connectionPoints[2] && !connectionPoints[0])
            {
                SetRoomType(RoomType.SConnect, 1);
                //SetRotation(90);
            }
            //if (connectionPoints[2] && connectionPoints[0] && !connectionPoints[1] && !connectionPoints[3])
            //{
            //    SetRoomType(RoomType.SConnect);
            //    SetRotation(180);
            //}
            //if (connectionPoints[3] && connectionPoints[1] && !connectionPoints[2] && !connectionPoints[0])
            //{
            //    SetRoomType(RoomType.SConnect);
            //    SetRotation(90);
            //}
            if (connectionPoints[0] && connectionPoints[1] && connectionPoints[2] && !connectionPoints[3])
            {
                SetRoomType(RoomType.TConnect, 0);
            }
            if (connectionPoints[1] && connectionPoints[2] && connectionPoints[3] && !connectionPoints[0])
            {
                SetRoomType(RoomType.TConnect, 1);
                //SetRotation(90);
            }
            if (connectionPoints[2] && connectionPoints[3] && connectionPoints[0] && !connectionPoints[1])
            {
                SetRoomType(RoomType.TConnect, 2);
                //SetRotation(180);
            }
            if (connectionPoints[3] && connectionPoints[0] && connectionPoints[1] && !connectionPoints[2])
            {
                SetRoomType(RoomType.TConnect, 3);
                //SetRotation(270);
            }
            if (connectionPoints[0] && connectionPoints[1] && connectionPoints[2] && connectionPoints[3])
            {
                SetRoomType(RoomType.XConnect, 0);
            }
            if (connectionPoints[0] && !connectionPoints[1] && !connectionPoints[2] && !connectionPoints[3])
            {
                SetRoomType(RoomType.DeadEnd, 0);
            }
            if (!connectionPoints[0] && connectionPoints[1] && !connectionPoints[2] && !connectionPoints[3])
            {
                SetRoomType(RoomType.DeadEnd, 1);
                //SetRotation(90);
            }
            if (!connectionPoints[0] && !connectionPoints[1] && connectionPoints[2] && !connectionPoints[3])
            {
                SetRoomType(RoomType.DeadEnd, 2);
                //SetRotation(180);
            }
            if (!connectionPoints[0] && !connectionPoints[1] && !connectionPoints[2] && connectionPoints[3])
            {
                SetRoomType(RoomType.DeadEnd, 3);
                //SetRotation(270);
            }
        }
    }

    private GameObject VariantPick(GameObject[] roomSubset, bool isRotationSensitive, int rotation)
    {

        if (!isRotationSensitive)
        {
            GameObject roomOut = roomSubset[Random.Range(0, roomSubset.Length)];
            return roomOut;
        }
        else
        {
            //GameObject roomOut = ;
            //return roomOut;
            switch (rotation)
            {
                case 0:
                    return roomSubset[0];
                case 1:
                    return roomSubset[1];
                case 2:
                    return roomSubset[2];
                case 3:
                    return roomSubset[3];
                default:
                    return roomSubset[0];
            }
        }
    }
}
