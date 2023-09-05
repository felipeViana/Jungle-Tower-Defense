using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Pathfinding pathfinding;

    private void Start()
    {
        pathfinding = new Pathfinding(10, 10);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Utils.GetMouseWorldPosition();

            Vector2 position = pathfinding.GetGrid().GetXY(mouseWorldPosition);
            Debug.Log(position);

            List<PathNode> path = pathfinding.FindPath(0, 0, (int)position.x, (int)position.y);

            if (path != null)
            {
                Debug.Log("found a path");
                for (int i = 0; i < path.Count - 1; i++) 
                {
                    Debug.Log(path[i]);
                    Debug.DrawLine(
                        new Vector3(path[i].GetX(), path[i].GetY()) * 10f + Vector3.one * 5f,
                        new Vector3(path[i + 1].GetX(), path[i + 1].GetY()) * 10f + Vector3.one * 5f,
                        Color.green,
                        3f
                    );
                }
            }
        }

        //if (Input.GetMouseButtonDown(1))
        //{
        //    Debug.Log(grid.GetValue(Utils.GetMouseWorldPosition()));
        //}
    }

    
}
