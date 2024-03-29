using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器１（デフォルト武器）
/// </summary>
public class Weapon_1 : MonoBehaviour
{
    // プレイヤー制御
    [SerializeField] private PlayerController playerController;

    // 弾のプレハブ
    [SerializeField] private GameObject bulletPrefab;
    // 弾オブジェクトを配置する親オブジェクト
    [SerializeField] private Transform bulletParent;

    // 弾の発射間隔（３レベル）
    private readonly float[] interval = { 1.5f, 1.0f, 0.5f };
    // 弾の射程（３レベル）
    private readonly float[] range = { 4.0f, 6.0f, 8.0f };
    // 同時発射数（３レベル）
    private readonly int[] simultaneous = { 1, 2, 3 };

    // 武器レベル
    private int level = 0;

    // インターバルタイマー
    private float intervalTimer = 0;

    // フレームワーク
    private void Update()
    {
        // インターバルタイマー更新
        intervalTimer += Time.deltaTime;
        // インターバルチェック
        if (intervalTimer > interval[level])
        {
            // インターバルタイマー初期化
            intervalTimer = 0;
            // 弾発射
            fire();
        }
    }

    // 弾発射
    private void fire()
    {
        // 同時発射数分発射
        for (int i = 0; i < simultaneous[level]; i++)
        {
            // 弾の生成
            GameObject bullet = Instantiate(bulletPrefab, bulletParent);
            // 弾の初期化
            bullet.GetComponent<Bullet>().Initialize(playerController.GetPosition(), playerController.GetDirection(), range[level]);            
        }
    }

}
