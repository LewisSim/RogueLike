using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReqManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestsQue = new Queue<PathRequest>();

    PathRequest currentPathRequest;

    static ReqManager instance;
    Pathfind pathfinding;
    bool isProcessing;


    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfind>();
    }


    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestsQue.Enqueue(newRequest);
        instance.TryProcessNext();

    }

    void TryProcessNext()
    {
        if (!isProcessing && pathRequestsQue.Count > 0)
        {
            //Debug.Log("processing");
            currentPathRequest = pathRequestsQue.Dequeue();
            isProcessing = true;
            pathfinding.StartFindingPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessing(Vector3[] path, bool done)
    {
        currentPathRequest.callback(path, done);

        isProcessing = false;
        TryProcessNext();
    }


    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> back)
        {
            pathStart = start;
            pathEnd = end;
            callback = back;
        }

    }

}
