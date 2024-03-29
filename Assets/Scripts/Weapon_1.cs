using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器１（デフォルト武器）
/// </summary>
public class Weapon_1 : WeaponBase
{
    // 弾の発射間隔（３レベル）
    private readonly float[] interval = { 2.0f, 1.75f, 1.5f };
    // 弾の射程（３レベル）
    private readonly float[] range = { 4.0f, 6.0f, 8.0f };
    // 同時発射数（３レベル）
    private readonly int[] simultaneous = { 1, 3, 5 };
    // 発射角度（5方向：simultaneous の数に依存する）
    private readonly float[] angle = { 0, 10, -10, 20, -20 };

    /// <summary>
    /// 起動時処理
    /// </summary>
    void Start()
    {
        // デフォルトでアクティブ状態
        Activate();
    }

    /// <summary>
    /// フレームワーク
    /// </summary>
    void Update()
    {
        // アクティブでない場合は処理しない
        if (!isActive) return;
        // ポーズ中は無視
        if (GameController.isPause) return;
        
        // インターバルタイマー更新
        intervalTimer += Time.deltaTime;
        // インターバルチェック
        if (intervalTimer > interval[GetLevel()])
        {
            // インターバルタイマー初期化
            intervalTimer = 0;

            // 射程内のエネミーに向けて弾発射
            var enemy = enemyController.GetNearestEnemy(playerController.GetPosition(), range[GetLevel()]);
            Vector2 dir;
            if(enemy)   // エネミーが存在する場合
            {
                dir = enemy.transform.position - playerController.GetPosition();
                playerController.SetDirection(dir.normalized);   // プレイヤーの向きを設定
            }
            else        // エネミーが存在しない場合
            {
                dir = playerController.GetDirection();
            }

            // 弾発射
            fire(dir.normalized);
        }
    }

    /// <summary>
    /// 球発射
    /// </summary>
    /// <param name="dir">ベースの発射方向</param>
    private void fire(Vector2 dir)
    {
        // 同時発射数分発射
        for (int i = 0; i < simultaneous[GetLevel()]; i++)
        {
            // 発射角度に応じて dir を変更
            Vector2 d = Quaternion.Euler(0, 0, angle[i]) * dir;

            // 弾の生成
            GameObject bullet = Instantiate(prefab, parent);
            // 弾の初期化
            bullet.GetComponent<Bullet>().Initialize(playerController.GetPosition(), d, range[GetLevel()]);
        }
    }



}
