using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject SimpleTower;
    public GameObject FastTower;
    public GameObject BigTower;

    [SerializeField] private int playerHealth = 100;
    private Pathfinding pathfinding;

    private int gold = 200;
    private int points = 0;

    private bool isBuilding = false;
    private int buildType = 0;

    public static GameManager Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        pathfinding = new Pathfinding(10, 10);

        UpdateLifeText();
        UpdateGoldText();
        UpdatePointsText();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Utils.GetMouseWorldPosition();
            Vector2 position = pathfinding.GetGrid().GetXY(mouseWorldPosition);
            if (isBuilding)
            {
                isBuilding = false;

                Debug.Log("trying to build tower on: " + position.x + ", " + position.y);
                CreateTowerAt(mouseWorldPosition);
            }


            //Debug.Log(position);

            //List<PathNode> path = pathfinding.FindPath(0, 0, (int)position.x, (int)position.y);

            //if (path != null)
            //{
            //    Debug.Log("found a path");
            //    for (int i = 0; i < path.Count - 1; i++)
            //    {
            //        Debug.Log(path[i]);
            //        Debug.DrawLine(
            //            new Vector3(path[i].GetX(), path[i].GetY()) * 10f + Vector3.one * 5f,
            //            new Vector3(path[i + 1].GetX(), path[i + 1].GetY()) * 10f + Vector3.one * 5f,
            //            Color.red,
            //            3f
            //        );
            //    }

            //    var enemy = GameObject.Find("EnemyMedium");
            //    enemy.transform.position = mouseWorldPosition;
            //}
        }

        //if (Input.GetMouseButtonDown(1))
        //{
        //    Debug.Log(grid.GetValue(Utils.GetMouseWorldPosition()));
        //}
    }

    public Pathfinding GetPathfinding()
    {
        return pathfinding;
    }

    public int ModifyPlayerHealth(int modifier)
    {
        playerHealth += modifier;
        UpdateLifeText();

        if (playerHealth <= 0)
        {
            GameOver();
        }
        
        return playerHealth;
    }

    private void UpdateLifeText()
    {
        var lifeText = GameObject.Find("LifeText");
        lifeText.GetComponent<TMP_Text>().text = "LIFE: " + playerHealth.ToString();
    }

    private void UpdateGoldText()
    {
        var goldText = GameObject.Find("GoldText");
        goldText.GetComponent<TMP_Text>().text = "GOLD: " + gold.ToString() + "g";
    }

    private void UpdatePointsText()
    {
        var pointsText = GameObject.Find("PointsText");
        pointsText.GetComponent<TMP_Text>().text = "POINTS: " + points.ToString();
    }

    private void GameOver()
    {
        // TODO: add option to restart game

        // close game
        Debug.Log("closing game ..");
        Application.Quit();

    }

    public void GainPoints(int amount)
    {
        points += amount;
        UpdatePointsText();
    }

    public void ModifyGold(int amount)
    {
        gold += amount;
        UpdateGoldText();
    }

    public int GetGold()
    {
        return gold;
    }

    public void SetIsBuilding(int type)
    {
        isBuilding = true;
        buildType = type;
    }

    private void CreateTowerAt(Vector3 position)
    {
        GameObject towerType;

        switch (buildType)
        {
            case 0:
            default:
                towerType = SimpleTower; break;

            case 1:
                towerType = FastTower; break;

            case 2:
                towerType = BigTower; break;
        }

        int towerCost = towerType.GetComponent<Tower>().GetCost();
        GridManager grid = pathfinding.GetGrid();

        Vector3 centeredPosition = grid.CenterPositionOnCell(position);

        if (gold >= towerCost)
        {
            ModifyGold(-towerCost);
            Instantiate(towerType, centeredPosition, Quaternion.identity);

            grid.SetNodeUnWalkable(centeredPosition);

            // recalculate routes for each enemy
            List<GameObject> enemies = WaveSpawner.Instance.GetEnemies();
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<Enemy>().SetTargetPosition();
            }
        }
    }
}
