using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int life = 100;
    [SerializeField] private float speed = 15f;
    [SerializeField] private int damage = 10;

    private List<Vector3> path;
    private int currentPathIndex = 0;

    void Start()
    {
        SetTargetPosition();
    }

    void Update()
    {
        // move within path
        if (path != null)
        {
            Vector3 nextWaypointPosition = path[currentPathIndex];
            if (Vector3.Distance(this.transform.position, nextWaypointPosition) > 1f)
            {
                Vector3 moveDirection = (nextWaypointPosition - this.transform.position).normalized;
                this.transform.position += moveDirection * speed * Time.deltaTime;
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= path.Count)
                {
                    // reached objective
                    Die();
                    GameManager.Instance.ModifyPlayerHealth(-damage);
                }
            }

        }
    }

    public void LoseLife(int amount)
    {
        this.life -= amount;

        if (life <= 0)
        {
            Die();
            GameManager.Instance.ModifyGold(50);
            GameManager.Instance.GainPoints(10);
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
        WaveSpawner.Instance.RemoveEnemy(this.gameObject);
    }
    public void SetTargetPosition()
    {
        GameObject objective = GameObject.Find("Objective");
        Vector3 targetPosition = objective.transform.position;

        currentPathIndex = 0;
        path = Pathfinding.Instance.FindPath(this.transform.position, targetPosition);

        if ( path != null && path.Count > 1)
        {
            path.RemoveAt(0);
        }
    }
}
