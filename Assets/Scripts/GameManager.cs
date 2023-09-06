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
    private TMP_Text placeTowerText;

    public static GameManager Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        pathfinding = new Pathfinding(10, 10);

        UpdateLifeText();
        UpdateGoldText();
        UpdatePointsText();

        placeTowerText = GameObject.Find("PlaceTowerText").GetComponent<TMP_Text>();
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
                placeTowerText.text = "";

                CreateTowerAt(mouseWorldPosition);
            }
        }
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
        placeTowerText.text = "Click on map to place tower!";
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

        if (grid.isValid(position) && gold >= towerCost && grid.isWalkable(position))
        {
            ModifyGold(-towerCost);
            
            Vector3 centeredPosition = grid.CenterPositionOnCell(position);
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
