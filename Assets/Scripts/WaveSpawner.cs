using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private float delayBetweenSpawns = 1f;
    private float timePassedAfterSpawn = 0f;

    [SerializeField] private float timeBetweenWaves = 5f;
    private float timePassedAfterWave = 0f;

    private int enemiesSpawned = 0;

    private int wave = 1;
    private List<GameObject> enemies = new List<GameObject> ();

    public GameObject EnemyMedium;
    public GameObject EnemyBig;
    public GameObject EnemySmall;

    public static WaveSpawner Instance { get; private set; }


    void Start()
    {
        Instance = this;
        UpdateWaveText();
    }

    // Update is called once per frame
    void Update()
    {
        // generate enemies
        if (enemiesSpawned < EnemiesCountForWave() && timePassedAfterSpawn > delayBetweenSpawns)
        {
            enemiesSpawned++;
            timePassedAfterSpawn = 0;

            int enemyType = Random.Range(0, 3);
            switch(enemyType)
            {
                case 0:
                default:
                    enemies.Add(Instantiate(EnemyMedium, EnemyMedium.transform.position, EnemyMedium.transform.rotation));
                    break;
                case 1:
                    enemies.Add(Instantiate(EnemyBig, EnemyBig.transform.position, EnemyBig.transform.rotation));
                    break;
                case 2:
                    enemies.Add(Instantiate(EnemySmall, EnemySmall.transform.position, EnemySmall.transform.rotation));
                    break;
            }
        }
        timePassedAfterSpawn += Time.deltaTime;
        UpdateTimeText();

        // finish wave and start next one
        if (enemiesSpawned > 0 && enemies.Count == 0)
        {
            timePassedAfterWave += Time.deltaTime;
            UpdateTimeText();
            if (timePassedAfterWave > timeBetweenWaves)
            {
                wave++;
                enemiesSpawned = 0;
                timePassedAfterSpawn = 0;
                timePassedAfterWave = 0;
                UpdateWaveText();
            }

        }
    }

    public List<GameObject> GetEnemies()
    {
        return enemies;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    private int EnemiesCountForWave()
    {
        return wave + 3;
    }

    private void UpdateWaveText()
    {
        var WaveText = GameObject.Find("WaveText");
        WaveText.GetComponent<TMP_Text>().text = "WAVE: " + wave.ToString();
    }

    private void UpdateTimeText()
    {
        float timeToNextWave = timeBetweenWaves - timePassedAfterWave;
        var WaveText = GameObject.Find("TimeText");

        if (timePassedAfterWave == 0)
        {
            WaveText.GetComponent<TMP_Text>().text = "";
        }
        else
        {
            WaveText.GetComponent<TMP_Text>().text = "NEXT WAVE IN: " + timeToNextWave.ToString("0.0");
        }
    }
}
