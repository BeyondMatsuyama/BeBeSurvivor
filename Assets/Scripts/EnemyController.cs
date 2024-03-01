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
    private const float spawnInterval = 3.0f;

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
            // 常に2対のエネミーが存在するように生成する
            if(enemies.Count < 2)
            {
                // ランダムな位置に生成
                Vector2 pos = new Vector2(UnityEngine.Random.Range(-spawnRadius, spawnRadius), UnityEngine.Random.Range(-spawnRadius, spawnRadius));

                // ランダムなエネミーを生成
                GameObject enemy = Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)], pos, Quaternion.identity);
                enemy.transform.parent = parent.transform;
                enemies.Add(enemy.gameObject);

                // エネミーの向きを設定
                enemy.GetComponent<Enemy>().SetDirection(playerController.GetPosition());

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

}
