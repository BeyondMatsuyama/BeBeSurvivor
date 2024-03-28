using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミー管理
/// </summary>
public class EnemyController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ExpController expController;

    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject parent;

    private List<GameObject> enemies = new List<GameObject>();

    private int no = 0;

    /// <summary>
    /// 起動時処理
    /// </summary>
    void Start()
    {
        StartCoroutine("spawnBase");
        StartCoroutine("spawnWave");
    }

    /// <summary>
    /// エネミー生成（基本）
    /// </summary>
    private IEnumerator spawnBase()
    {
        const int MAX_BASE_ENEMYS  = 50;
        const float spawnRadiusMin = 8.0f;
        const float spawnRadiusMax = 10.0f;
        const float spawnInterval  = 1f;
        var waitTime = new WaitForSeconds(spawnInterval);
        while(true)
        {
            // 常に MAX体のエネミーが存在するように生成する
            if(enemies.Count < MAX_BASE_ENEMYS)
            {
                // プレイヤー位置
                Vector2 playerPos = playerController.GetPlayer().Position;
                // 配置半径を決定
                float r = UnityEngine.Random.Range(spawnRadiusMin, spawnRadiusMax);
                // 配置角度を決定
                float angle = UnityEngine.Random.Range(0, 360);

                // 生成
                spawn(playerPos, r, angle);
            }
            yield return waitTime;
        }
    }

    private IEnumerator spawnWave()
    {
        const float spawnInterval = 10;
        const float MaxSpawnNum = 200;
        const float spawnRadiusMax = 12.0f;
        var waitTime = new WaitForSeconds(spawnInterval);
        int wave = 0;
        while(true)
        {
            if(wave > 0)
            {
                // 生成数を wave から決定（最大100体）
                float spawnNum = Mathf.Min(wave * 10, MaxSpawnNum);
                // 等間隔に配置
                float angleRange = 360 / spawnNum;
                // プレイヤー位置
                Vector2 playerPos = playerController.GetPlayer().Position;

                // 生成
                for(int i = 0; i < spawnNum; i++)
                {
                    // 配置半径を決定
                    float r = spawnRadiusMax + UnityEngine.Random.Range(-1.0f, 1.0f);
                    // 配置角度を決定
                    float angle = angleRange * i;

                    // 生成
                    spawn(playerPos, r, angle);
                }
            }
            wave++;
            yield return waitTime;
        }
    }

    /// <summary>
    /// エネミー生成
    /// </summary>
    /// <param name="playerPos">プレイヤー位置</param>
    /// <param name="r">配置半径</param>
    /// <param name="angle">配置角度</param>
    private void spawn(Vector2 playerPos, float r, float angle)
    {
        no++;

        // プレイヤー位置を中心として、ランダムな位置に生成
        Vector2 pos = new Vector2(playerPos.x + r * Mathf.Cos(angle), playerPos.y + r * Mathf.Sin(angle));

        // ランダムなエネミーを生成
        GameObject enemy = Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)], pos, Quaternion.identity);
        enemy.transform.parent = parent.transform;
        enemy.name = "Enemy_" + no;
        enemies.Add(enemy.gameObject);


        // エネミーの向きを設定
        enemy.GetComponent<Enemy>().Init(playerController.GetPlayer(), expController);

        // エネミーの削除を監視し、削除されたらリストから削除
        enemy.GetComponent<Enemy>().OnDead.AddListener(() => Remove(enemy));
    }

    /// <summary>
    /// エネミー削除
    /// </summary>
    /// <param name="enemy">エネミーオブジェクト</param>
    public void Remove(GameObject enemy)
    {
        enemies.Remove(enemy);        
    }

    /// <summary>
    /// プレイヤー位置から最も近いくかつ、射程距離内のエネミーを取得
    /// </summary>
    /// <param name="pos">プレイヤー位置</param>
    /// <param name="range">射程距離</param>
    /// <returns>エネミーオブジェクト</returns>
    public GameObject GetNearestEnemy(Vector3 pos, float range)
    {
        GameObject nearest = null;
        float min = float.MaxValue;
        foreach(var enemy in enemies)
        {
            // エネミーが死んでいる場合は無視
            if(enemy.GetComponent<Enemy>().CurStatus == Enemy.Status.Dead) continue;
            // プレイヤーとエネミーの距離を計算
            float dist = Vector3.Distance(pos, enemy.transform.position);
            if(dist < min)
            {
                min = dist;
                nearest = enemy;
            }
        }
        if(min < range)
        {
            return nearest;
        }
        return null;
    }


}
