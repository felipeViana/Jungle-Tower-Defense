using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject SimpleTower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateSimpleTower()
    {
        GameManager gameManager = GameManager.Instance;
        int towerCost = SimpleTower.GetComponent<Tower>().GetCost();

        if (gameManager.GetGold() >= towerCost)
        {
            gameManager.ModifyGold(-towerCost);


            Instantiate(SimpleTower, this.transform.position, Quaternion.identity);

            // make tower position not walkable
            Pathfinding.Instance.GetGrid().SetNodeUnWalkable(this.transform.position);

            // recalculate routes for each enemy
            List<GameObject> enemies = WaveSpawner.Instance.GetEnemies();
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<Enemy>().SetTargetPosition();
            }
        }
    }
}
