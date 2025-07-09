using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0,enemyList.Count)];

    public int WaveNumber => waveNumber;//敌人波数

    public float TimeBetweenWaves => timeBetweenWaves;//敌人波数时间间隔



    [SerializeField] bool spawnEnemy = true;

    [SerializeField] GameObject waveUI;

    [SerializeField] GameObject[] enemyPrefabs;

    [SerializeField] float timeBetweenSpawns = 1f;//生成时间间隔

    [SerializeField] float timeBetweenWaves = 3f;//每波敌人生成间隔时间

    [SerializeField] int minEnemyAmount = 4;

    [SerializeField] int maxEnemyAmount = 10;

    [Header("---Boss Settings---")]

    [SerializeField] GameObject bossPrefab;

    [SerializeField] int bossWaveNumber;

    int waveNumber = 1;//敌人波数

    int enemyAmount;//敌人数量

    WaitForSeconds waitTimeBetweenSpawns;//等待生产间隔时间

    WaitForSeconds waitTimeBetweenWaves;

    WaitUntil waitUntilNoEnemy;//直到敌人全部死亡

    List<GameObject> enemyList;

    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();//列表初始化
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        waitUntilNoEnemy = new WaitUntil(() => enemyList.Count == 0);//列表中的敌人数量为0时为真
    }

    IEnumerator Start()
    {
        while (spawnEnemy && GameManager.GameState != GameState.GameOver)
        {

            waveUI.SetActive(true);

            yield return waitTimeBetweenWaves;

            waveUI.SetActive(false);

            yield return StartCoroutine(RandomlySpawnCoroutine());//等待敌人生成完毕  
        }
        
    }

    IEnumerator RandomlySpawnCoroutine()
    {
        if (waveNumber % bossWaveNumber == 0)
        {
            var boss = PoolManager.Release(bossPrefab);
            enemyList.Add(boss);
        }
        else
        {
            enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / bossWaveNumber, maxEnemyAmount);//敌人数量
            for (int i = 0; i < enemyAmount; i++)
            {
                enemyList.Add(PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));//敌人生成时将敌人加入敌人列表

                yield return waitTimeBetweenSpawns;
            }
        }
        
        yield return waitUntilNoEnemy;//当没有敌人时进行下一步

        waveNumber++;
    }

    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);//敌人摧毁时将敌人从列表中移除
}
