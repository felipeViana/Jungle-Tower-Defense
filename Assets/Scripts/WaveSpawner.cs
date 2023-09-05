using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private int wave = 0;
    private List<GameObject> enemies = new List<GameObject> ();

    public GameObject EnemyMedium;
    public GameObject EnemyBig;
    public GameObject EnemySmall;

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count == 0) 
        {
            // create enemies

            enemies.Add(Instantiate(EnemyMedium, this.transform.position, Quaternion.identity));
            enemies.Add(Instantiate(EnemyMedium, this.transform.position, Quaternion.identity));
            enemies.Add(Instantiate(EnemyMedium, this.transform.position, Quaternion.identity));
        }


    }
}
