using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject enemyToFollow;
    [SerializeField] private float speed = 100f;
    private float timeToLive = 3f;
    private float timeAlive = 0;

    private int damage = 25;

    void Start()
    {
        List<GameObject> enemies = WaveSpawner.Instance.GetEnemies();
        
        // follow closest enemy
        enemyToFollow = enemies[0];

        for (int i = 1; i < enemies.Count; i++)
        {
            float newDistance = Vector3.Distance(enemies[i].transform.position, this.transform.position);
            float currentDistance = Vector3.Distance(enemyToFollow.transform.position, this.transform.position);

            if (newDistance < currentDistance)
            {
                enemyToFollow = enemies[i];
            }
        }
    }
    
    void Update()
    {
        timeAlive += Time.deltaTime;

        if (enemyToFollow == null || timeAlive > timeToLive)
        {
            Die();
            return;
        }

        // move
        Vector3 targetPosition = enemyToFollow.transform.position;
        Vector3 moveDirection = (targetPosition - this.transform.position).normalized;
        this.transform.position += moveDirection * speed * Time.deltaTime;

        // hit
        if (Vector3.Distance(this.transform.position, targetPosition) < 1f)
        {
            Die();
            enemyToFollow.GetComponent<Enemy>().LoseLife(damage);
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    public void SetDamage(int newDamage)
    {
        this.damage = newDamage;
    }
}
