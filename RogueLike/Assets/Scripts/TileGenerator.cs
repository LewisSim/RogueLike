using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    //NOTE: Current use of public variables is for debugging only, refactor at a later date
    public GameObject entrance, exit, lConnect, tConnect, xConnect, sConnect, room, deadEnd;
    public enum RoomType { Entrance, Exit, LConnect, TConnect, XConnect, SConnect, Room, DeadEnd };
    public bool[] connectionPoints;
    RoomType type;
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

    public void SetRoomType(RoomType t)
    {
        type = t;
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        switch (type)
        {
            case RoomType.Entrance:
                Instantiate(entrance, transform);
                unchangable = true;
                Debug.Log("Entrance spawned");
                break;

            case RoomType.Exit:
                Instantiate(exit, transform);
                unchangable = true;
                Debug.Log("Exit spawned");
                break;

            case RoomType.LConnect:
                Instantiate(lConnect, transform);
                break;

            case RoomType.TConnect:
                Instantiate(tConnect, transform);
                break;

            case RoomType.XConnect:
                Instantiate(xConnect, transform);
                break;

            case RoomType.SConnect:
                Instantiate(sConnect, transform);
                break;

            case RoomType.Room:
                Instantiate(room, transform);
                unchangable = true;
                break;

            case RoomType.DeadEnd:
                Instantiate(deadEnd, transform);
                break;

            default:
                break;
        }
        isAssigned = true;
    }

    public void SetRotation(float r)
    {
        Debug.Log("WAS " + transform.rotation.y);
        transform.rotation = Quaternion.Euler(0f, r, 0f);
        Debug.Log(transform.rotation.eulerAngles);
        //Assign connection point if room is pre placed type e.g. Entrance, Exit, Important room
        if (type == RoomType.Entrance || type == RoomType.Exit || type == RoomType.Room)
        {
            switch (transform.rotation.y)
            {
                case 0:
                    connectionPoints[0] = true;
                    connectionPoints[1] = false;
                    connectionPoints[2] = false;
                    connectionPoints[3] = false;
                    break;
                case -90:
                    connectionPoints[0] = false;
                    connectionPoints[1] = true;
                    connectionPoints[2] = false;
                    connectionPoints[3] = false;
                    break;
                case -180:
                    connectionPoints[0] = false;
                    connectionPoints[1] = false;
                    connectionPoints[2] = true;
                    connectionPoints[3] = false;
                    break;
                case -270:
                    connectionPoints[0] = false;
                    connectionPoints[1] = false;
                    connectionPoints[2] = false;
                    connectionPoints[3] = true;
                    break;

            }
        }
        Debug.Log(iD + " Got Rotated: " + r);
    }
}
