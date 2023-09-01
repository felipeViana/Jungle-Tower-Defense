using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Grid grid;

    private void Start()
    {
        grid = new Grid(4, 2, 10);       
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(Utils.GetMouseWorldPosition(), 42);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(Utils.GetMouseWorldPosition()));
        }
    }

    
}
