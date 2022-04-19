using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class CreateRoadMesh2 : MonoBehaviour
{

    public Transform[] waypoints;
    public PathCreator pathCreator;
    public RoadMeshCreator roadMeshCreator;
    // Start is called before the first frame update
    void Start()
    {
        CreateRoadForMeBaby();
    }

	public void CreateRoadForMeBaby()
	{
        if (waypoints.Length > 0)
        {
            // Create a new bezier path from the waypoints.
            BezierPath bezierPath = new BezierPath(waypoints, false, PathSpace.xyz);
            pathCreator.bezierPath = bezierPath;
        }
        pathCreator.TriggerPathUpdate();
        roadMeshCreator.UpdateRoad();
    }

}
