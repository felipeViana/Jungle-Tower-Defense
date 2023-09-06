using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private int damage = 25;
    [SerializeField] private float timeToShoot = 1f;
    private float timePassedAfterShoot = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // shoot
        if (timePassedAfterShoot > timeToShoot)
        {
            Debug.Log("shooting ..");
            timePassedAfterShoot = 0f;
        }
        
        timePassedAfterShoot += Time.deltaTime;
    }
}
