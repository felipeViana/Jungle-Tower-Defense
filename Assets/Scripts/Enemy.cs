using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int life = 100;
    [SerializeField] private float speed = 15f;
    [SerializeField] private int damage = 10;

    void Start()
    {
        var spawner = GameObject.Find("WaveSpawner");

        this.transform.position = spawner.transform.position;
    }

    void Update()
    {
        // move to objective
        var objective = GameObject.Find("Objective");
        Vector3 targetPosition = objective.transform.position;
        Vector3 moveDirection = (targetPosition - this.transform.position).normalized;
        this.transform.position += moveDirection * speed * Time.deltaTime;

        // die when reach objective
        if (Vector3.Distance(this.transform.position, targetPosition) < 1f)
        {
            // die
            Destroy(this.gameObject);

            // take players health
            GameManager.Instance.ModifyPlayerHealth(-damage);
        }
    }
}