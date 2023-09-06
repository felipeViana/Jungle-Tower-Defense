using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject Bullet;

    [SerializeField] private float range = 100f;
    [SerializeField] private int damage = 25;
    [SerializeField] private float timeToShoot = 1f;
    private float timePassedAfterShoot = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        // shoot
        if (timePassedAfterShoot > timeToShoot)
        {
            timePassedAfterShoot = 0f;

            // if there's an enemy in range
            bool hasEnemyInRange = false;
            List<GameObject> enemies = WaveSpawner.Instance.GetEnemies();
            for (int i = 0;  i < enemies.Count; i++)
            {
                float distance = Vector3.Distance(enemies[i].transform.position, this.transform.position);
                Debug.Log(distance);
                if (distance < range)
                {
                    hasEnemyInRange = true;
                    break;
                }
            }

            if (hasEnemyInRange)
            {
                // instatiate bullet
                GameObject bullet = Instantiate(Bullet, this.transform.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().SetDamage(damage);
            }
        }
        
        timePassedAfterShoot += Time.deltaTime;
    }
}
