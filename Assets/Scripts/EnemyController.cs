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

    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject parent;

    private const float spawnRadius = 5.0f;
    private const float spawnInterval = 1.0f;
    private const int MAX_ENEMYS = 5;

    private List<GameObject> enemies = new List<GameObject>();

    /// <summary>
    /// 起動時処理
    /// </summary>
    void Start()
    {
        StartCoroutine("spawn");
    }

    /// <summary>
    /// エネミー生成
    /// </summary>
    private IEnumerator spawn()
    {
        var waitTime = new WaitForSeconds(spawnInterval);
        while(true)
        {
            // 常に MAX体のエネミーが存在するように生成する
            if(enemies.Count < MAX_ENEMYS)
            {
                // ランダムな位置に生成
                Vector2 pos = new Vector2(UnityEngine.Random.Range(-spawnRadius, spawnRadius), UnityEngine.Random.Range(-spawnRadius, spawnRadius));

                // ランダムなエネミーを生成
                GameObject enemy = Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)], pos, Quaternion.identity);
                enemy.transform.parent = parent.transform;
                enemies.Add(enemy.gameObject);

                // エネミーの向きを設定
                enemy.GetComponent<Enemy>().SetEnemyDirection(playerController.GetPosition());

                // エネミーの削除を監視し、削除されたらリストから削除
                enemy.GetComponent<Enemy>().OnDead.AddListener(() => Remove(enemy));
            }
            yield return waitTime;
        }
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
